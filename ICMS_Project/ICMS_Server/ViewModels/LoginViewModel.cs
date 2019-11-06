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
        Database Sconn { get; set; }
        public DataTable data { get; set; }
        public DataRow login_data { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password;

        public string username { get; set; }
        public string login_id { get; set; }
        string test;


        #endregion

        #region Comamnd
        public ICommand btn_login { get; set; }

        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        public ICommand username_lost_focus_changed { get; set; }

        #endregion

        public LoginViewModel()
        {
            username_lost_focus_changed = new RelayCommand(p => GoUsernameLostFocusChanged(p));
            pass = new RelayCommand(p=> GoPassChanged(p));
            btn_login = new RelayCommand(p =>
            {
                StartLogin();
            });

            btn_cancel = new RelayCommand(p =>
            {
                StartCancel();
            });
        }

        #region Start Login
        public Process process;

        public CancellationTokenSource taskCancelSource;
        public CancellationToken taskCancelToken;

        private void StartLogin()
        {
            MessageBox.Show("1");
            if (txt_username == null || txt_password == null || 
                txt_username == "" || txt_password == "")
            {
                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_user_pass");
                DialogHost.Show(new WarningView(), "Msg");
            }
            else
            {
                MessageBox.Show("2");
                if (IsLoginCheck() == true && data.Rows.Count > 0)
                {
                    login_data = data.Rows[0];
                    if (login_data["exp_check"].ToString() == "true")
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
        }
        #endregion

        #region Start Cencel
        private void StartCancel()
        {
            IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
            IoC.ConfirmView.msg_text = GetLocalizedValue<string>("cancel_confirm");
            DialogHost.Show(new ConfirmView(), "Msg", ConfirmClosingEventHandler);
            
        }

        public bool IsLoginCheck()
        {
            MessageBox.Show("3");
            string query = $"select *,if(staff_e_date is null or staff_e_date >= CURRENT_TIMESTAMP(),'true','false') as exp_check " +
                           $"from staff " +
                           $"left join user_group on user_group.group_id = staff.group_id " +
                           $"left join type on type.type_id = user_group.type_id " +
                           $"where staff_username = '{txt_username}' and staff_password = '{txt_password}' ";
            try
            {
                MessageBox.Show("4");
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data = dt;
                    Sconn.conn.Close();
                    //MessageBox.Show($"{dt}");
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

            string EncryptionKey = "test123456key";
            byte[] clearBytes = Encoding.Unicode.GetBytes(passwordBox.Password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    txt_password = Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public void GoUsernameLostFocusChanged(object p)
        {
            //MessageBox.Show($"{"test"}");
        }

        public bool OpenConnection()
        {
            MessageBox.Show("5");
            try
            {
                MessageBox.Show("6");
                Sconn.conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                MessageBox.Show($"{ex.Number}");
                //if (ex.Number == 0)
                //{
                //    //IoC.Application.DialogHostMsg = false;
                //    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                //    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                //    DialogHost.Show(new WarningView(), "Msg");
                //}
                //else
                //{
                //    //IoC.Application.DialogHostMsg = false;
                //    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                //    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                //    DialogHost.Show(new WarningView(), "Msg");
                //}
                return false;
            }
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
