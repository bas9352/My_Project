using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
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
    public class AddEditOptionCouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public bool add_edit_coupon_check { get; set; } = true;
        public DataTable data { get; set; }
        public DataRowView group_item { get; set; }
        public ComboBox group_data { get; set; }
        public string op_c_id { get; set; } = null;
        public string txt_username { get; set; } = null;
        public string group_id { get; set; } = null;
        public string txt_hr_rate { get; set; } = null;
        public string txt_hr_price { get; set; } = null;
        public string txt_free_money { get; set; } = null;
        public string txt_total_amount { get; set; } = "0";
        public string txt_add_hh { get; set; } = "0 h";
        public string txt_add_mm { get; set; } = "0 m";
        public string txt_exp_date { get; set; } = null;
        public bool start_create_date { get; set; } = true;
        public bool start_first_use { get; set; } = false;
        public string txt_address { get; set; } = null;
        public string txt_id_card { get; set; } = null;

        public string seconds { get; set; }

        public string g_hr_price = "0";
        public string g_free_money = "0";



        #endregion

        #region Commands
        public ICommand price { get; set; }
        public ICommand free_money { get; set; }
        public ICommand total_amount { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        public ICommand item_group { get; set; }
        public ICommand item_group_change { get; set; }
        #endregion

        #region Constructor
        public AddEditOptionCouponViewModel()
        {
            item_group = new RelayCommand(p=>GoItemGroup(p));
            item_group_change = new RelayCommand(p => GoItemGroupChanged(p));
            price = new RelayCommand(p=>GoPrice(p));
            free_money = new RelayCommand(p => GoFreeMoney(p));
            total_amount = new RelayCommand(p => GoTotalAmount(p));

            btn_ok = new RelayCommand(p=>
            {
                add_edit_coupon_check = false;

                if (txt_username == null || txt_username == "" ||
                txt_hr_price == null || txt_hr_price == "" ||
                txt_exp_date == null || txt_exp_date == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        int num = 0;

                        if (op_c_id == null)
                        {
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_op_c_name"].ToString() == txt_username)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("coupon_name_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                            }

                            if (num == 0)
                            {
                                if (IsInsert() == true)
                                {
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                                    DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_op_c_name"].ToString() == txt_username && 
                                    row["v_op_c_id"].ToString() != op_c_id)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("coupon_name_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                                else if (row["v_op_c"].ToString() == "true" &&
                                        row["v_op_c_id"].ToString() == op_c_id &&
                                        row["v_op_c_real_amount"].ToString() != txt_hr_price)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("cant_edit_price");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                            }

                            if (num == 0)
                            {
                                if (IsUpdate() == true)
                                {
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("edit_success");
                                    DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                                }
                            }
                        }
                    }
                }
            });

            btn_cancel = new RelayCommand(p =>
            {
                IsClear();
                IoC.Application.DialogHostInMain = false;
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new OptionCouponView(), "Main");
            });

        }
        #endregion

        #region Other method
        public bool IsInsert()
        {
            string query = $"insert into option_coupon set " +
                           $"op_c_name = '{txt_username}', " +
                           $"op_c_real_amount = '{txt_hr_price}', " +
                           $"op_c_free_amount = '{txt_free_money}', " +
                           $"op_c_s_date = '{start_create_date}', " +
                           $"op_c_e_date = '{txt_exp_date}', " +
                           $"group_id = '{group_id}' ";
            try
            {
                Sconn.conn.Open();
                if (Sconn.conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();
                    return true;
                }
                else
                {
                    Sconn.conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }            
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                    add_edit_coupon_check = true;
                }).ContinueWith((previousTask) =>
                {

                    IoC.Application.DialogHostMain = false;
                    IoC.Application.DialogHostInMain = false;
                    DialogHost.Show(new OptionCouponView(), "Main");

                    IsClear();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                add_edit_coupon_check = true;
            }
        }

        public bool IsUpdate()
        {
            string query = $"update option_coupon set " +
                           $"op_c_name='{txt_username}' , " +
                           $"op_c_real_amount='{txt_hr_price}' , " +
                           $"op_c_free_amount='{txt_free_money}' , " +
                           $"op_c_s_date='{start_create_date}' , " +
                           $"op_c_e_date='{txt_exp_date}' , " +
                           $"group_id='{group_id}' " +
                           $"where op_c_id = '{op_c_id}'";

            try
            {
                Sconn.conn.Open();
                if (Sconn.conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();
                    return true;
                }
                else
                {
                    Sconn.conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
        }

        public void IsClear()
        {
            data = null;
            group_item = null;
            txt_username = null;
            group_id = null;
            txt_hr_rate = null;
            txt_hr_price = null;
            txt_free_money = null;
            //txt_total_amount = "0";
            txt_add_hh = "0 h";
            txt_add_mm = "0 m";
            txt_exp_date = null;
            start_create_date = true;
            start_first_use = false;
            //seconds = null;
            g_hr_price = "0";
            g_free_money = "0";
    }

        private void GoPrice(object p)
        {
            var item = p as TextBox;
            if (item.IsFocused == true)
            {
                if (txt_hr_price == "" || txt_hr_price == null)
                {
                    g_hr_price = "0";
                }
                else
                {
                    g_hr_price = txt_hr_price;
                }

                if (txt_free_money == "" || txt_free_money == null)
                {
                    g_free_money = "0";
                }
                else
                {
                    g_free_money = txt_free_money;
                }

                if (txt_hr_price != "" || txt_hr_price != null&&
                    txt_free_money != "" ||txt_free_money != null)
                {
                    IsChanged();
                }
            }
        }
        private void GoFreeMoney(object p)
        {
            var item = p as TextBox;
            if (item.IsFocused == true)
            {
                if (txt_hr_price == "" || txt_hr_price == null)
                {
                    g_hr_price = "0";
                }
                else
                {
                    g_hr_price = txt_hr_price;
                }

                if (txt_free_money == "" || txt_free_money == null)
                {
                    g_free_money = "0";
                }
                else
                {
                    g_free_money = txt_free_money;
                }

                if (txt_hr_price != "" || txt_hr_price != null &&
                    txt_free_money != "" || txt_free_money != null)
                {
                    IsChanged();
                }
            }
        }

        public void IsChanged()
        {
            txt_total_amount = (float.Parse(g_hr_price) + float.Parse(g_free_money)).ToString();
        }

        private void GoTotalAmount(object p)
        {
            seconds = ((3600 / float.Parse(txt_hr_rate)) * float.Parse(txt_total_amount)).ToString();
            txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(seconds) / 3600));
            txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(seconds) / 60) % 60));
        }

        public void GoItemGroup(object p)
        {
            group_data = p as ComboBox;
            string query = $"select * " +
                           $"from user_group " +
                           $"inner join type " +
                           $"on type.type_id = user_group.type_id " +
                           $"where type_name like '%coupon%' " +
                           $"order by group_id";

            try
            {
                Sconn.conn.Open();
                if (Sconn.conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "user_group");
                    group_data.ItemsSource = ds.Tables[0].DefaultView;
                    group_data.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
                    group_data.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
                    group_data.SelectedIndex = 0;
                    group_item = group_data.SelectedItem as DataRowView;
                    if (group_id == null)
                    {
                        group_id = group_item["group_id"].ToString();
                        txt_hr_rate = group_item["group_rate"].ToString();
                    }
                    else
                    {
                        group_data.SelectedValue = group_id;
                        txt_hr_rate = group_item["group_rate"].ToString();
                    }
                    Sconn.conn.Close();
                    add_edit_coupon_check = true;
                }
                else
                {
                    Sconn.conn.Close();
                    add_edit_coupon_check = false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
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
        }

        public void GoItemGroupChanged(object p)
        {
            var item = p as ComboBox;
            group_item = item.SelectedItem as DataRowView;
            group_id = group_item["group_id"].ToString();
            txt_hr_rate = group_item["group_rate"].ToString();
            txt_total_amount = "0";
            IsChanged();
        }
                
        public bool IsSelect()
        {
            string query = $"select * " +
                           $"from v_op_c";

            try
            {
                Sconn.conn.Open();
                if (Sconn.conn.State == ConnectionState.Open)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data = dt;
                    Sconn.conn.Close();
                    return true;
                }
                else
                {
                    Sconn.conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
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
                    item_group.Execute(group_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
