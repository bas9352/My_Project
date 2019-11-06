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
    public class AddEditStaffViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DateTime s_put_date { get; set; } = DateTime.Now;
        public bool group { get; set; } = false;
        public bool create_date { get; set; } = false;
        public bool start_date { get; set; } = false;
        public bool end_date { get; set; } = false;
        public bool IsCheck { get; set; } = false;
        public bool grid_add_edit_s_check { get; set; } = true;
        public DataTable data { get; set; }
        public DataRowView group_item { get; set; }

        private int conn_number;


        public string staff_id { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password;
        //public string convertpass { get; set; } = null;
        public string selectedIndex { get; set; } = null;
        public string group_id { get; set;} = null;
        public string txt_c_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_s_date { get; set; } = null;
        public string txt_e_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_name { get; set; } = null;
        public string txt_nickname { get; set; } = null;
        public string txt_lastname { get; set; } = null;
        public string txt_birthday { get; set; } = null;
        public string txt_tel { get; set; } = null;
        public string txt_email { get; set; } = null;
        public string txt_address { get; set; } = null;
        public string txt_id_card { get; set; } = null;

        

        #endregion

        #region Commands
        public ICommand btn_check_box { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        public ICommand item_group { get; set; }
        public ICommand item_group_changed { get; set; }

        #endregion

        #region Constructor
        public AddEditStaffViewModel()
        {
            pass = new RelayCommand(p => GoPassChanged(p));
            item_group = new RelayCommand(p => GoItemGroup(p));
            item_group_changed = new RelayCommand(p => GoItemGroupChanged(p));
            btn_check_box = new RelayCommand(p =>
            {
                if (IsCheck == true)
                {
                    end_date = true;
                }
                else
                {
                    end_date = false;
                }
            });

            btn_ok = new RelayCommand(p =>
            {
                grid_add_edit_s_check = false;
                if (txt_username == null || txt_password == null || txt_username == "" || txt_password == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_user_pass");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if(IsSelect() == true)
                    {
                        if (staff_id == null)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_all_username"].ToString() == txt_username)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("staff_name_unsuccess");
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
                                else
                                {
                                    if (conn_number == 0)
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                                        DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                    }
                                    else
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("add_unsuccess");
                                        DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                    }
                                }
                            }
                        }
                        else
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_all_username"].ToString() == txt_username && 
                                    row["v_all_id"].ToString() != staff_id &&
                                    row["v_all_type"].ToString() == "staff")
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("staff_name_unsuccess");
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
                                else
                                {
                                    if (conn_number == 0)
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                                        DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                    }
                                    else
                                    {
                                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("edit_unsuccess");
                                        DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                    }
                                }
                            }
                        }
                    }
                    else if(conn_number == 0)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                        DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                    }
                }                    

            });

            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                IsClear();
            });
        }

        private void IsClear()
        {
            group = false;
            create_date = false;
            start_date = false;
            end_date = false;
            IsCheck = false;
            group_item = null;
            staff_id = null;
            txt_username = null;
            txt_password = null;
            //public string convertpass { get; set; } = null;
            group_id = null;
            txt_c_date = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
            txt_s_date = null;
            txt_e_date = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
            txt_name = null;
            txt_nickname = null;
            txt_lastname = null;
            txt_birthday = null;
            txt_tel = null;
            txt_email = null;
            txt_address = null;
            txt_id_card = null;
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.StaffView.item_staff.Execute(IoC.StaffView.staff_data);
                grid_add_edit_s_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
                //IoC.OptionView.CurrPage = ApplicationPage.Reset;
                //IoC.OptionView.CurrPage = ApplicationPage.Staff;
                //IoC.StaffView.IsSelect();
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_s_check = true;
            }
        }

        public bool IsInsert()
        {
            if (IsCheck == false)
            {
                txt_e_date = null;
            }
            else
            {
                var e_date = DateTime.Parse(txt_e_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                txt_e_date = e_date;
            }
            string query = $"insert into staff set " +
                           $"staff_username = '{txt_username}', " +
                           $"staff_password = '{txt_password}', " +
                           $"group_id = '{group_id}', " +
                           $"staff_name = '{txt_name}', " +
                           $"staff_lastname = '{txt_lastname}', " +
                           $"staff_nickname = '{txt_nickname}', " +
                           $"staff_birthday = '{txt_birthday}', " +
                           $"staff_tel = '{txt_tel}', " +
                           $"staff_email = '{txt_email}', " +
                           $"staff_address = '{txt_address}', " +
                           $"staff_id_card = '{txt_id_card}', " +
                           $"staff_c_date = '{DateTime.Parse(txt_c_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")}', " +
                           $"staff_e_date = '{txt_e_date}' ";
            //MessageBox.Show($"{query}");
            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    conn_number = ex.Number;
                    MessageBox.Show(ex.ToString());
                    Sconn.conn.Close();
                    return false;
                }
                finally
                {
                    Sconn.conn.Close();
                }

            }
            else
            {
                Sconn.conn.Close();
                return false;
            }
        }

        public bool IsUpdate()
        {
            if (IsCheck == false)
            {
                txt_e_date = null;
            }
            else
            {
                var e_date = DateTime.Parse(txt_e_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                txt_e_date = e_date;
            }
            string query = $"update staff set " +
                           $"staff_username='{txt_username}' , " +
                           $"staff_password='{txt_password}' , " +
                           $"staff_name='{txt_name}' , " +
                           $"staff_lastname='{txt_lastname}' , " +
                           $"staff_nickname='{txt_nickname}' , " +
                           $"staff_birthday='{txt_birthday}' , " +
                           $"staff_tel='{txt_tel}' , " +
                           $"staff_email='{txt_email}' , " +
                           $"staff_address='{txt_address}' , " +
                           $"staff_id_card='{txt_id_card}', " +
                           $"staff_e_date='{txt_e_date}', " +
                           $"group_id ='{group_id}' " +
                           $"where staff_id = '{staff_id}'";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    conn_number = ex.Number;
                    Sconn.conn.Close();
                    return false;
                }
                finally
                {
                    Sconn.conn.Close();
                }

            }
            else
            {
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
            string query = $"select * from user_group " +
                           $"left join type on type.type_id = user_group.type_id where type_name like '%staff%' order by group_id";

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
            //if (type_item["type_name"].ToString() == "coupon")
            //{
            //    bonus_on_off = false;
            //    on_off_bonus = false;
            //}
            //else
            //{
            //    bonus_on_off = true;
            //}
        }

        public bool IsSelect()
        {
            string query = $"select * from v_all_username";

            if (OpenConnection() == true)
            {
                try
                {
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
                    conn_number = ex.Number;
                    //MessageBox.Show(ex.ToString());
                    Sconn.conn.Close();
                    return false;
                }
                finally
                {
                    Sconn.conn.Close();
                }
            }
            else
            {
                Sconn.conn.Close();
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
