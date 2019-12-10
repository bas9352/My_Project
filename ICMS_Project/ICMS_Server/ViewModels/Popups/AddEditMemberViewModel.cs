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
    public class AddEditMemberViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DateTime s_put_date { get; set; } = DateTime.Now;
        public string title { get;set; }
        public bool is_debt { get; set; } = false;
        public bool is_remaining_amount { get; set; } = false;
        public bool is_remaining_free_amount { get; set; } = false;

        public bool grid_add_edit_m_check { get; set; } = true;

        public bool group { get; set; } = true;
        public bool create_date { get; set; } = false;
        public bool start_date { get; set; } = false;
        public bool end_date { get; set; } = false;
        public bool IsCheck { get; set; } = false;

        public bool member_add { get; set; } = false;
        public bool member_edit { get; set; } = false;

        public DataTable data { get; set; }
        public DataRowView group_item { get; set; }
        public ComboBox group_data { get; set; }


        public string member_id { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password;
        public string group_id { get; set; } = null;
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

        public string txt_debt { get; set; } = "0";
        public string txt_total_real_amount { get; set; } = "0";
        public string txt_use_real_free_amount { get; set; } = "0";
        public string txt_remaining_real_amount { get; set; } = "0";
        public string txt_total_free_amount { get; set; } = "0";
        public string txt_remaining_free_amount { get; set; } = "0";
        public string txt_remaining_point { get; set; } = "0";

        public string use_real_amount { get; set; }
        public string use_free_amount { get; set; }
        #endregion
        #region Commands
        public ICommand btn_check_box { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        public ICommand item_group { get; set; }
        public ICommand item_group_change { get; set; }
        #endregion

        #region Constructor
        public AddEditMemberViewModel()
        {
            pass = new RelayCommand(p => GoPassChanged(p));
            item_group = new RelayCommand(p => GoItemGroup(p));
            item_group_change = new RelayCommand(p => GoItemGroupChanged(p));

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
                grid_add_edit_m_check = false;

                if (txt_username == null || txt_password == null || txt_username == "" || txt_password == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        if (member_id == null)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_all_username"].ToString() == txt_username)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("member_name_unsuccess");
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
                            }
                        }
                        else
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_all_username"].ToString() == txt_username &&
                                    row["v_member_id"].ToString() != member_id)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("member_name_unsuccess");
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

        #region Ohter method
        public void IsClear()
        {
            group = true;
            create_date = false;
            start_date = false;
            end_date = false;
            IsCheck = false;
            data = null;
            group_item = null;

            is_debt = false;
            is_remaining_amount = false;
            is_remaining_free_amount = false;


            member_id = null;
            txt_username = null;
            txt_password = null;
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

            txt_debt = "0";
            txt_total_real_amount = "0";
            txt_use_real_free_amount = "0";
            txt_remaining_real_amount = "0";
            txt_total_free_amount= "0";
            txt_remaining_free_amount = "0";
            txt_remaining_point = "0";

            use_real_amount = null;
            use_free_amount = null;
        }
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.MemberView.item_member.Execute(IoC.MemberView.member_data);
                grid_add_edit_m_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_m_check = true;
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
            group_data = p as ComboBox;
            string query = $"select * from user_group " +
                           $"left join type on type.type_id = user_group.type_id " +
                           $"where type_name like '%member%' " +
                           $"order by group_id";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "user_group");
                Sconn.conn.Close();
                group_data.ItemsSource = ds.Tables[0].DefaultView;
                group_data.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
                group_data.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
                group_data.SelectedIndex = 0;
                group_item = group_data.SelectedItem as DataRowView;
                if (group_id == null)
                {
                    group_id = group_item["group_id"].ToString();
                }
                else
                {
                    group_data.SelectedValue = group_id;
                }
                grid_add_edit_m_check = true;
            }
            catch (MySqlException ex)
            {
                Task.Factory.StartNew(async () =>
                {
                    grid_add_edit_m_check = false;
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {
                    if (ex.Number == 0)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                        DialogHost.Show(new WarningView(), "Msg", conn_fail);
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                        DialogHost.Show(new WarningView(), "Msg", conn_fail);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        private void conn_fail(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.DialogHostMsg = false;
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {
                    item_group.Execute(group_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoItemGroupChanged(object p)
        {
            var item = p as ComboBox;
            group_item = item.SelectedItem as DataRowView;
            group_id = group_item["group_id"].ToString();
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
            string query = $"insert into member set " +
                           $"member_username = '{txt_username}', " +
                           $"member_password = '{txt_password}', " +
                           $"group_id = '{group_id}', " +
                           $"member_name = '{txt_name}', " +
                           $"member_lastname = '{txt_lastname}', " +
                           $"member_nickname = '{txt_nickname}', " +
                           $"member_birthday = '{txt_birthday}', " +
                           $"member_tel = '{txt_tel}', " +
                           $"member_email = '{txt_email}', " +
                           $"member_address = '{txt_address}', " +
                           $"member_id_card = '{txt_id_card}', " +
                           $"member_c_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}', " +
                           $"member_e_date = '{txt_e_date}', " +
                           $"member_create_by = '{IoC.LoginView.login_id}' ";
            
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
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
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
            if (IsCheck == false)
            {
                txt_e_date = null;
            }
            else
            {
                var e_date = DateTime.Parse(txt_e_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                txt_e_date = e_date;
            }
            string query = $"update member set " +
                           $"member_username='{txt_username}' , " +
                           $"member_password='{txt_password}' , " +
                           $"member_name='{txt_name}' , " +
                           $"member_lastname='{txt_lastname}' , " +
                           $"member_nickname='{txt_nickname}' , " +
                           $"member_birthday='{txt_birthday}' , " +
                           $"member_tel='{txt_tel}' , " +
                           $"member_email='{txt_email}' , " +
                           $"member_address='{txt_address}' , " +
                           $"member_id_card='{txt_id_card}', " +
                           $"member_e_date='{txt_e_date}', " +
                           $"group_id ='{group_id}' " +
                           $"where member_id = '{member_id}'";

            
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
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public bool IsSelect()
        {
            string query = $"select * from v_all_user";
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
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
