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
    public class PayDebtViewModel :BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public bool grid_p_check { get; set; } = true;
        public string txt_username { get; set; }
        public string txt_debt { get; set; }
        public string member_id { get; set; }
        public string txt_pay_debt { get; set; }
        public int ordinal { get; set; }
        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        #endregion

        #region Constructor
        public PayDebtViewModel()
        {
            btn_ok = new RelayCommand(p=> 
            {
                grid_p_check = false;
                if (txt_pay_debt == null || txt_pay_debt == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (float.Parse(txt_pay_debt) > float.Parse(txt_debt))
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("max_debt");
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

        #region Other method
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_p_check = true;
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
                grid_p_check = true;
            }
        }

        public void IsClear()
        {
            txt_username = null;
            txt_debt = null;
            member_id = null;
            txt_pay_debt = null;
        }

        public bool IsInsert()
        {
            string query = $"insert into member_top_up set " +
                           $"mt_member_id = '{member_id}', " +
                           $"mt_by = '{IoC.LoginView.login_id}', " +
                           $"mt_ordinal = '{ordinal + 1}', " +
                           $"mt_pay_debt = '{txt_pay_debt}', " +
                           $"mt_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";

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
