using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    public class LogHistoryViewModel : BaseView
    {
        #region Properties
        public bool add_check { get; set; } = true;
        public bool del_check { get; set; } = true;
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        //public ObservableCollection<StaffViewModel> LoadData { get; set; }
        Database Sconn = new Database();
        public DataRowView online_item { get; set; } = null;
        public DataGrid online_data { get; set; } = null;
        public ComboBox cmb { get; set; }
        public int staff_index { get; set; }
        public string txt_search { get; set; }
        public string txt_search_null { get; set; }
        public string search_text { get; set; }
        public string txt_start_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_end_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));


        DispatcherTimer online_timer = new DispatcherTimer();

        #endregion

        #region Commands
        public ICommand btn_search { get; set; }
        public ICommand item_online { get; set; }
        public ICommand item_online_changed { get; set; }
        public ICommand item_search { get; set; }
        public ICommand item_search_changed { get; set; }
        public ICommand txt_search_changed { get; set; }
        #endregion

        #region Constructor
        public LogHistoryViewModel()
        {
            btn_search = new RelayCommand(p => 
            {
                item_online.Execute(online_data);
            });
            item_search = new RelayCommand(p => GoItemSearch(p));
            item_search_changed = new RelayCommand(p => GoItemSearchChanged(p));
            item_online = new RelayCommand(p => GoItemOnline(p));
            txt_search_changed = new RelayCommand(p=> GoTxtSearchChanged(p));

            online_timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            online_timer.Tick += online_timer_tick;
            online_timer.Start();
        }
        #endregion

        #region Other method
        private void online_timer_tick(object sender, EventArgs e)
        {
            if (IoC.MainView.CurrPage == ApplicationPage.LogHistory)
            {
                if (online_data != null)
                {
                    item_online.Execute(online_data);
                }
            }
            else
            {
                online_timer.Stop();
            }
        }

        DateTime temp;
        private string query;
        private string start_date, end_date;
        private void GoItemOnline(object p)
        {
            if (cmb.ItemsSource == null)
            {
                cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
                {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("username"), "username"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("pc_name"), "pc_name")
                };
                cmb.ItemsSource = cmb_data;
                cmb.SelectedValuePath = "Value";
                cmb.DisplayMemberPath = "Key";
                cmb.SelectedIndex = 0;
            }
            online_data = p as DataGrid;
            try
            {
                temp = Convert.ToDateTime(txt_start_date);
                start_date = temp.ToString("yyyy-MM-dd");
            }
            catch
            {
                start_date = null;
            }

            try
            {
                temp = Convert.ToDateTime(txt_end_date);
                end_date = temp.ToString("yyyy-MM-dd");
            }
            catch
            {
                end_date = null;
            }

            if (search_text == "username")
            {
                query = $"select * " +
                        $"from v_online_history " +
                        $"where if('{start_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '{start_date}' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}') and " +
                                $"if('{end_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '1999-01-01' and '{end_date}') and " +
                        $"if('{search_text}' = '{"username"}' and '{txt_search}' = '{""}', v_online_id = v_online_id, v_all_username like '%{txt_search}%') " +
                        $"order by v_online_id";
            }
            else if (search_text == "pc_name")
            {
                query = $"select * " +
                        $"from v_online_history " +
                        $"where if('{start_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '{start_date}' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}') and " +
                                $"if('{end_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '1999-01-01' and '{end_date}') and " +
                        $"if('{search_text}' = '{"_pc_name"}' and '{txt_search}' = '{""}', v_online_id = v_online_id, v_online_pc_name like '%{txt_search}%') " +
                        $"order by v_online_id";
            }
            else
            {
                query = $"select * " +
                        $"from v_online_history " +
                        $"where if('{start_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '{start_date}' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}') and " +
                                $"if('{end_date}' = '{""}', v_online_s_date = v_online_s_date, v_online_s_date between '1999-01-01' and '{end_date}') " +
                        $"order by v_online_id";
            }
            
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();

                dt.Columns.Add("new_v_online_use_time", typeof(string));
                dt.Columns.Add("new_v_online_s_date", typeof(string));
                foreach (DataRow data in dt.Rows)
                {
                    data["new_v_online_use_time"] = string.Format("{0:00:}{1:00}", data["v_all_hr"], data["v_all_mn"]);
                    data["new_v_online_s_date"] = DateTime.Parse(data["v_online_s_date"].ToString()).ToString("dd/MM/yyyy");
                }
                online_data.ItemsSource = dt.DefaultView;
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
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
            }
        }
        public void GoItemSearch(object p)
        {
            var item = p as ComboBox;
            cmb = item;
            
        }
        public void GoTxtSearchChanged(object p)
        {
            var item = p as TextBox;
            txt_search = item.Text;
        }

        public void GoItemSearchChanged(object p)
        {
            var item = p as ComboBox;
            if (item.ItemsSource != null)
            {
                search_text = ((KeyValuePair<string, string>)item.SelectedItem).Value;
            }

        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
