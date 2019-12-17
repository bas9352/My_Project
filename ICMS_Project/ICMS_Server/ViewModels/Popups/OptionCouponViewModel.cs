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
    public class OptionCouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DataRowView coupon_item { get; set; }
        public int coupon_index { get; set; }
        public bool button_add { get; set; } = false;
        public bool grid_op_c_check { get; set; } = true;

        public DataGrid op_c_data { get; set; }
        public DataTable check_option_coupon { get; set; }
        public Window MainApp { get; set; }
        public bool list_menu { get; set; } = false;

        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostMain { get; set; }
        public bool DialogHostInMain { get; set; }
        public bool DialogHostMsg { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Member;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        public int index { get; set; }
        public string query { get; set; }

        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }

        public ICommand item_coupon { get; set; }
        public ICommand item_coupon_changed { get; set; }
        #endregion

        #region Constructor
        public OptionCouponViewModel()
        {
            item_coupon = new RelayCommand(p=> GoItemCoupon(p));
            item_coupon_changed = new RelayCommand(p => GoItemCouponChanged(p));

            btn_add = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                IoC.AddEditOptionCouponView.op_c_id = null;
                DialogHost.Show(new AddEditOptionCouponView(), "InMain");
            });

            btn_edit = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                if (coupon_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.AddEditOptionCouponView.op_c_id = coupon_item["op_c_id"].ToString();
                    IoC.AddEditOptionCouponView.txt_username = coupon_item["op_c_name"].ToString();
                    IoC.AddEditOptionCouponView.group_id = coupon_item["group_id"].ToString();
                    IoC.AddEditOptionCouponView.txt_hr_rate = coupon_item["group_rate"].ToString();
                    //MessageBox.Show($"{coupon_item["op_c_real_amount"].ToString()}");
                    if (coupon_item["op_c_real_amount"].ToString() == "0")
                    {
                        IoC.AddEditOptionCouponView.txt_hr_price = "";
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.txt_hr_price = coupon_item["op_c_real_amount"].ToString();
                    }
                    if (coupon_item["op_c_free_amount"].ToString() == "0")
                    {
                        IoC.AddEditOptionCouponView.txt_free_money = "";
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.txt_free_money = coupon_item["op_c_free_amount"].ToString();
                    }
                    IoC.AddEditOptionCouponView.g_hr_price = coupon_item["op_c_real_amount"].ToString();
                    IoC.AddEditOptionCouponView.g_free_money = coupon_item["op_c_free_amount"].ToString();
                    IoC.AddEditOptionCouponView.txt_exp_date = coupon_item["op_c_e_date"].ToString();
                    if (coupon_item["op_c_s_date"].ToString() == "true")
                    {
                        IoC.AddEditOptionCouponView.start_create_date = true;
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.start_first_use = true;
                    }
                    //IoC.AddEditCouponView.txt_total_amount = "0";
                    IoC.AddEditOptionCouponView.IsChanged();
                    IoC.AddEditOptionCouponView.txt_total_amount = (float.Parse(IoC.AddEditOptionCouponView.g_hr_price) + float.Parse(IoC.AddEditOptionCouponView.g_free_money)).ToString();
                    IoC.AddEditOptionCouponView.seconds = ((3600 / float.Parse(IoC.AddEditOptionCouponView.txt_hr_rate)) * float.Parse(IoC.AddEditOptionCouponView.txt_total_amount)).ToString();
                    IoC.AddEditOptionCouponView.txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.AddEditOptionCouponView.seconds) / 3600));
                    IoC.AddEditOptionCouponView.txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.AddEditOptionCouponView.seconds) / 60) % 60));
                    
                    DialogHost.Show(new AddEditOptionCouponView(), "InMain");
                }

            });

            btn_del = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                if (coupon_item == null)
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

            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new GenerateCouponView(), "Main");
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
                    if (IsCheckOptionCoupon() == true)
                    {
                        int num = 0;
                        for (int i = 0; i < check_option_coupon.Rows.Count; i++)
                        {
                            DataRow row = check_option_coupon.Rows[i];
                            if (row["v_op_c"].ToString() == "true" &&
                                row["v_op_c_id"].ToString() == coupon_item["op_c_id"].ToString())
                            {
                                num = 1;
                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                IoC.WarningView.msg_text = GetLocalizedValue<string>("cant_del_op_c");
                                DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                            }
                        }

                        if (num == 0)
                        {

                            string query = $"select * from option_coupon";

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
                                DataRow dr = dt.Rows[coupon_index];
                                dr.Delete();
                                adp.Update(dt);

                                IoC.Application.DialogHostMain = false;
                                DialogHost.Show(new OptionCouponView(), "Main");
                            }
                            catch (MySqlException ex)
                            {
                                if (ex.Number == 1451)
                                {
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false_in_use");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                                else if (ex.Number == 0)
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
                            }
                            finally
                            {
                                Sconn.conn.Close();
                            }
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                grid_op_c_check = true;
            }
        }
        public bool IsCheckOptionCoupon()
        {
            string query = $"select * from v_op_c";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                check_option_coupon = dt;
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
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_op_c_check = true;
            }
        }

        private void GoItemCouponChanged(object p)
        {
            var item = p as DataGrid;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_index = item.SelectedIndex;
        }

        private void GoItemCoupon(object p)
        {
            IsClear();
            grid_op_c_check = false;
            op_c_data = p as DataGrid;
            string query = $"select * from option_coupon " +
                           $"inner join user_group on user_group.group_id = option_coupon.group_id " +
                           $"order by option_coupon.op_c_id";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                op_c_data.ItemsSource = dt.DefaultView;
                if (op_c_data != null)
                {
                    grid_op_c_check = true;
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

        public void IsClear()
        {
            coupon_item = null;
            coupon_index = 0;
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
                    item_coupon.Execute(op_c_data);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
