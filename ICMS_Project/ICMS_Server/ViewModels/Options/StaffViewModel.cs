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
using System.Windows.Threading;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class StaffViewModel : BaseView
    {
        #region Properties
        public bool add_check { get; set; } = true;
        public bool del_check { get; set; } = true;
        //public ObservableCollection<StaffViewModel> LoadData { get; set; }
        Database Sconn = new Database();
        public DataRowView staff_item { get; set; } = null;
        public DataGrid staff_data { get; set; } = null;
        public int staff_index { get; set; }

        private int conn_number;

        #endregion

        #region Commands
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand item_staff { get; set; }
        public ICommand item_staff_change { get; set; }


        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public StaffViewModel()
        {
            item_staff = new RelayCommand(p => GoStaffItem(p));
            item_staff_change = new RelayCommand(p => GoStaffItemChange(p));
            //IsSelect();
            btn_add = new RelayCommand(p =>
            {
                IoC.AddEditStaffView.staff_id = null;
                DialogHost.Show(new AddEditStaffView(), "Main");
            });
            btn_edit = new RelayCommand(p =>
            {
                if (staff_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.AddEditStaffView.txt_username = staff_item["staff_username"].ToString();
                    IoC.AddEditStaffView.txt_password = staff_item["staff_password"].ToString();
                    IoC.AddEditStaffView.group_id = staff_item["group_id"].ToString();
                    IoC.AddEditStaffView.txt_name = staff_item["staff_name"].ToString();
                    IoC.AddEditStaffView.txt_lastname = staff_item["staff_lastname"].ToString();
                    IoC.AddEditStaffView.txt_nickname = staff_item["staff_nickname"].ToString();
                    IoC.AddEditStaffView.txt_birthday = staff_item["staff_birthday"].ToString();
                    IoC.AddEditStaffView.txt_tel = staff_item["staff_tel"].ToString();
                    IoC.AddEditStaffView.txt_email = staff_item["staff_email"].ToString();
                    IoC.AddEditStaffView.txt_address = staff_item["staff_address"].ToString();
                    IoC.AddEditStaffView.txt_id_card = staff_item["staff_id_card"].ToString();
                    IoC.AddEditStaffView.txt_c_date = staff_item["staff_c_date"].ToString();
                    IoC.AddEditStaffView.txt_s_date = staff_item["staff_s_date"].ToString();

                    if (staff_item["staff_e_date"].ToString() == null || staff_item["staff_e_date"].ToString() == "")
                    {
                        IoC.AddEditStaffView.IsCheck = false;
                        IoC.AddEditStaffView.end_date = false;
                        IoC.AddEditStaffView.txt_e_date = staff_item["staff_e_date"].ToString();
                    }
                    else
                    {
                        IoC.AddEditStaffView.IsCheck = true;
                        IoC.AddEditStaffView.end_date = true;
                        IoC.AddEditStaffView.txt_e_date = staff_item["staff_e_date"].ToString();
                    }
                    //IsRow();
                    IoC.AddEditStaffView.staff_id = staff_item["staff_id"].ToString();
                    //CurrPage = ApplicationPage.Admin;
                    DialogHost.Show(new AddEditStaffView(), "Main");
                }
                
            });

            btn_del = new RelayCommand(p =>
            {
                if (staff_item == null)
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

        public void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                
                Task.Factory.StartNew(() =>
                {
                }).ContinueWith((previousTask) => {
                    string query = $"delete from staff " +
                                   $"where staff_id = '{staff_item["staff_id"].ToString()}' ";

                    try
                    {
                        if (OpenConnection() == true)
                        {
                            MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                            MySqlDataReader reader = cmd.ExecuteReader();

                            reader.Close();
                            Sconn.conn.Close();
                            item_staff.Execute(staff_data);
                        }
                        else
                        {
                            Sconn.conn.Close();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1451)
                        {
                            //IoC.Application.DialogHostMsg = false;
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false_in_use");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                        else if (ex.Number == 0)
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
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                        Sconn.conn.Close();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GoStaffItem(object p)
        {
            //MessageBox.Show($"{IoC.LoginView.login_data["staff_id"]}");
            IsClear();
            //var item = p as DataGrid;
            staff_data = p as DataGrid;
            string query;
            if (IoC.LoginView.login_data["type_name"].ToString() == "staff")
            {
                query = $"select * from staff " +
                        $"left join user_group on user_group.group_id = staff.group_id " +
                        $"left join type on type.type_id = user_group.type_id " +
                        $"where staff_id = '{IoC.LoginView.login_data["staff_id"]}' order by staff_id";
            }
            else
            {
                query = $"select * from staff " +
                        $"left join user_group on user_group.group_id = staff.group_id " +
                        $"left join type on type.type_id = user_group.type_id " +
                        $"where type_name like '%staff%' ";
            }

            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    staff_data.ItemsSource = dt.DefaultView;
                    Sconn.conn.Close();
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
            }
        }

        public void IsClear()
        {
            staff_item = null;
            staff_index = 0;
        }

        public void GoStaffItemChange(object p)
        {
            var item = p as DataGrid;
            staff_item = item.SelectedItem as DataRowView;
            staff_index = item.SelectedIndex;
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
