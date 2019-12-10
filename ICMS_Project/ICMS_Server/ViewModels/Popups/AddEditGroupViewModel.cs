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
    public class AddEditGroupViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public bool create_date { get; set; } = false;
        public bool bonus { get; set; } = false;
        public bool on_off_bonus { get; set; } = false;
        public bool type { get; set; } = true;
        public bool IsCheck { get; set; } = false;
        public bool bonus_on_off { get; set; } = true;
        public bool grid_add_edit_g_check { get; set; } = true;
        public DataTable data { get; set; }
        public DataTable data_username { get; set; }
        public DataRowView type_item { get; set; }
        public ComboBox type_data { get; set; }
        public DataRowView bonus_item { get; set; }
        public DataGrid bonus_data { get; set; }

        public string group_id { get; set; }
        public int bonus_index { get; set; }
        public string txt_group_name { get; set; } = null;
        public string txt_group_rate { get; set; } = null;
        public int type_index { get; set; }
        public string type_id { get; set; } = null;
        public string txt_c_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));

        public string title { get; set; }


        #endregion

        #region Commands
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }
        public ICommand btn_check_box { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }

        public ICommand item_type { get; set; }
        public ICommand item_type_change { get; set; }
        public ICommand item_bonus { get; set; }
        public ICommand item_bonus_change { get; set; }
        public ICommand on_off_bonus_status { get; set;}

        #endregion

        #region Constructor
        public AddEditGroupViewModel()
        {
            item_type_change = new RelayCommand(p => GoItemTypeChanged(p));
            item_type = new RelayCommand(p => GoItemType(p));

            item_bonus_change = new RelayCommand(p => GoItemBonusChanged(p));
            item_bonus = new RelayCommand(p => GoItemBonus(p));

            on_off_bonus_status = new RelayCommand(p =>
            {
                if (on_off_bonus == true)
                {
                    bonus = true;
                }
                else
                {
                    bonus = false;
                }
            });

            btn_ok = new RelayCommand(p =>
            {
                grid_add_edit_g_check = false;
                if (txt_group_name == null || txt_group_name == "" || txt_group_rate == null || txt_group_rate == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        if (group_id == null)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["group_name"].ToString() == txt_group_name)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("group_name_unsuccess");
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
                                if (row["group_name"].ToString() == txt_group_name && row["group_id"].ToString() != group_id)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("group_name_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                            }

                            if (num == 0)
                            {
                                if (IsSelectGroupCheck() == true)
                                {
                                    int number = 0;
                                    for (int i =0; i<data_username.Rows.Count;i++)
                                    {
                                        var row = data_username.Rows[i];

                                        if (row["v_all_group_id"].ToString() == group_id && 
                                            row["v_all_group_rate"].ToString() != txt_group_rate)
                                        {
                                            number = 1;
                                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                            IoC.WarningView.msg_text = GetLocalizedValue<string>("cant_edit_rate");
                                            DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                        }
                                    }

                                    if (number == 0)
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
                    }
                }

            });

            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                IsClear();
            });

            btn_add = new RelayCommand(p =>
            {
                grid_add_edit_g_check = false;
                IoC.AddEditBonusView.bonus_id = null;
                DialogHost.Show(new AddEditBonusView(), "InMain");
            });

            btn_edit = new RelayCommand(p =>
            {
                grid_add_edit_g_check = false;
                if (bonus_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.AddEditBonusView.bonus_id = bonus_item["bonus_id"].ToString();
                    IoC.AddEditBonusView.txt_bonus_top_up = bonus_item["bonus_top_up"].ToString();
                    IoC.AddEditBonusView.txt_bonus_point = bonus_item["bonus_point"].ToString();

                    if (bonus_item["bonus_status"].ToString() == "true")
                    {
                        IoC.AddEditBonusView.on_off_bonus = true;
                    }
                    else
                    {
                        IoC.AddEditBonusView.on_off_bonus = false;
                    }
                    //CurrPage = ApplicationPage.Admin;
                    DialogHost.Show(new AddEditBonusView(), "InMain");
                }

            });

            btn_del = new RelayCommand(p =>
            {
                grid_add_edit_g_check = false;
                if (bonus_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                    IoC.ConfirmView.msg_text = GetLocalizedValue<string>("del_confirm");
                    DialogHost.Show(new ConfirmView(), "Msg", IsDelete);
                }
            });
        }
        #endregion

        #region Other method
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IsClear();
                IoC.UserGroupView.item_group.Execute(IoC.UserGroupView.group_data);
                grid_add_edit_g_check = true;
                IoC.Application.DialogHostMain = false;
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_g_check = true;
            }
        }

        private void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            int num = 0;
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                    if (IsDelete() == true)
                    {
                        num = 1;

                    }
                    else
                    {
                        num = 0;
                    }
                }).ContinueWith((previousTask) => {
                    if (num == 1)
                    {
                        grid_add_edit_g_check = true;
                        bonus_item = null;
                        bonus_index = 0;
                        item_bonus.Execute(bonus_data);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
        }


        private void IsClear()
        {
            bonus = false;
            on_off_bonus = false;
            type = true;
            IsCheck = false;
            bonus_on_off = true;
            data = null;
            type_item = null;
            bonus_item = null;
            group_id = null;
            bonus_index = 0;
            txt_group_name = null;
            txt_group_rate = null;
            type_index = 0;
            type_id = null;
        }

        public bool IsInsert()
        {
            string query = $"insert into user_group set " +
                           $"group_name = '{txt_group_name}', " +
                           $"type_id = '{type_id}', " +
                           $"group_rate = '{txt_group_rate}', " +
                           $"group_bonus_status = '{on_off_bonus}', " +
                           $"group_c_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";

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

        public bool IsUpdate()
        {
            string query = $"update user_group set " +
                           $"group_name='{txt_group_name}' , " +
                           $"type_id='{type_id}' , " +
                           $"group_rate='{txt_group_rate}' , " +
                           $"group_bonus_status='{on_off_bonus}' " +
                           $"where group_id = '{group_id}'";

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

        public bool IsDelete()
        {
            string query = $"select * from bonus";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder cmdb = new MySqlCommandBuilder(adp);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                Console.WriteLine(cmdb.GetDeleteCommand().CommandText);
                DataRow dr = dt.Rows[bonus_index];
                dr.Delete();
                adp.Update(dt);
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

        public void GoItemTypeChanged(object p)
        {
            var item = p as ComboBox;
            type_item = item.SelectedItem as DataRowView;
            type_id = type_item["type_id"].ToString();

            if (type_item["type_name"].ToString() == "coupon")
            {
                bonus_on_off = false;
                on_off_bonus = false;
                bonus = false;
            }
            else
            {
                bonus_on_off = true;
            }
        }

        public void GoItemType(object p)
        {
            type_data = p as ComboBox;
            string query = $"select * from type " +
                           $"where not type_name = 'admin' and not type_name = 'staff' " +
                           $"order by type_id";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "type");
                Sconn.conn.Close();
                type_data.ItemsSource = ds.Tables[0].DefaultView;
                type_data.SelectedValuePath = ds.Tables[0].Columns["type_id"].ToString();
                type_data.DisplayMemberPath = ds.Tables[0].Columns["type_name"].ToString();
                type_data.SelectedIndex = 0;
                type_item = type_data.SelectedItem as DataRowView;
                if (type_id == null)
                {
                    type_id = type_item["type_id"].ToString();
                }
                else
                {
                    type_data.SelectedValue = type_id;
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
                    item_type.Execute(type_data);
                    item_bonus.Execute(bonus_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoItemBonus(object p)
        {
            //IsClear();
            bonus_data = p as DataGrid;
            string query = $"select * from bonus order by bonus_top_up";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                bonus_data.ItemsSource = dt.DefaultView;
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

        public void GoItemBonusChanged(object p)
        {
            var item = p as DataGrid;
            bonus_item = item.SelectedItem as DataRowView;
            bonus_index = item.SelectedIndex;
        }

        public bool IsSelect()
        {
            string query = $"select * from user_group order by group_id;";

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

        public bool IsSelectGroupCheck()
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
                data_username = dt;
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
