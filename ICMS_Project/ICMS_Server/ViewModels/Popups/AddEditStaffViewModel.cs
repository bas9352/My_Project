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
        public ComboBox combobox_data { get; set; }
        public PasswordBox staff_password { get; set; }
        public string title { get; set; }


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
                if (string.IsNullOrEmpty(txt_username) == true || string.IsNullOrEmpty(txt_password) == true)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
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
                            }
                        }
                        else
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_all_username"].ToString() == txt_username && 
                                    row["v_staff_id"].ToString() != staff_id)
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

        #region Other method
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
                           $"staff_password = AES_ENCRYPT('{txt_password}', 'dead_project'), " +
                           $"group_id = '{group_id}', " +
                           $"staff_name = '{txt_name}', " +
                           $"staff_lastname = '{txt_lastname}', " +
                           $"staff_nickname = '{txt_nickname}', " +
                           $"staff_birthday = '{txt_birthday}', " +
                           $"staff_tel = '{txt_tel}', " +
                           $"staff_email = '{txt_email}', " +
                           $"staff_address = '{txt_address}', " +
                           $"staff_id_card = '{txt_id_card}', " +
                           $"staff_c_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}', " +
                           $"staff_e_date = '{txt_e_date}' ";

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
            string query = $"update staff set " +
                           $"staff_username='{txt_username}' , " +
                           $"staff_password=if('{staff_password}' = '{""}', AES_ENCRYPT((select AES_DECRYPT(staff_password,'dead_project') from staff where staff_id = '{staff_id}'), 'dead_project'), AES_ENCRYPT('{txt_password}', 'dead_project')), " +
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

        public void GoPassChanged(object p)
        {
            staff_password = p as PasswordBox;
            txt_password = staff_password.Password;
        }

        public void GoItemGroup(object p)
        {
            combobox_data = p as ComboBox;
            string query = $"select * from user_group " +
                           $"left join type on type.type_id = user_group.type_id " +
                           $"where type_name like '%staff%' " +
                           $"order by group_id";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "user_group");
                Sconn.conn.Close();
                combobox_data.ItemsSource = ds.Tables[0].DefaultView;
                combobox_data.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
                combobox_data.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
                combobox_data.SelectedIndex = 0;
                group_item = combobox_data.SelectedItem as DataRowView;
                if (group_id == null)
                {
                    group_id = group_item["group_id"].ToString();
                }
                else
                {
                    combobox_data.SelectedValue = group_id;
                }
            }
            catch (MySqlException ex)
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
                    item_group.Execute(combobox_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoItemGroupChanged(object p)
        {
            var item = p as ComboBox;
            group_item = item.SelectedItem as DataRowView;
            group_id = group_item["group_id"].ToString();
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
                Sconn.conn.Close();
                data = dt;
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
