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
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Client
{
    public class LoginViewModel : BaseView
    {
        #region Properties 
        Database Sconn = new Database();
        public DataRow user_data { get; set; }
        DataTable data { get; set; }
        //public Window MainApp { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password { get; set; } = null;
        public string username { get; set; } = null;

        public string all_user_id { get; set; } = null;

        private string conn_number { get; set; }

        #endregion

        #region Comamnd
        public ICommand btn_login { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }

        #endregion

        public LoginViewModel()
        {
            pass = new RelayCommand(p=>GoPassChanged(p));

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
            if (txt_username == null || txt_password == null || 
                txt_username == "" || txt_password == "")
            {
                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_user_pass");
                DialogHost.Show(new WarningView(), "Msg");
            }
            else
            {
                if (IsSelectAllUser() == true && data.Rows.Count > 0)
                {
                    user_data = data.Rows[0];
                    if (IsOnline() == true)
                    {
                        if (user_data["v_all_type_name"].ToString() == "admin")
                        {

                        }
                    }
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("login_false");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                //ถ้ามีข้อมูล
                //if (IsSelectStaff() == true && data.Rows.Count > 0)
                //{
                //    staff = data.Rows[0];
                //    if (IsOnline() == true)
                //    {
                //        all_user_id = null;
                //        IoC.MainView.account_check = false;
                //        IoC.MainView.promotion_check = false;
                //        IoC.MainView.log_history_check = false;
                //        //username = staff["staff_username"].ToString();

                //        //IoC.Application.list_menu = true;
                //        Application.Current.MainWindow = new MainWindow()
                //        {
                //            DataContext = IoC.MainView,
                //            Width = Math.Round(SystemParameters.PrimaryScreenWidth * 20 / 100),
                //            Height = Math.Round(SystemParameters.PrimaryScreenHeight * 70 / 100),
                //            WindowStartupLocation = WindowStartupLocation.Manual,
                //            Left = Math.Round(SystemParameters.PrimaryScreenWidth * 80 / 100),
                //            Top = Math.Round(SystemParameters.PrimaryScreenWidth * 0 / 100),
                //            ResizeMode = ResizeMode.NoResize,
                //            ShowInTaskbar = false
                //            //Left = 1000,
                //            //Top = 0
                //        };
                //        //IoC.Application.CurrPage = ApplicationPage.Main;
                //        IoC.Application.MainApp.Hide();
                //        IoC.Application.MainApp.Close();
                //        IoC.Application.MainApp = Application.Current.MainWindow;
                //        Application.Current.MainWindow.ShowDialog();
                //    }
                //    else
                //    {
                //        if (conn_number == "0")
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //        else
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //    }

                //}
                //else if(IsSelectMember() == true && data.Rows.Count > 0)
                //{
                //    //MessageBox.Show($"{"2"}");
                //    member = data.Rows[0];
                //    if (IsOnline() == true)
                //    {
                //        all_user_id = member["member_id"].ToString();

                //        //IoC.MainView.online_status();
                //        //IoC.MainView.online_timer.Start();
                //        //username = staff["staff_username"].ToString();

                //        //IoC.Application.list_menu = true;
                //        Application.Current.MainWindow = new MainWindow()
                //        {
                //            DataContext = IoC.MainView,
                //            Width = Math.Round(SystemParameters.PrimaryScreenWidth * 20 / 100),
                //            Height = Math.Round(SystemParameters.PrimaryScreenHeight * 70 / 100),
                //            WindowStartupLocation = WindowStartupLocation.Manual,
                //            Left = Math.Round(SystemParameters.PrimaryScreenWidth * 80 / 100),
                //            Top = Math.Round(SystemParameters.PrimaryScreenWidth * 0 / 100),
                //            ResizeMode = ResizeMode.NoResize,
                //            ShowInTaskbar = false
                //            //Left = 1000,
                //            //Top = 0
                //        };
                //        //IoC.Application.CurrPage = ApplicationPage.Main;
                //        IoC.Application.MainApp.Hide();
                //        IoC.Application.MainApp.Close();
                //        IoC.Application.MainApp = Application.Current.MainWindow;
                //        Application.Current.MainWindow.ShowDialog();
                //    }
                //    else
                //    {
                //        if (conn_number == "0")
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //        else
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //    }
                //}
                //else if (IsSelectCoupon() == true && data.Rows.Count > 0)
                //{
                //    coupon = data.Rows[0];
                //    if (IsOnline() == true)
                //    {
                //        all_user_id = coupon["coupon_id"].ToString();
                //        //IoC.MainView.online_status();
                //        IoC.MainView.account_check = false;
                //        IoC.MainView.promotion_check = false;
                //        //IoC.MainView.online_timer.Start();
                //        //username = staff["staff_username"].ToString();

                //        //IoC.Application.list_menu = true;
                //        Application.Current.MainWindow = new MainWindow()
                //        {
                //            DataContext = IoC.MainView,
                //            Width = Math.Round(SystemParameters.PrimaryScreenWidth * 20 / 100),
                //            Height = Math.Round(SystemParameters.PrimaryScreenHeight * 70 / 100),
                //            WindowStartupLocation = WindowStartupLocation.Manual,
                //            Left = Math.Round(SystemParameters.PrimaryScreenWidth * 80 / 100),
                //            Top = Math.Round(SystemParameters.PrimaryScreenWidth * 0 / 100),
                //            ResizeMode = ResizeMode.NoResize,
                //            ShowInTaskbar = false
                //            //Left = 1000,
                //            //Top = 0
                //        };
                //        //IoC.Application.CurrPage = ApplicationPage.Main;
                //        IoC.Application.MainApp.Hide();
                //        IoC.Application.MainApp.Close();
                //        IoC.Application.MainApp = Application.Current.MainWindow;
                //        Application.Current.MainWindow.ShowDialog();
                //    }
                //    else
                //    {
                //        if (conn_number == "0")
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //        else
                //        {
                //            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //            DialogHost.Show(new WarningView(), "RootDialogMain");
                //        }
                //    }
                //}
                ////ถ้าไม่มีข้อมูล
                //else if(IsSelectStaff() == true ||
                //        IsSelectMember() == true ||
                //        IsSelectCoupon() == true &&
                //        data.Rows.Count < 1)
                //{
                //    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //    IoC.WarningView.msg_text = GetLocalizedValue<string>("login_false");
                //    DialogHost.Show(new WarningView(), "Msg");
                //}
                //else
                //{
                //    if (conn_number == "0")
                //    {
                //        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //        DialogHost.Show(new WarningView(), "RootDialogMain");
                //    }
                //    else
                //    {
                //        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                //        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                //        DialogHost.Show(new WarningView(), "RootDialogMain");
                //    }
                //}
            }
        }

        public bool IsSelectAllUser()
        {
            string query = $"select * " +
                           $"from v_all_user " +
                           $"where v_all_username = '{txt_username}' " +
                           $"and v_all_password = '{txt_password}' " +
                           $"and (v_all_e_date is null or v_all_e_date >= CURRENT_TIMESTAMP()) ";

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

        string member_id, coupon_id;
        private bool IsOnline()
        {
            string query = $"insert into online set " +
                           $"online_pc_id = '{IoC.Application.row["pc_id"].ToString()}', " +
                           $"online_member_id = if('{user_data["v_member_id"].ToString()}' = '',null,'{user_data["v_member_id"].ToString()}'), " +
                           $"online_coupon_id = if('{user_data["v_coupon_id"].ToString()}' = '',null,'{user_data["v_coupon_id"].ToString()}'), " +
                           $"online_status = 'online', " +
                           $"online_ordinal = (select max(a.online_ordinal)+1 " +
                                              $"from online a " +
                                              $"where a.online_pc_id = '{IoC.Application.row["pc_id"].ToString()}'), " +
                           $"online_s_datetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";

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

        public void UnsuccessClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.CurrPage = ApplicationPage.Reset;
                IoC.Application.CurrPage = ApplicationPage.Login;
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

        public void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                Application.Current.Shutdown();
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

        public void IsClear()
        {
            txt_username = null;
            txt_password = null;
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
