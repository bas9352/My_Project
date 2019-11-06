using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class AdminViewModel : BaseView
    {
        #region Properties
        public DataTable data_pass { get; set; }
        public DataTable data_username { get; set; }
        public DataRow row { get; set; }
        public string txt_name { get; set; } = null;
        public string txt_lastname { get; set; } = null;
        public string txt_username { get; set; } = null;
        public string txt_old_password { get; set; } = null;
        public string txt_new_password { get; set; } = null;
        public string txt_confirm_password { get; set; } = null;
        public Database Sconn = new Database();

        private int conn_number;
        private string pass;
        private string id;

        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand old_pass { get; set; }
        public ICommand new_pass { get; set; }
        public ICommand confirm_pass { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public AdminViewModel()
        {
            old_pass = new RelayCommand(p => GoOldPassChanged(p));
            new_pass = new RelayCommand(p => GoNewPassChanged(p));
            confirm_pass = new RelayCommand(p => GoConfirmPassChanged(p));

            btn_ok = new RelayCommand(p =>
            {
                //MessageBox.Show($"{txt_username}");
                //MessageBox.Show($"{txt_old_password},{txt_new_password},{txt_confirm_password}");
                if (txt_username == null ||
                    txt_old_password == null || 
                    txt_new_password == null || 
                    txt_confirm_password == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    if (txt_new_password != txt_confirm_password)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_match_pass");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        if (IsSelectCheckUserName() == true)
                        {
                            int num = 0;
                            for (int i = 0; i < data_username.Rows.Count; i++)
                            {
                                var item = data_username.Rows[i];

                                if (txt_username == item["v_all_username"].ToString() &&
                                    item["v_all_id"].ToString() != IoC.LoginView.login_id &&
                                    item["v_all_type_name"].ToString() != "admin")
                                {
                                    num = 1;
                                }
                            }

                            if (num == 0)
                            {
                                if (IsSelectCheckPass() == true)
                                {
                                    for (int i = 0; i < data_pass.Rows.Count; i++)
                                    {
                                        row = data_pass.Rows[i];
                                        //MessageBox.Show($"{row["staff_id"].ToString()}");
                                        string EncryptionKey = "test123456key";

                                        byte[] cipherBytes = Convert.FromBase64String(row["staff_password"].ToString());
                                        Aes encryptor = Aes.Create();
                                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                                        encryptor.Key = pdb.GetBytes(32);
                                        encryptor.IV = pdb.GetBytes(16);
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                                            {
                                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                                cs.Close();
                                            }
                                            pass = Encoding.Unicode.GetString(ms.ToArray());
                                        }
                                    }

                                    if (txt_old_password != pass)
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_match");
                                        DialogHost.Show(new WarningView(), "Msg");
                                    }
                                    else
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
                            else
                            {
                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                IoC.WarningView.msg_text = GetLocalizedValue<string>("staff_name_unsuccess");
                                DialogHost.Show(new WarningView(), "Msg");
                            }
                        }
                    }
                }
            });

            btn_cancel = new RelayCommand(p => 
            {
                IsClear();
                //IoC.OptionView.btn_officer.Execute("");
                IoC.Application.CurrPage = ApplicationPage.Reset;
                IoC.Application.CurrPage = ApplicationPage.Main;
            });
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IsClear();
                //IoC.OptionView.btn_officer.Execute("");
                IoC.OptionView.CurrPage = ApplicationPage.Reset;
                IoC.OptionView.CurrPage = ApplicationPage.Admin;
                //IoC.StaffView.IsSelect();
            }
        }

        public void IsClear()
        {
            txt_name = null;
            txt_lastname = null;
            txt_username = null;
            txt_old_password = null;
            txt_new_password = null;
            txt_confirm_password = null;
        }

        public void GoOldPassChanged(object p)
        {
            var passwordBox = p as PasswordBox;
            txt_old_password = passwordBox.Password;
        }
        public void GoNewPassChanged(object p)
        {
            var passwordBox = p as PasswordBox;
            txt_new_password = passwordBox.Password;
        }
        public void GoConfirmPassChanged(object p)
        {
            var passwordBox = p as PasswordBox;
            txt_confirm_password = passwordBox.Password;
        }

        private bool IsUpdate()
        {
            string pass;
            string EncryptionKey = "test123456key";
            byte[] clearBytes = Encoding.Unicode.GetBytes(txt_confirm_password);
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
                    pass = Convert.ToBase64String(ms.ToArray());
                }
            }
            string query = $"update staff set " +
                           $"staff_username='{txt_username}' , " +
                           $"staff_password='{pass}' " +
                           $"where staff_id = {IoC.LoginView.login_id} ";

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
                Sconn.conn.Close();
                return false;
            }
            
        }

        public bool IsSelectCheckPass()
        {
            string query = $"select staff_password " +
                           $"from staff " +
                           $"where staff_id = '{IoC.LoginView.login_id}' ";

            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data_pass = dt;
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
                Sconn.conn.Close();
                return false;
            }
            
        }

        public bool IsSelectCheckUserName()
        {
            string query = $"select * " +
                           $"from v_all_username ";

            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data_username = dt;
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
                Sconn.conn.Close();
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
