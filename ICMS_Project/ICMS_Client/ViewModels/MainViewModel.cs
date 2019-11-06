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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Client
{
    public class MainViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DataTable online_status_data { get; set; }
        public DataRow online_data { get; set; }
        public string txt_start_datetime { get; set; }
        public string txt_all_time { get; set; }
        public string txt_use_time { get; set; }
        public string txt_remaining_time { get; set; }
        public string txt_group_rate { get; set; }
        public string txt_bonus { get; set; }
        MainWindow mainWindow = new MainWindow();
        public DispatcherTimer online_timer = new DispatcherTimer();
        public Window MainApp { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Main;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        public bool account_check { get; set; } = true;
        public bool promotion_check { get; set; } = true;
        public bool log_history_check { get; set; } = true;
        public bool timer_start { get; set; } = false;

        private string conn_number;
        private string all_total_remaining_amount { get; set; }

        #endregion

        #region Commands
        public ICommand btn_account { get; set; }
        public ICommand btn_promotion { get; set; }
        public ICommand online_status { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            online_timer.Interval = new TimeSpan(0, 0, 0, 0, 60000);
            online_timer.Tick += online_timer_tick;

            online_status = new RelayCommand(p => 
            {
                if (online_check() == true && online_status_data.Rows.Count > 0)
                {
                    online_data = online_status_data.Rows[0];
                    if (txt_start_datetime == null)
                    {
                        txt_start_datetime = online_data["v_all_s_time"].ToString();
                    }
                    if (txt_all_time == null)
                    {
                        txt_all_time = string.Format("{0:00}:{1:00}", int.Parse(online_data["v_all_hr"].ToString()), int.Parse(online_data["v_all_mn"].ToString()));
                        all_total_remaining_amount = online_data["v_all_total_remaining_amount"].ToString();
                    }
                    else
                    {
                        if (float.Parse(all_total_remaining_amount) < float.Parse(online_data["v_all_total_remaining_amount"].ToString()))
                        {
                            txt_all_time = string.Format("{0:00}:{1:00}", int.Parse(online_data["v_all_hr"].ToString()), int.Parse(online_data["v_all_mn"].ToString()));
                        }
                    }
                    txt_use_time = string.Format("{0:00}:{1:00}", int.Parse(online_data["v_all_use_hr"].ToString()), int.Parse(online_data["v_all_use_mn"].ToString()));
                    txt_remaining_time = string.Format("{0:00}:{1:00}", int.Parse(online_data["v_all_hr"].ToString()), int.Parse(online_data["v_all_mn"].ToString()));
                    txt_group_rate = online_data["v_group_rate"].ToString();
                    txt_bonus = online_data["v_member_remaining_bonus_point"].ToString();

                    if (timer_start == false)
                    {
                        online_timer.Start();
                        //online_timer.Interval = new TimeSpan(0, 0, 0, 0, 60000);
                    }
                }
                else if(online_check() == true && online_status_data.Rows.Count < 1)
                {
                    Application.Current.MainWindow = new MainWindow()
                    {
                        DataContext = IoC.Application,
                        Width = System.Windows.SystemParameters.PrimaryScreenWidth,
                        Height = System.Windows.SystemParameters.PrimaryScreenHeight,
                        UseNoneWindowStyle = true,
                        IgnoreTaskbarOnMaximize = true,
                        Topmost = true,
                        //TitleAlignment = ,
                        //WindowStyle = WindowStyle.None,
                        AllowsTransparency = true,
                        ResizeMode = ResizeMode.NoResize,
                        WindowState = WindowState.Maximized,
                        ShowInTaskbar = false

                    };
                    IoC.Application.MainApp = Application.Current.MainWindow;
                    Application.Current.MainWindow.ShowDialog();
                }
                else
                {
                    if (conn_number == "0")
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                }
            });
            
            btn_account = new RelayCommand(p =>
            {
                //MessageBox.Show($"{id}");
                Application.Current.MainWindow = new MainWindow()
                {
                    DataContext = IoC.AccountView,
                    Width = Math.Round(System.Windows.SystemParameters.PrimaryScreenWidth * 50 / 100),
                    Height = Math.Round(System.Windows.SystemParameters.PrimaryScreenHeight * 70 / 100)

                };
                IoC.Application.MainApp = Application.Current.MainWindow;
                Application.Current.MainWindow.ShowDialog();
                //istest1();
            });

            btn_promotion = new RelayCommand(p =>
            {
                //DialogHost.Show(new PromotionView(), "Msg");
                Application.Current.MainWindow = new MainWindow()
                {
                    DataContext = IoC.PromotionView,
                    Width = Math.Round(System.Windows.SystemParameters.PrimaryScreenWidth * 50 / 100),
                    Height = Math.Round(System.Windows.SystemParameters.PrimaryScreenHeight * 50 / 100)

                };
                IoC.Application.MainApp = Application.Current.MainWindow;
                Application.Current.MainWindow.ShowDialog();
            });
                
        }

        private void online_timer_tick(object sender, EventArgs e)
        {
            //MessageBox.Show($"{float.Parse(online_data["v_all_remaining_real_amount"].ToString()) + " "+ (60 / (3600 / float.Parse(online_data["v_group_rate"].ToString())))}");
            if (float.Parse(online_data["v_all_remaining_free_amount"].ToString()) >= (60 / (3600 / float.Parse(online_data["v_group_rate"].ToString()))))
            {
                online_use_free_amount = (60 / (3600 / float.Parse(online_data["v_group_rate"].ToString()))).ToString();
                online_use_real_amount = null;
                //MessageBox.Show($"หักจากเงินฟรี,{online_use_free_amount}");
                if (online_update() == true)
                {
                    online_status.Execute("");
                }
                else
                {
                    if (conn_number == "0")
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                }
            }
            else if (float.Parse(online_data["v_all_remaining_real_amount"].ToString()) >= (60 / (3600 / float.Parse(online_data["v_group_rate"].ToString()))))
            {
                online_use_real_amount = (60 / (3600 / float.Parse(online_data["v_group_rate"].ToString()))).ToString();
                online_use_free_amount = null;
                //MessageBox.Show($"หักจากเงินจริง,{online_use_real_amount}");
                if (online_update() == true)
                {
                    online_status.Execute("");
                }
                else
                {
                    if (conn_number == "0")
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", conn_agian);
                    }
                }
            }
            //MessageBox.Show($"{Math.Round(float.Parse(online_data["v_all_remaining_free_amount"].ToString()), 2) >= Math.Round(59 / (3600 / float.Parse(online_data["v_group_rate"].ToString())), 2)}");



        }

        private void conn_agian(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.DialogHostMsg = false;
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {
                    online_check();
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
        }

        public bool online_check()
        {
            string query = $"select * " +
                           //$"concat(v_all_hr,':',v_all_mn) as v_all_remaining_time, " +//เวลาที่เหลือ
                           //$"concat(v_all_use_hr,':',v_all_use_mn) as v_all_use_remaining_time " +//เวลาที่ใช้ไป
                           $"from v_computer_status " +
                           $"where v_pc_id = '{IoC.Application.row["pc_id"].ToString()}' ";
            // $"inner join user_group on user_group.group_id = admin.group_id ";
            //$"inner join user_group on staff.group_id = user_group.group_id";
            //MessageBox.Show($"{query}");
            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    online_status_data = dt;
                    Sconn.conn.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    conn_number = ex.Number.ToString();
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

        private string online_use_free_amount, online_use_real_amount;
        private bool online_update()
        {
            //txt_add_money = Math.Round((float.Parse(new_seconds) / float.Parse(group_rate)), 2).ToString();
            string query = $"update online " +
                           $"left join v_online_update " +
                           $"on v_online_update.v_online_pc_id_x = online.online_pc_id " +
                           $"and v_online_update.v_online_ordinal_x = online.online_ordinal set " +
                           $"online.online_use_real_amount = v_online_update.v_online_use_real_amount_x + coalesce('{online_use_real_amount}',0), " +
                           $"online.online_use_free_amount = v_online_update.v_online_use_free_amount_x + coalesce('{online_use_free_amount}',0)," +
                           $"online.online_e_datetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' " +
                           $"where online_pc_id = '{IoC.Application.row["pc_id"].ToString()}' " +
                           $"and online_ordinal = (select v_online_ordinal " +
                                                  $"from v_online_ordinal " +
                                                  $"where v_online_pc_id = '{IoC.Application.row["pc_id"].ToString()}') ";
            // $"inner join user_group on user_group.group_id = admin.group_id ";
            //$"inner join user_group on staff.group_id = user_group.group_id";
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
                    conn_number = ex.Number.ToString();
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
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show(ex.ToString());
                        break;
                    case 1045:
                        MessageBox.Show(ex.ToString());
                        break;
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
