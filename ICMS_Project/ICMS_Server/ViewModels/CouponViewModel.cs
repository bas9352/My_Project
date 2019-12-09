using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class CouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DataRowView coupon_item { get; set; }
        public DataTable data_ct { get; set; }
        public DataTable data_online { get; set; }
        public DataGrid coupon_data { get; set; }
        public DataRow ct_data { get; set; }
        public DataRow online_data { get; set; }
        public int coupon_index { get; set; }

        #endregion

        #region Commands
        public ICommand item_coupon { get; set; }
        public ICommand item_coupon_change { get; set; }

        #endregion

        #region Constructor
        public CouponViewModel()
        {
            item_coupon_change = new RelayCommand(p => GoItemCouponChanged(p));
            item_coupon = new RelayCommand(p => GoItemCoupon(p));
        }

        private void GoItemCoupon(object p)
        {
            if (IoC.MemberCouponView.cmb.ItemsSource == null)
            {
                IoC.MemberCouponView.cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("coupon_name"), "coupon_name")
                };
                IoC.MemberCouponView.cmb.ItemsSource = IoC.MemberCouponView.cmb_data;
                IoC.MemberCouponView.cmb.SelectedValuePath = "Value";
                IoC.MemberCouponView.cmb.DisplayMemberPath = "Key";
                IoC.MemberCouponView.cmb.SelectedIndex = 0;
            }

            IsClear();
            coupon_data = p as DataGrid;
            string query;
            if (IoC.MemberCouponView.txt_search == null || IoC.MemberCouponView.txt_search == "")
            {
                query = $"select * " +
                        $"from v_all_customer " +
                        $"where v_all_type_name = 'coupon'  " +
                        $"order by v_all_username";
            }
            else
            {
                query = $"select * " +
                        $"from v_all_customer " +
                        $"where v_all_type_name = 'coupon' " +
                        $"and v_coupon_username like '%{IoC.MemberCouponView.txt_search}%' " +
                        $"order by v_coupon_username";

            }
            IoC.MemberCouponView.txt_search_null = null;
            IoC.MemberCouponView.txt_search = null;
            
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();

                dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                foreach (DataRow data in dt.Rows)
                {
                    data["new_v_all_remaining_amount"] = string.Format("{0:#,##0.##}", double.Parse(data["v_all_remaining_amount"].ToString()));
                }
                coupon_data.ItemsSource = dt.DefaultView;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", conn_fail);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", conn_fail);
                }
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        private void conn_fail(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.DialogHostMsg = false;
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {
                    item_coupon.Execute(coupon_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void GoItemCouponChanged(object p)
        {
            var item = p as DataGrid;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_index = item.SelectedIndex;
        }

        public void IsClear()
        {
            coupon_item = null;
            coupon_index = 0;
        }

        public bool OpenConnection()
        {
            try
            {
                Sconn.conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                return false;
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
