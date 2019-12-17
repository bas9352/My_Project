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
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class LoginViewModel : BaseView
    {
        #region Public Properties
        Database Sconn = new Database();
        public DataTable data { get; set; }
        public DataRow login_data { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password = null;

        public string username { get; set; }
        public string login_id { get; set; }
        #endregion

        #region Comamnd
        public ICommand btn_login { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            pass = new RelayCommand(p=> GoPassChanged(p));
            btn_login = new RelayCommand(p =>
            {
                if (txt_username == null || txt_password == null ||
                txt_username == "" || txt_password == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_user_pass");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    if (IsLoginCheck() == true && data.Rows.Count > 0)
                    {
                        login_data = data.Rows[0];

                        if (login_data["exp_check"].ToString() == "true")
                        {
                            if (login_data["staff_s_date"].ToString() == "" &&
                                login_data["type_name"].ToString() != "admin")
                            {
                                if (IsUpdate() == true)
                                {
                                    GoToMain();
                                }
                            }
                            else
                            {
                                GoToMain();
                            }
                        }
                        else
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("user_expire");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                    }
                    else if (IsLoginCheck() == true && data.Rows.Count < 1)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("unsuccess");
                        DialogHost.Show(new WarningView(), "Msg");

                        IoC.Application.CurrPage = ApplicationPage.Reset;
                        IoC.Application.CurrPage = ApplicationPage.Login;
                    }
                }
            });

            btn_cancel = new RelayCommand(p =>
            {
                IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                IoC.ConfirmView.msg_text = GetLocalizedValue<string>("cancel_confirm");
                DialogHost.Show(new ConfirmView(), "Msg", ConfirmClosingEventHandler);
            });
        }
        #endregion

        #region other method
        public void GoToMain()
        {
            username = login_data["staff_username"].ToString();
            login_id = login_data["staff_id"].ToString();

            IoC.MainView.btn_control.Execute("");
            IoC.Application.list_menu = true;
            IoC.Application.CurrPage = ApplicationPage.Reset;
            IoC.Application.CurrPage = ApplicationPage.Main;

            if (login_data["type_name"].ToString() == "staff")
            {
                IoC.OptionView.admin_check = false;
                IoC.OptionView.user_group_check = false;
                IoC.OptionView.promotion_check = false;
                IoC.StaffView.add_check = false;
                IoC.StaffView.del_check = false;
                IoC.GenerateCouponView.option_coupon_check = false;
            }
        }

        public bool IsLoginCheck()
        {
            string query = $"select *,if(staff_e_date is null or staff_e_date >= current_timestamp(),'true','false') as exp_check " +
                           $"from staff " +
                           $"left join user_group on user_group.group_id = staff.group_id " +
                           $"left join type on type.type_id = user_group.type_id " +
                           $"where staff_username = '{txt_username}' and staff_password = AES_ENCRYPT('{txt_password}', 'dead_project') ";
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                data = dt;
                Sconn.conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
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
            finally
            {
                Sconn.conn.Close();
            }
        }
        public bool IsUpdate()
        {
            string query = $"update staff set " +
                           $"staff_s_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' " +
                           $"where staff_id = '{login_data["staff_id"].ToString()}' ";
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Close();
                Sconn.conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
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
            finally
            {
                Sconn.conn.Close();
            }
        }

        public void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                Application.Current.Shutdown();
            }
        }

        public void GoPassChanged(object p)
        {
            var passwordBox = p as PasswordBox;
            txt_password = passwordBox.Password;
        }

        public void IsClear()
        {
            txt_username  = null;
            txt_password = null;
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
