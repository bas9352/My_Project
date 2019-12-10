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
    public class UserGroupViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public bool edit { get; set; } = true;
        public bool del { get; set; } = true;
        public DataRowView group_item { get; set; } = null;
        public DataGrid group_data { get; set; } = null;
        public string group_id { get; set; } = null;
        public int group_index { get; set; }
        

        private int conn_number;

        #endregion

        #region Commands
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand item_group { get; set; }
        public ICommand item_group_changed { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public UserGroupViewModel()
        {
            item_group = new RelayCommand(p=>GoUserGroupItem(p));
            item_group_changed = new RelayCommand(p => GoUserGroupItemChanged(p));
            //IsSelect();
            btn_add = new RelayCommand(p =>
            {
                IoC.AddEditGroupView.title = GetLocalizedValue<string>("add_group");
                IoC.AddEditGroupView.group_id = null;
                DialogHost.Show(new AddEditGroupView(), "Main");
            });

            btn_edit = new RelayCommand(p =>
            {
                IoC.AddEditGroupView.title = GetLocalizedValue<string>("edit_group");
                if (group_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.AddEditGroupView.group_id = group_item["group_id"].ToString();
                    IoC.AddEditGroupView.type_id = group_item["type_id"].ToString();
                    IoC.AddEditGroupView.txt_group_name = group_item["group_name"].ToString();
                    IoC.AddEditGroupView.txt_group_rate = group_item["group_rate"].ToString();
                    IoC.AddEditGroupView.txt_c_date = group_item["group__c_date"].ToString();

                    if (group_item["type_name"].ToString() == "coupon")
                    {
                        IoC.AddEditGroupView.bonus_on_off = false;
                    }

                    if (group_item["group_bonus_status"].ToString() == "true")
                    {
                        IoC.AddEditGroupView.on_off_bonus = true;
                        IoC.AddEditGroupView.bonus = true;
                    }
                    else
                    {
                        IoC.AddEditGroupView.on_off_bonus = false;
                        IoC.AddEditGroupView.bonus = false;
                    }

                    IoC.AddEditGroupView.type = false;
                    DialogHost.Show(new AddEditGroupView(), "Main");
                }

            });

            btn_del = new RelayCommand(p =>
            {
                if (group_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
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
        public void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                }).ContinueWith((previousTask) => {
                    string query = $"delete from user_group " +
                           $"where group_id = '{group_item["group_id"].ToString()}' ";

                    try
                    {
                        Sconn.conn.Open();

                        MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        reader.Close();
                        Sconn.conn.Close();
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1451)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false_in_use");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                        else if (ex.Number == 0)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                        else
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                    }
                    finally
                    {
                        Sconn.conn.Close();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoUserGroupItem(object p)
        {
            IsClear();
            group_data = p as DataGrid;
            string query = $"select * from user_group " +
                           $"left join type on type.type_id = user_group.type_id " +
                           $"order by user_group.group_id";
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                group_data.ItemsSource = dt.DefaultView;
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
                    item_group.Execute(group_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void IsClear()
        {
            edit = true;
            del = true;
            group_item = null;
            group_id = null;
            group_index = 0;
        }

        public void GoUserGroupItemChanged(object p)
        {
            var item = p as DataGrid;
            group_item = item.SelectedItem as DataRowView;
            group_index = item.SelectedIndex;

            if (group_item != null)
            {
                if (group_item["type_name"].ToString() == "admin" ||
                group_item["type_name"].ToString() == "staff")
                {
                    edit = false;
                    del = false;
                }
                else if ((group_item["type_name"].ToString() == "member" ||
                        group_item["type_name"].ToString() == "coupon") &&
                        group_item["group_c_date"].ToString() == "")
                {
                    edit = true;
                    del = false;
                }
                else
                {
                    edit = true;
                    del = true;
                }
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
