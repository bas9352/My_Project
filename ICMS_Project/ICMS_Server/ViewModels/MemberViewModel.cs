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
    public class MemberViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DataRowView member_item { get; set; }
        public DataTable data_mt { get; set; }
        public DataTable data_online { get; set; }
        public DataGrid member_data { get; set; }
        public DataRow mt_data { get; set; }
        public DataRow online_data { get; set; }
        public int member_index { get; set; }

        #endregion

        #region Commands
        public ICommand item_member { get; set; }
        public ICommand item_member_change { get; set; }

        #endregion

        #region Constructor
        public MemberViewModel()
        {
            item_member_change = new RelayCommand(p => GoItemMemberChanged(p));
            item_member = new RelayCommand(p => GoItemMember(p));
        }
        #endregion

        #region Other method
        string query;
        public void GoItemMember(object p)
        {
            IoC.MemberCouponView.cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("username"), "username"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("name"), "name"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("lastname"), "lastname")
                };
            IoC.MemberCouponView.cmb.ItemsSource = IoC.MemberCouponView.cmb_data;
            IoC.MemberCouponView.cmb.SelectedValuePath = "Value";
            IoC.MemberCouponView.cmb.DisplayMemberPath = "Key";
            IoC.MemberCouponView.cmb.SelectedIndex = 0;

            IsClear();
            member_data = p as DataGrid;

            if (IoC.MemberCouponView.txt_search == null || IoC.MemberCouponView.txt_search == "")
            {
                query = $"select * " +
                        $"from v_all_customer " +
                        $"where v_all_type_name = 'member' " +
                        $"order by v_all_username";
            }
            else
            {
                if (IoC.MemberCouponView.search_text == "username")
                {

                    //MessageBox.Show($"{"test1"}");
                    query = $"select * " +
                            $"from v_all_customer " +
                            $"where v_all_type_name = 'member' " +
                            $"and v_all_username like '%{IoC.MemberCouponView.txt_search}%' " +
                            $"order by v_member_name";
                }
                else if (IoC.MemberCouponView.search_text == "name")
                {
                    query = $"select * " +
                            $"from v_all_customer " +
                            $"where v_all_type_name = 'member' and " +
                            $" v_member_name like '%{IoC.MemberCouponView.txt_search}%' " +
                            $"order by v_member_name";
                }
                else if (IoC.MemberCouponView.search_text == "lastname")
                {
                    query = $"select * " +
                            $"from v_all_customer " +
                            $"where v_all_type_name = 'member' and " +
                            $"v_member_lastname like '%{IoC.MemberCouponView.txt_search}%' " +
                            $"order by v_member_lastname";
                }
                IoC.MemberCouponView.txt_search_null = null;
                IoC.MemberCouponView.txt_search = null;
            }

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
                member_data.ItemsSource = dt.DefaultView;
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
                    item_member.Execute(member_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoItemMemberChanged(object p)
        {
            var item = p as DataGrid;
            member_item = item.SelectedItem as DataRowView;
            member_index = item.SelectedIndex;
        }

        public void IsClear()
        {
            member_item = null;
            member_index = 0;
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }

        #endregion
    }

}
