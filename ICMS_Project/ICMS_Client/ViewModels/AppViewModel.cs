using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Input;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Client
{
    public class AppViewModel : BaseView
    {

        #region Properties
        Database Sconn = new Database();
        public DataRow row { get; set; }
        public Window MainApp { get; set; }
        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostOpen { get; set; }
        public bool DialogHostMain { get; set; }
        public bool DialogHostInMain { get; set; }
        public bool DialogHostMsg { get; set; }
        public ApplicationPage CurrPage { get; set; }

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }
        public DataTable data { get; set; }

        public string host_name { get; set; }
        public string ip_address { get; set; }
        public string mac_address { get; set; }

        private int conn_number;



        #endregion

        #region Commands
        public ICommand ToggleBaseCommand { get; set; }

        public ICommand btn_switchMode { get; set; }

        public ICommand btn_ViewHelp { get; set; }

        public ICommand btn_About { get; set; }

        public ICommand btn_setting { get; set; }
        public ICommand btn_sendFeedBack { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }
        public ICommand ip_check { get; set; }
        #endregion

        #region Constructor

        public AppViewModel()
        {
            ip_check = new RelayCommand(p=> 
            {
                go_ip_check();
            });
        }

        #endregion

        #region Orther Mathod
        public void go_ip_check()
        {
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    host_name = Dns.GetHostName();
                    ip_address = IPA.ToString();
                    break;
                }
            }

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                string macAddr = mo["MacAddress"] as string;
                if (macAddr != null && macAddr.Trim() != "")
                {
                    mac_address = macAddr.ToString();
                    break;
                }

            }

            if (IsSelect() == true && data.Rows.Count > 0)
            {
                //MessageBox.Show("2");
                row = data.Rows[0];
                if (row["pc_ip_address"].ToString() != ip_address ||
                    row["pc_name"].ToString() != host_name)
                {
                    //MessageBox.Show("2-1");
                    if (IsUpdate() == false)
                    {
                        if (conn_number == 0)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                            DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                        }
                        else
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                            DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                        }
                    }
                    else
                    {
                        CurrPage = ApplicationPage.Login;
                    }
                }
                else
                {
                    //MessageBox.Show("2-2");
                    if (IsOnline() == false)
                    {
                        if (conn_number == 0)
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                            DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                        }
                        else
                        {
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                            DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                        }
                    }
                    else
                    {
                        CurrPage = ApplicationPage.Login;
                    }
                }
                
            }
            else if(IsSelect() == true && data.Rows.Count < 1)
            {
                if (IsInsert() == false)
                {
                    if (conn_number == 0)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                        DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                    }
                }
                else
                {
                    //MessageBox.Show("1");
                    go_ip_check();
                }
            }
            else
            {
                if (conn_number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                    DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_unsuccess");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_false");
                    DialogHost.Show(new WarningView(), "RootDialogMain", ConfirmClosingEventHandler);
                }
            }
        }

        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                DialogHostOpen = false;
                Task.Factory.StartNew(async () =>
                {                    
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {

                    go_ip_check();

                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
        }

        public bool IsSelect()
        {
            string query = $"select * " +
                           $"from computer " +
                           $"where pc_mac_address = '{mac_address}' ";
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
                    data = dt;
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
        public bool IsInsert()
        {
            string query = $"insert into computer set " +
                           $"pc_name = '{host_name}', " +
                           $"pc_ip_address = '{ip_address}', " +
                           $"pc_mac_address = '{mac_address}' ";

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
                    MessageBox.Show(ex.Number.ToString());
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
            string query = $"update computer set  " +
                           $"pc_name = '{host_name}', " +
                           $"pc_ip_address ='{ip_address}' " +
                           $"where pc_mac_address ='{mac_address}'; " +

                           $"insert into online set " +
                           $"online_pc_id = (select pc_id " +
                                            $"from computer " +
                                            $"where pc_mac_address = '{mac_address}'), " +
                           $"online_status = '{"available"}', " +
                           $"online_ordinal = (select max(a.online_ordinal)+1 " +
                                              $"from online a " +
                                              $"where a.online_pc_id = '{row["pc_id"].ToString()}'), " +
                           $"online_s_datetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";

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

        public bool IsOnline()
        {
            string query = $"insert into online set " +
                           $"online_pc_id = (select pc_id " +
                                            $"from computer " +
                                            $"where pc_mac_address = '{mac_address}'), " +
                           $"online_status = '{"available"}', " +
                           $"online_ordinal = (select coalesce(max(a.online_ordinal),0)+1 " +
                                              $"from online a " +
                                              $"where a.online_pc_id = '{row["pc_id"].ToString()}'), " +
                           $"online_s_datetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";
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

        //public System.Windows.Forms.NotifyIcon appNotifyIcon = new System.Windows.Forms.NotifyIcon();
        public void setSystemTray()
        {

        }

    }
}
