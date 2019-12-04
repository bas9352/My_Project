using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Microsoft.Xrm.Sdk;
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
    public class TopUpViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();

        public bool grid_top_up_check { get; set; } = true;
        DataTable data { get; set; }

        public string member_id { get; set; } = null;
        public string coupon_id { get; set; } = null;
        public string bonus_status { get; set; } = null;
        public string txt_username { get; set; } = null;
        public string txt_add_hh { get; set; } = "0 h";
        public string txt_add_mm { get; set; } = "0 m";
        public string bonus_id { get; set; } = null;
        public string txt_add_money { get; set; } = null;
        public string txt_remaining_hh { get; set; } = null;
        public string txt_remaining_mm { get; set; } = null;
        public string txt_total_remaining_amount { get; set; } = null;
        public string remaining_money { get; set; } = null;
        public string member_total_real_amount { get; set; } = null;
        public string seconds { get; set; } = null;
        public string new_seconds { get; set; } = null;
        public string group_rate { get; set; } = null;
        public string member_bonus { get; set; } = null;
        public string remaining_real_money { get; set; } = null;
        public int ordinal { get; set; }

        private string hr, min;


        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand add_money_change { get; set; }
        public ICommand add_hh_change { get; set; }
        public ICommand add_mm_change { get; set; }
        #endregion

        #region Constructor
        public TopUpViewModel()
        {
            add_money_change = new RelayCommand(p => GoAddMoneyChanged(p));
            add_hh_change = new RelayCommand(p => GoAddHHChanged(p));
            add_mm_change = new RelayCommand(p => GoAddMMChanged(p));

            btn_ok = new RelayCommand(p=>
            {
                grid_top_up_check = false;

                if (txt_add_money == null || txt_add_money == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (bonus_status == "false")
                    {
                        if (IsInsert() == true)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                            DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                        }
                    }
                    else
                    {
                        if (IsSelect() == true)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["bonus_top_up"].ToString() == txt_add_money)
                                {
                                    num = 1;
                                    bonus_id = row["bonus_id"].ToString();
                                    if (IsInsert() == true)
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                                        DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                                    }
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
                    }
                }
            });
            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                IsClear();
            });
        }
        #endregion

        #region other method
        public bool IsSelect()
        {
            string query = $"select * from bonus order by bonus_id;";
            try
            {
                if (OpenConnection() == true)
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

        public void IsClear()
        {
            txt_username = null;
            txt_add_hh = "0 h";
            txt_add_mm = "0 m";
            txt_add_money = null;
            txt_remaining_hh = null;
            txt_remaining_mm = null;
            txt_total_remaining_amount = null;
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_top_up_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
                if (IoC.MainView.CurrPage == ApplicationPage.Control)
                {
                    IoC.ControlView.item_online.Execute(IoC.ControlView.online_data);
                }
                else if (IoC.MainView.CurrPage == ApplicationPage.MemberCoupon)
                {
                    if (IoC.MemberCouponView.CurrPage == ApplicationPage.Member)
                    {
                        IoC.MemberView.item_member.Execute(IoC.MemberView.member_data);
                    }
                    else
                    {
                        IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
                    }
                }
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_top_up_check = true;
            }
        }

        string query;
        public bool IsInsert()
        {
            if (IoC.MemberCouponView.CurrPage == ApplicationPage.Member)
            {
                query = $"insert into member_top_up set " +
                        $"mt_member_id = '{member_id}', " +
                        $"mt_by = '{IoC.LoginView.login_id}', " +
                        $"mt_ordinal = '{ordinal + 1}', " +
                        $"mt_real_amount = '{txt_add_money}', " +
                        $"mt_bonus_id = '{bonus_id}', " +
                        $"mt_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";
            }
            else if (IoC.MemberCouponView.CurrPage == ApplicationPage.Coupon)
            {
                query = $"insert into coupon_top_up set " +
                        $"ct_coupon_id = '{coupon_id}', " +
                        $"ct_by = '{IoC.LoginView.login_id}', " +
                        $"ct_ordinal = '{ordinal + 1}', " +
                        $"ct_real_amount = '{txt_add_money}', " +
                        $"ct_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";
            }

            try
            {
                if (OpenConnection() == true)
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

        public void GoAddHHChanged(object p)
        {
            var item = p as TextBox;

            if (item.IsFocused)
            {
                if (txt_add_mm == "" || txt_add_mm == null)
                {
                    min = "0";
                }
                else
                {
                    string[] add_min = txt_add_mm.Split(" m".ToCharArray());
                    min = add_min[0];
                }

                if (txt_add_hh == "" || txt_add_hh == null)
                {
                    hr = "0";
                }
                else
                {
                    string[] add_hr = txt_add_hh.Split(" h".ToCharArray());
                    hr = add_hr[0];
                }

                txt_add_hh = string.Format("{0:0}" + " h", hr);
                txt_add_mm = string.Format("{0:0}" + " m", min);

                if (txt_add_hh != "" && txt_add_mm != "")
                {
                    if (hr == "")
                    {
                        hr = "0";
                    }
                    new_seconds = ((int.Parse(hr) * 3600) + (int.Parse(min) * 60)).ToString();
                    txt_add_money = Math.Round((float.Parse(new_seconds) / float.Parse(group_rate)), 2).ToString();
                    txt_add_money = (float.Parse(new_seconds) / float.Parse(group_rate)).ToString();
                }
                IsChanged();
            }
        }

        public void GoAddMMChanged(object p)
        {
            var item = p as TextBox;

            if (item.IsFocused)
            {
                if (txt_add_mm == "" || txt_add_mm == null)
                {
                    min = "0";
                }
                else
                {
                    string[] add_min = txt_add_mm.Split(" m".ToCharArray());
                    min = add_min[0];
                }

                if (txt_add_hh == "" || txt_add_hh == null)
                {
                    hr = "0";
                }
                else
                {
                    string[] add_hr = txt_add_hh.Split(" h".ToCharArray());
                    hr = add_hr[0];
                }

                txt_add_hh = string.Format("{0:0}" + " h", hr);
                txt_add_mm = string.Format("{0:0}" + " m", min);

                if (txt_add_hh != "" && txt_add_mm != "")
                {
                    if (min == "")
                    {
                        min = "0";
                    }
                    new_seconds = ((int.Parse(hr) * 3600) + (int.Parse(min) * 60)).ToString();
                    txt_add_money = Math.Round((float.Parse(new_seconds) / float.Parse(group_rate)), 2).ToString();
                    txt_add_money = (float.Parse(new_seconds) / float.Parse(group_rate)).ToString();
                }
                IsChanged();
            }
        }

        public void GoAddMoneyChanged(object p)
        {
            var item = p as TextBox;
            if (item.Text == null || item.Text == "")
            {
                txt_add_money = "";
                new_seconds = (float.Parse(group_rate) * 0).ToString();
                txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(new_seconds) / 3600));
                txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(new_seconds) / 60) % 60));
            }
            else
            {
                new_seconds = (float.Parse(group_rate) * float.Parse(txt_add_money)).ToString();
                txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(new_seconds) / 3600));
                txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(new_seconds) / 60) % 60));
            }
            IsChanged();
        }

        public void IsChanged()
        {
            var member_total_seconds = float.Parse(new_seconds) + float.Parse(seconds);
            txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(member_total_seconds / 3600));
            txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((member_total_seconds / 60) % 60));

            if (txt_add_money == "")
            {
                txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(remaining_money));
            }
            else
            {
                txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(remaining_money) + float.Parse(txt_add_money));
            }
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
