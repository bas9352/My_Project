using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class ControlViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();

        DispatcherTimer online_timer = new DispatcherTimer();
        public DataRowView online_item { get; set; } = null;
        public int online_index { get; set; }
        public DataGrid online_data { get; set; } = null;

        public string lab_username { get; set; }
        public string lab_start_time { get; set; }
        public string lab_use_time { get; set; }
        public string lab_remaining_time { get; set; }
        public string lab_group_rate { get; set; }

        #endregion

        #region Commands
        public ICommand btn_top_up { get; set; }
        public ICommand btn_free_top_up { get; set; }
        public ICommand btn_debt { get; set; }
        public ICommand item_online { get; set; }
        public ICommand item_online_changed { get; set; }
        #endregion

        #region Constructor
        public ControlViewModel()
        {
            item_online = new RelayCommand(p => GoItemOnline(p));
            item_online_changed = new RelayCommand(p => GoItemOnlineChanged(p));

            btn_top_up = new RelayCommand(p =>
            {
                if (online_item != null)
                {
                    if (online_item["v_all_type_name"].ToString() == "member")
                    {
                        IoC.TopUpView.member_id = online_item["v_online_member_id"].ToString();
                        IoC.TopUpView.bonus_status = online_item["v_group_bonus_status"].ToString();//สถานะโบนัส เปิด หรือปิด
                        IoC.TopUpView.group_rate = (3600 / double.Parse(online_item["v_all_group_rate"].ToString())).ToString();//เรทราคา
                        IoC.TopUpView.txt_username = online_item["v_all_username"].ToString();
                        IoC.TopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(online_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.TopUpView.remaining_money = online_item["v_all_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.TopUpView.seconds = (double.Parse(IoC.TopUpView.group_rate) * double.Parse(IoC.TopUpView.remaining_money)).ToString();
                        IoC.TopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(double.Parse(IoC.TopUpView.seconds) / 3600));
                        IoC.TopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((double.Parse(IoC.TopUpView.seconds) / 60) % 60));
                        IoC.TopUpView.ordinal = int.Parse(online_item["v_all_ordinal_last"].ToString());
                        DialogHost.Show(new TopUpView(), "Main");
                    }
                    else if (online_item["v_all_type_name"].ToString() == "coupon")
                    {
                        IoC.TopUpView.coupon_id = online_item["v_online_coupon_id"].ToString();
                        IoC.TopUpView.bonus_status = "false";
                        IoC.TopUpView.group_rate = (3600 / double.Parse(online_item["v_all_group_rate"].ToString())).ToString();//เรทราคา
                        IoC.TopUpView.txt_username = online_item["v_all_username"].ToString();
                        IoC.TopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(online_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.TopUpView.remaining_money = online_item["v_all_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.TopUpView.seconds = (double.Parse(IoC.TopUpView.group_rate) * double.Parse(IoC.TopUpView.remaining_money)).ToString();
                        IoC.TopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(double.Parse(IoC.TopUpView.seconds) / 3600));
                        IoC.TopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((double.Parse(IoC.TopUpView.seconds) / 60) % 60));
                        IoC.TopUpView.ordinal = int.Parse(online_item["v_all_ordinal_last"].ToString());
                        DialogHost.Show(new TopUpView(), "Main");
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("not_group_can_top_up");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
            });

            btn_free_top_up = new RelayCommand(p =>
            {
                if (online_item != null)
                {
                    if (online_item["v_all_type_name"].ToString() == "member")
                    {
                        IoC.FreeTopUpView.member_id = online_item["v_online_member_id"].ToString();
                        IoC.FreeTopUpView.group_rate = (3600 / double.Parse(online_item["v_all_group_rate"].ToString())).ToString();
                        IoC.FreeTopUpView.remaining_free_money = online_item["v_all_remaining_free_amount"].ToString();//เงินฟรีคงเหลือ
                        IoC.FreeTopUpView.txt_username = online_item["v_all_username"].ToString();

                        IoC.FreeTopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(online_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.FreeTopUpView.remaining_money = online_item["v_all_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.FreeTopUpView.seconds = (double.Parse(IoC.FreeTopUpView.group_rate) * double.Parse(IoC.FreeTopUpView.remaining_money)).ToString();
                        IoC.FreeTopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(double.Parse(IoC.FreeTopUpView.seconds) / 3600));
                        IoC.FreeTopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((double.Parse(IoC.FreeTopUpView.seconds) / 60) % 60));
                        IoC.FreeTopUpView.ordinal = int.Parse(online_item["v_all_ordinal_last"].ToString());
                        DialogHost.Show(new FreeTopUpView(), "Main");
                    }
                    else if (online_item["v_all_type_name"].ToString() == "coupon")
                    {
                        IoC.FreeTopUpView.coupon_id = online_item["v_online_coupon_id"].ToString();
                        IoC.FreeTopUpView.group_rate = (3600 / double.Parse(online_item["v_all_group_rate"].ToString())).ToString();
                        IoC.FreeTopUpView.remaining_free_money = online_item["v_all_remaining_free_amount"].ToString();//เงินฟรีคงเหลือ
                        IoC.FreeTopUpView.txt_username = online_item["v_all_username"].ToString();

                        IoC.FreeTopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(online_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.FreeTopUpView.remaining_money = online_item["v_all_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.FreeTopUpView.seconds = (double.Parse(IoC.FreeTopUpView.group_rate) * double.Parse(IoC.FreeTopUpView.remaining_money)).ToString();
                        IoC.FreeTopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(double.Parse(IoC.FreeTopUpView.seconds) / 3600));
                        IoC.FreeTopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((double.Parse(IoC.FreeTopUpView.seconds) / 60) % 60));
                        IoC.FreeTopUpView.ordinal = int.Parse(online_item["v_all_ordinal_last"].ToString());
                        DialogHost.Show(new FreeTopUpView(), "Main");
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("not_group_can_top_up");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
            });

            btn_debt = new RelayCommand(p =>
            {
                if (online_item != null)
                {
                    if (online_item["v_all_type_name"].ToString() == "member")
                    {
                        IoC.DebtView.CurrPage = ApplicationPage.Lend;
                        //เพิ่มหนี้
                        IoC.LendView.member_id = online_item["v_online_member_id"].ToString();
                        IoC.LendView.group_rate = (3600 / double.Parse(online_item["v_all_group_rate"].ToString())).ToString();
                        IoC.LendView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(online_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.LendView.remaining_money = online_item["v_all_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.LendView.txt_credit_limit = online_item["v_member_credit_limit"].ToString();//ยืมได้ไม่เกิน
                        IoC.LendView.txt_username = online_item["v_all_username"].ToString();
                        IoC.LendView.txt_debt = online_item["v_member_total_debt_remaining_amount"].ToString();
                        IoC.LendView.member_seconds = (double.Parse(IoC.LendView.group_rate) * double.Parse(IoC.LendView.remaining_money)).ToString();
                        IoC.LendView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(double.Parse(IoC.LendView.member_seconds) / 3600));
                        IoC.LendView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((double.Parse(IoC.LendView.member_seconds) / 60) % 60));
                        IoC.LendView.ordinal = int.Parse(online_item["v_all_ordinal"].ToString());
                        //ชำระหนี้
                        IoC.PayDebtView.member_id = online_item["v_online_member_id"].ToString();
                        IoC.PayDebtView.txt_username = online_item["v_all_username"].ToString();
                        IoC.PayDebtView.txt_debt = online_item["v_member_total_debt_remaining_amount"].ToString();
                        IoC.PayDebtView.ordinal = int.Parse(online_item["v_all_ordinal"].ToString());
                        DialogHost.Show(new DebtView(), "Main");
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("not_group_can_top_up");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
            });

            online_timer.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            online_timer.Tick += online_timer_tick;
            online_timer.Start();
        }
        #endregion

        #region other method
        private void online_timer_tick(object sender, EventArgs e)
        {
            if (IoC.MainView.CurrPage == ApplicationPage.Control)
            {
                if (online_data != null)
                {
                    lab_username = null;
                    lab_start_time = null;
                    lab_use_time = null;
                    lab_remaining_time = null;
                    lab_group_rate = null;
                    item_online.Execute(online_data);
                }
            }
            else
            {
                online_timer.Stop();
            }
            
        }
        private void GoItemOnline(object p)
        {
            IsClear();
            online_data = p as DataGrid;
            string query = $"select * " +
                           $"from v_computer_status";

            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt); 
                    Sconn.conn.Close();
                    dt.Columns.Add("v_all_remaining_time", typeof(string));
                    dt.Columns.Add("v_all_use_remaining_time", typeof(string));
                    dt.Columns.Add("new_v_group_rate", typeof(string));

                    foreach (DataRow data in dt.Rows)
                    {
                        data["v_all_remaining_time"] = string.Format("{0:00:}{1:00}", data["v_all_hr"], data["v_all_mn"]);
                        data["v_all_use_remaining_time"] = string.Format("{0:00:}{1:00}", data["v_all_use_hr"], data["v_all_use_mn"]);
                        data["new_v_group_rate"] = string.Format("{0:#,##0.##}", data["v_all_group_rate"]);
                    }
                    online_data.ItemsSource = dt.DefaultView;
                    
                }
                else
                {
                    Sconn.conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
            }
            
        }

        private void GoItemOnlineChanged(object p)
        {
            var item = p as DataGrid;
            online_item = item.SelectedItem as DataRowView;
            online_index = item.SelectedIndex;
            Task.Factory.StartNew(async () =>
            {
                online_timer.Stop();

                lab_username = online_item["v_all_username"].ToString();
                lab_start_time = online_item["v_online_s_time"].ToString();
                lab_use_time = online_item["v_all_use_remaining_time"].ToString();
                lab_remaining_time = online_item["v_all_remaining_time"].ToString();
                lab_group_rate = online_item["new_v_group_rate"].ToString();
                await Task.Delay(15000);
            }).ContinueWith((previousTask) => {
                online_timer.Start();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void IsClear()
        {
            online_item = null;
            online_index = 0;
        }

        private bool OpenConnection()
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
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
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
