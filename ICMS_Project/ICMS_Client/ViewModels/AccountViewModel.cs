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
    public class AccountViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
            
        public DataRowView group_item { get; set; }
        public string group_id { get; set; }
        public bool group { get; set; } = false;
        public bool create_date { get; set; }
        public bool start_date { get; set; }
        public bool end_date { get; set; }
        
            
        public string txt_username { get; set; }
        public string txt_password { get; set; }
        public string txt_c_date { get; set; }
        public string txt_s_date { get; set; }
        public string txt_e_date { get; set; }
        public string txt_name { get; set; }
        public string txt_nickname { get; set; }
        public string txt_lastname { get; set; }
        public string txt_birthday { get; set; }
        public string txt_tel { get; set; }
        public string txt_email { get; set; }
        public string txt_address { get; set; }
        public string txt_id_card { get; set; }
        public string txt_debt { get; set; }
        public string txt_top_up_amount { get; set; }
        public string txt_use_amount { get; set; }
        public string txt_remaining_amount { get; set; }
        public string txt_total_free_amount { get; set; }
        public string txt_remaining_free_amount { get; set; }
        public string txt_remaining_point { get; set; }




        public Window MainApp { get; set; }
        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostOpen { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Account;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        #endregion

        #region Commands
        public ICommand item_group { get; set; }
        public ICommand item_group_change { get; set; }
        public ICommand pass { get; set; }



        public ICommand ToggleBaseCommand { get; set; }

        public ICommand btn_switchMode { get; set; }

        public ICommand btn_ViewHelp { get; set; }

        public ICommand btn_About { get; set; }

        public ICommand btn_setting { get; set; }
        public ICommand btn_sendFeedBack { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public AccountViewModel()
        {
            pass = new RelayCommand(p => GoPassChanged(p));
            item_group = new RelayCommand(p => GoItemGroup(p));
            item_group_change = new RelayCommand(p => GoItemGroupChanged(p));
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

        public void GoItemGroup(object p)
        {
            var item = p as ComboBox;
            string query = $"select * from user_group inner join type on type.type_id = user_group.type_id where type_name like '%member%' order by group_id";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "user_group");
                    item.ItemsSource = ds.Tables[0].DefaultView;
                    item.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
                    item.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
                    item.SelectedIndex = 0;
                    group_item = item.SelectedItem as DataRowView;
                    if (group_id == null)
                    {
                        group_id = group_item["group_id"].ToString();
                    }
                    else
                    {
                        //MessageBox.Show($"{selectedItem}");
                        item.SelectedValue = group_id;
                    }
                    //selectedIndex = group_item.SelectedValue.ToString();
                    Sconn.conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                    Sconn.conn.Close();
                }
                finally
                {
                    Sconn.conn.Close();
                }
            }
            else
            {
                Sconn.conn.Close();
            }
        }

        public void GoItemGroupChanged(object p)
        {
            var item = p as ComboBox;
            group_item = item.SelectedItem as DataRowView;
            group_id = group_item["group_id"].ToString();
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
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("ไม่มีการเชื่อมต่อ");
                        break;
                    case 1045:
                        MessageBox.Show("เชื่อมต่อสำเร็จ");
                        break;
                }
                return false;
            }
        }
        #endregion
    }
}
