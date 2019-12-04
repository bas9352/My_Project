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
    public class LendViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public bool grid_l_check { get; set; } = true;

        public string member_id { get; set; } = null;
        public string txt_username { get; set; } = null;
        public string txt_debt { get; set; } = null;
        public string txt_credit_limit { get; set; } = null;
        public string txt_add_hh { get; set; } = "0 h";
        public string txt_add_mm { get; set; } = "0 m";
        public string txt_lend { get; set; } = null;
        public string txt_remaining_hh { get; set; } = null;
        public string txt_remaining_mm { get; set; } = null;
        public string txt_total_remaining_amount { get; set; } = null;
        public string remaining_money { get; set; } = null;
        public string member_seconds { get; set; } = null;
        public string member_new_seconds { get; set; } = null;
        public string group_rate { get; set; } = null;

        public int ordinal { get; set; }

        private string hr, min;

        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand add_money_change { get; set; }
        public ICommand add_hh_change { get; set; }
        public ICommand add_mm_change { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public LendViewModel()
        {
            add_money_change = new RelayCommand(p => GoAddMoneyChanged(p));
            add_hh_change = new RelayCommand(p => GoAddHHChanged(p));
            add_mm_change = new RelayCommand(p => GoAddMMChanged(p));

            btn_ok = new RelayCommand(p =>
            {
                grid_l_check = false;

                if (txt_lend == null || txt_lend == "" || txt_credit_limit == null || txt_credit_limit == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (double.Parse(txt_lend) > (double.Parse(txt_credit_limit)- double.Parse(txt_debt)))
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("max_credit");
                        DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                    }
                    else
                    {
                        if (IsInsert() == true)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                            DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
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
        public void IsClear()
        {
            member_id = null;
            txt_username = null;
            txt_add_hh = "0 h";
            txt_add_mm = "0 m";
            txt_lend = null;
            txt_remaining_hh = null;
            txt_remaining_mm = null;
            txt_total_remaining_amount = null;
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_l_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
                if (IoC.MainView.CurrPage == ApplicationPage.Control)
                {
                    IoC.ControlView.item_online.Execute(IoC.ControlView.online_data);
                }
                else if (IoC.MainView.CurrPage == ApplicationPage.MemberCoupon)
                {
                    IoC.MemberView.item_member.Execute(IoC.MemberView.member_data);
                }
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_l_check = true;
            }
        }

        public bool IsInsert()
        {
            string query = $"insert into member_top_up set " +
                           $"mt_member_id = '{member_id}', " +
                           $"mt_by = '{IoC.LoginView.login_id}', " +
                           $"mt_ordinal = '{ordinal + 1}', " +
                           $"mt_debt_amount = '{txt_lend}', " +
                           $"mt_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}'; " +

                           $"update member set " +
                           $"member_credit_limit = '{txt_credit_limit}' " +
                           $"where member_id = {member_id} ";
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
                    member_new_seconds = ((int.Parse(hr) * 3600) + (int.Parse(min) * 60)).ToString();
                    txt_lend = (float.Parse(member_new_seconds) / float.Parse(group_rate)).ToString();
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
                    member_new_seconds = ((int.Parse(hr) * 3600) + (int.Parse(min) * 60)).ToString();
                    txt_lend = (float.Parse(member_new_seconds) / float.Parse(group_rate)).ToString();
                }
                IsChanged();
            }
            
        }

        public void GoAddMoneyChanged(object p)
        {
            var item = p as TextBox;
            if (item.IsFocused == true && item.Text == null || item.Text == "")
            {
                txt_lend = "";
                member_new_seconds = (float.Parse(group_rate) * 0).ToString();
                txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(member_new_seconds) / 3600));
                txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(member_new_seconds) / 60) % 60));
            }
            else if (item.IsFocused == true && item.Text != "" || item.Text != null)
            {
                member_new_seconds = (float.Parse(group_rate) * float.Parse(txt_lend)).ToString();
                txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(member_new_seconds) / 3600));
                txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(member_new_seconds) / 60) % 60));
            }
            IsChanged();
        }

        public void IsChanged()
        {
            var member_total_seconds = float.Parse(member_new_seconds) + float.Parse(member_seconds);
            txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(member_total_seconds / 3600));
            txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((member_total_seconds / 60) % 60));

            if (txt_lend == "")
            {
                txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(remaining_money));
            }
            else
            {
                txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(remaining_money) + float.Parse(txt_lend));
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
