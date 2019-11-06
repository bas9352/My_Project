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
    public class AddEditBonusViewModel :BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public DataTable data { get; set; }
        public DataTable data_mt { get; set; }
        public bool on_off_bonus { get; set; } = false;

        public bool grid_add_edit_b_check { get; set; } = true;

        private int conn_number;

        public string txt_bonus_top_up { get; set; } = null;
        public string txt_bonus_point { get; set; } = null;
        public string bonus_id { get; set; } = null;
        #endregion
        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand on_off_bonus_status { get; set; }
        #endregion
        #region Constructor
        public AddEditBonusViewModel()
        {
            btn_ok = new RelayCommand(p=> 
            {
                grid_add_edit_b_check = false;
                if (txt_bonus_top_up == null || txt_bonus_point == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        if (bonus_id == null)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];

                                if (row["bonus_top_up"].ToString() == txt_bonus_top_up && 
                                    row["bonus_status"].ToString() == "true" &&
                                    on_off_bonus == true)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("bonus_topup_unsuccess");
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
                                        IoC.WarningView.msg_text = GetLocalizedValue<string>("edit_unsuccess");
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
                                if (row["bonus_top_up"].ToString() == txt_bonus_top_up &&
                                    row["bonus_status"].ToString() == "true" &&
                                    on_off_bonus == true &&
                                    row["bonus_id"].ToString() != bonus_id)
                                {
                                    //MessageBox.Show($"{"1"}");
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("bonus_topup_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                            }

                            if (num == 0)
                            {
                                if (IsSelectCheckMT() == true)
                                {
                                    int num_bonus = 0;
                                    for (int i =0; i< data_mt.Rows.Count; i++)
                                    {
                                        var item = data_mt.Rows[i];

                                        if (item["v_bonus_id"].ToString() == bonus_id && item["v_bonus_point"].ToString() != txt_bonus_point)
                                        {
                                            num_bonus = 1;
                                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                            IoC.WarningView.msg_text = GetLocalizedValue<string>("cant_edit_bonus");
                                            DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                        }
                                    }

                                    if (num_bonus == 0)
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
                                                //MessageBox.Show($"{"3"}");
                                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                                IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                                                DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                            }
                                            else
                                            {
                                                //MessageBox.Show($"{"4"}");
                                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                                IoC.WarningView.msg_text = GetLocalizedValue<string>("edit_unsuccess");
                                                DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                            }

                                        }
                                    }
                                }
                                else if (conn_number == 0)
                                {
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
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
                IsClear();
                IoC.Application.DialogHostInMain = false;
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new AddEditGroupView(), "Main");
            });
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.AddEditGroupView.item_bonus.Execute(IoC.AddEditGroupView.bonus_data);
                grid_add_edit_b_check = true;
                IoC.Application.DialogHostInMain = false;
                //IoC.Application.DialogHostMain = false;
                //DialogHost.Show(new AddEditGroupView(), "Main");
                IsClear();
                //IoC.UserGroupView.IsSelect();
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_b_check = true;
            }
        }

        public bool IsInsert()
        {
            string query = $"insert into bonus set " +
                           $"bonus_top_up = '{txt_bonus_top_up}', " +
                           $"bonus_point = '{txt_bonus_point}', " +
                           $"bonus_c_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}', " +
                           $"bonus_status = '{on_off_bonus}'";
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
            string query = $"update bonus set " +
                           $"bonus_top_up='{txt_bonus_top_up}', " +
                           $"bonus_point='{txt_bonus_point}', " +
                           $"bonus_status = {on_off_bonus} " +
                           $"where bonus_id = '{bonus_id}'";

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

        public void IsClear()
        {
            IoC.AddEditGroupView.bonus_item = null;
            txt_bonus_top_up = null;
            txt_bonus_point = null;
            on_off_bonus = false;
            IoC.AddEditGroupView.grid_add_edit_g_check = true;
        }

        public bool IsSelect()
        {
            string query = $"select * from bonus order by bonus_id;";

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

        public bool IsSelectCheckMT()
        {
            string query = $"select *" +
                           $"from v_bonus_check";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data_mt = dt;
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

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
