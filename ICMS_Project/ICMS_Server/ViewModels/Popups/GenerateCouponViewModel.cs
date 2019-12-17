﻿using CrystalDecisions.CrystalReports.Engine;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class GenerateCouponViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public bool option_coupon_check { get; set; } = true;
        public bool grid_g_coupon { get; set; } = true;
        public bool finished { get; set; }
        public DataTable data { get; set; }
        public DataGrid grid_data { get; set; }
        public DataRowView coupon_item { get; set; }
        public DataRow id_data { get; set; }
        public ComboBox combobox_data { get; set; }
        public string txt_create_amount { get; set; } = "1";
        public string coupon_s_date { get; set; }
        public string coupon_e_date { get; set; }
        public string coupon_c_date { get; set; }
        public string day { get; set; }
        public int row_count { get; set; }

        public int number { get; set; }
        public string coupon_username {get;set;}
        public string coupon_password { get; set; }
        public string group_id { get; set; }
        public string coupon_id { get; set; }
        public string seconds { get; set; }
        public string coupon_hr_rate { get; set; }
        public string coupon_real_amount { get; set;}
        public string coupon_free_amount { get; set; }
        public string coupon_total_amount { get; set; }
        public string coupon_time { get; set; }
        public string op_c_id { get; set; }

        public List<string> query_a = new List<string>();
        public List<string> query_b = new List<string>();
        public string sub_a = null;
        public string sub_b = null;
        public List<CouponClass> coupon_class = new List<CouponClass>();
        //public List<GridCoupon> grid_coupon = new List<GridCoupon>();

        public class GridCoupon
        {
            public int number { get; internal set; }
            public string coupon_username { get; internal set; }
            public string coupon_password { get; internal set; }
            public string coupon_pass { get; internal set; }
            public string coupon_price { get; internal set; }
            public string coupon_end_date { get; internal set; }
            public string coupon_time { get; internal set; }
            public string group_id { get; internal set; }
            public string coupon_free_money { get; internal set; }
            public string coupon_total_amount { get; internal set; }
            public string coupon_create_date { get; internal set; }
            public string coupon_start_date { get; internal set; }
            public string day { get; internal set; }
            public string op_c_id { get; internal set; }
        }
        #endregion

        #region Commands        
        public ICommand btn_save { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_option_coupon { get; set; }
        public ICommand btn_create_coupon { get; set; }
        public ICommand item_coupon { get; set; }
        public ICommand item_coupon_changed { get; set; }
        public ICommand data_coupon { get; set; }
        public ICommand data_coupon_changed { get; set; }
        #endregion

        #region Constructor
        public GenerateCouponViewModel()
        {
            item_coupon = new RelayCommand(p => GoItemCoupon(p));
            item_coupon_changed = new RelayCommand(p => GoItemCouponChanged(p));
            data_coupon = new RelayCommand(p => GoDataCoupon(p));

            btn_option_coupon = new RelayCommand(p=> 
            {
                IsClear();
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new OptionCouponView(), "Main");
            });

            btn_create_coupon = new RelayCommand(p =>
            {
                if (txt_create_amount == "" || txt_create_amount == null || txt_create_amount == "0")
                {
                    grid_g_coupon = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("create");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (coupon_item["v_op_c_s_date"].ToString() == "true")
                    {
                        coupon_c_date = coupon_s_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false));
                        day = DateTime.Now.AddDays(int.Parse(coupon_item["v_op_c_e_date"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                        coupon_e_date = DateTime.Parse(day).ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
                        //MessageBox.Show($"ตามวันที่สร้าง");
                    }
                    else if (coupon_item["v_op_c_s_date"].ToString() == "false")
                    {
                        coupon_s_date = null;
                        coupon_c_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false));
                        day = coupon_item["v_op_c_e_date"].ToString() + " " + GetLocalizedValue<string>("after_use"); ;
                        coupon_e_date = day;
                        //MessageBox.Show($"ตามวันที่ถูกใช้ครั้งแรก");
                    }

                    for (int i = 0; i < int.Parse(txt_create_amount); i++)
                    {
                        var seconds = ((3600 / float.Parse(coupon_item["v_group_rate"].ToString())) * float.Parse(coupon_item["v_total_op_c_amount"].ToString())).ToString();
                        //MessageBox.Show($"{i}");
                        //grid_coupon = new List<GridCoupon>();
                        grid_data.Items.Add(new GridCoupon
                        {
                            number = grid_data.Items.Count + 1,
                            coupon_username = coupon_item["v_op_c_name"] + GeneratePassword(2, 2, 1),
                            coupon_password = GeneratePassword(3, 3, 2),
                            group_id = coupon_item["v_group_id"].ToString(),
                            coupon_price = coupon_item["v_group_rate"].ToString(),
                            coupon_free_money = coupon_item["v_op_c_real_amount"].ToString(),
                            coupon_total_amount = coupon_item["v_total_op_c_amount"].ToString(),
                            coupon_create_date = coupon_c_date,
                            coupon_start_date = coupon_s_date,
                            coupon_end_date = coupon_e_date,
                            coupon_time = (Math.Floor(float.Parse(seconds) / 3600)) + " " + GetLocalizedValue<string>("hr") + " " + (Math.Round((float.Parse(seconds) / 60) % 60)) + " " + GetLocalizedValue<string>("mn"),
                            day = day,
                            op_c_id = coupon_item["v_op_c_id"].ToString()
                        });
                        //grid_data.Items.Add(grid_coupon);
                    }
                }
            });

            btn_save = new RelayCommand(p =>
            {
                grid_g_coupon = false;
                if (grid_data.Items.Count < 1)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("no_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    //DialogHost.Show(new ProgressBarView(), "InMain");

                    if (IsSelect() == true)
                    {
                        id_data = data.Rows[0];

                        //Task.Factory.StartNew(() =>
                        //{
                        //    for (int i = 0; i < grid_coupon.Count; i++)
                        //    {
                        //        if (grid_coupon[i].coupon_end_date != grid_coupon[i].day)
                        //        {
                        //            var end_date = DateTime.Parse(grid_coupon[i].coupon_end_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                        //            grid_coupon[i].coupon_end_date = end_date;
                        //        }
                        //        query_a.Add($"'{grid_coupon[i].coupon_username}', " +
                        //                    $"AES_ENCRYPT('{grid_coupon[i].coupon_password}', 'dead_project'), " +
                        //                    $"'{grid_coupon[i].coupon_start_date}', " +
                        //                    $"'{grid_coupon[i].coupon_end_date}', " +
                        //                    $"'{grid_coupon[i].coupon_create_date}', " +
                        //                    $"'{IoC.LoginView.login_id}', " +
                        //                    $"'{grid_coupon[i].op_c_id}'");

                        //        query_b.Add($"'{int.Parse(id_data["id_data"].ToString()) + i}', " +
                        //                    $"'{IoC.LoginView.login_id}', " +
                        //                    $"'1', " +
                        //                    $"'{grid_coupon[i].coupon_price}', " +
                        //                    $"'{grid_coupon[i].coupon_free_money}', " +
                        //                    $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}'");

                        //        if (sub_a == null && sub_b == null)
                        //        {
                        //            sub_a = "(" + query_a[i] + ")";
                        //            sub_b = "(" + query_b[i] + ")";
                        //        }
                        //        else
                        //        {
                        //            sub_a = sub_a + ", (" + query_a[i] + ")";
                        //            sub_b = sub_b + ", (" + query_b[i] + ")";
                        //        }

                        //        //coupon_class = new List<CouponClass>
                        //        //{
                        //        //    new CouponClass
                        //        //    {
                        //        //        r_coupon_id =  int.Parse(id_data["id_data"].ToString()),
                        //        //        r_coupon_username = grid_coupon[i].coupon_username,
                        //        //        r_coupon_password = grid_coupon[i].coupon_password,
                        //        //        r_coupon_e_date = grid_coupon[i].coupon_end_date,

                        //        //    }
                        //        //};
                        //    }

                        //}).ContinueWith((previousTask) =>
                        //{
                        //    if (IsInsert() == true)
                        //    {
                        //        IoC.Application.DialogHostInMain = false;
                        //        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                        //        IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                        //        DialogHost.Show(new WarningView(), "Msg", OpenReport);
                        //    }
                        //}, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                }
            });

            btn_cancel = new RelayCommand(p=> 
            {
                IsClear();
                IoC.Application.DialogHostMain = false;
            });
        }
        #endregion

        #region Other method
        public bool IsSelect()
        {
            string query = $"select AUTO_INCREMENT as id_data " +
                           $"from INFORMATION_SCHEMA.TABLES " +
                           $"where TABLE_SCHEMA = 'icms' " +
                           $"and TABLE_NAME = 'coupon'";

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
                    DialogHost.Show(new WarningView(), "Msg", insert_fail);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", insert_fail);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public bool IsInsert()
        {
            string query = $"insert into coupon " +
                           $"(coupon_username, " +
                           $"coupon_password, " +
                           $"coupon_s_date, " +
                           $"coupon_e_date, " +
                           $"coupon_c_date, " +
                           $"coupon_create_by, " +
                           $"op_c_id) " +
                           $"value " +
                           $"{sub_a};" +

                           $"insert into coupon_top_up " +
                           $"(ct_coupon_id, " +
                           $"ct_by, " +
                           $"ct_ordinal, " +
                           $"ct_real_amount, " +
                           $"ct_free_amount, " +
                           $"ct_date) " +
                           $"value " +
                           $"{sub_b}";

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
                    DialogHost.Show(new WarningView(), "Msg", insert_fail);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", insert_fail);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }
        private void insert_fail(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.DialogHostInMain = false;
                grid_g_coupon = true;
            }
        }

        private void OpenReport(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                string query = $"select *, " +
                               $"cast(AES_DECRYPT(r_coupon_password, 'dead_project') as char) as new_r_coupon_password " +
                               $"from r_coupon_save " +
                               $"where r_coupon_id between '{id_data["id_data"].ToString()}' and (select max(coupon_id) from coupon)";

                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    
                    adp.Fill(ds, "DataSetReport");
                    Sconn.conn.Close();
                    dt = ds.Tables[0];
                    //dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                    //foreach (DataRow data in dt.Rows)
                    //{
                    //    data["r_coupon_e_date"] = "test";
                    //}
                    IoC.ReportViewer.rpt = new ReportDocument();
                    IoC.ReportViewer.rpt.Load("C:\\Users\\ธรณ์ธันย์\\Desktop\\My_Project\\ICMS_Project\\ICMS_Server\\ViewModels\\Reports\\CrystalReports\\ReportCoupon.rpt");
                    IoC.ReportViewer.rpt.SetDataSource(dt);
                    //มีปัญหาไม่ล้างค่า
                    IoC.ReportViewer.report_viewer = new ReportViewer();
                    IoC.ReportViewer.report_viewer.item_report.ViewerCore.ReportSource = IoC.ReportViewer.rpt;
                    IoC.ReportViewer.report_viewer.Show();

                    //App.Current.MainWindow = new ReportViewer()
                    //{
                    //    DataContext = IoC.ReportViewer
                    //};
                    //IoC.ReportViewer.item_report.Execute(IoC.ReportViewer.report_data);
                    //IoC.Application.MainApp = App.Current.MainWindow;
                    //App.Current.MainWindow.Show();




                    IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
                    grid_g_coupon = true;
                    IoC.Application.DialogHostMain = false;
                    IsClear();
                }
                catch (MySqlException ex)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        grid_g_coupon = false;
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
        }

        private void UnsuccessClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_g_coupon = true;
            }
        }

        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_g_coupon = true;
            }
        }

        private void IsClear()
        {
            coupon_item = null;
            coupon_id = null;
            data = null;
            txt_create_amount = "1";
        }

        private void GoDataCoupon(object p)
        {
            //IsClear();
            grid_data = p as DataGrid;
        }

        public static string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);

        }

        private void GoItemCouponChanged(object p)
        {
            var item = p as ComboBox;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_id = coupon_item["v_op_c_id"].ToString();
        }

        private void GoItemCoupon(object p)
        {
            combobox_data = p as ComboBox;
            string query = $"select *, concat(v_op_c_name, ', ', '{GetLocalizedValue<string>("price")}',' ', " +
                           $"v_op_c_real_amount,' ','{GetLocalizedValue<string>("baht")}',', ', " +
                           $"'{GetLocalizedValue<string>("time")}',' ',v_all_hr,' ','{GetLocalizedValue<string>("hr")}',' ',v_all_mn,' ','{GetLocalizedValue<string>("mn")}') as data " +
                           $"from v_option_coupon";
            
            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "v_option_coupon");
                Sconn.conn.Close();
                combobox_data.ItemsSource = ds.Tables[0].DefaultView;
                combobox_data.SelectedValuePath = ds.Tables[0].Columns["v_op_c_id"].ToString();
                combobox_data.DisplayMemberPath = ds.Tables[0].Columns["data"].ToString();
                combobox_data.SelectedIndex = 0;
                coupon_item = combobox_data.SelectedItem as DataRowView;
                if (coupon_id == null)
                {
                    coupon_id = coupon_item["v_op_c_id"].ToString();
                }
                else
                {
                    combobox_data.SelectedValue = coupon_id;
                }
                grid_g_coupon = true;
            }
            catch (MySqlException ex)
            {
                Task.Factory.StartNew(async () =>
                {
                    grid_g_coupon = false;
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
                    item_coupon.Execute(combobox_data);
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
