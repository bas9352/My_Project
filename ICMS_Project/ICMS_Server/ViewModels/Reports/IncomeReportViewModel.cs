using CrystalDecisions.CrystalReports.Engine;
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
using System.Linq;
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
    public class IncomeReportViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public string txt_s_create_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_e_create_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public ComboBox income_data { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        public string income_item { get; set; }
        public List<CouponReportClass> coupon_report_class = new List<CouponReportClass>();
        #endregion

        #region Commands
        public ICommand item_income { get; set; }
        public ICommand item_income_changed { get; set; }
        public ICommand btn_open_report { get; set; }
        #endregion

        #region Constructor
        public IncomeReportViewModel()
        {
            item_income = new RelayCommand(p => GoItemIncome(p));
            item_income_changed = new RelayCommand(p => GoItemIncomeChanged(p));
            btn_open_report = new RelayCommand(p =>
            {
                string query = null;
                string s_create_date = null;
                string e_create_date = null;
                if (string.IsNullOrEmpty(txt_s_create_date) == false)
                {
                    s_create_date = DateTime.Parse(txt_s_create_date).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(txt_e_create_date) == false)
                {
                    e_create_date = DateTime.Parse(txt_e_create_date).ToString("yyyy-MM-dd");
                }
                if (income_item == "all")
                {
                    query = $"select * " +
                            $"from r_coupon " +
                            $"where if('{txt_s_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '{s_create_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '1999-01-01%' and '{e_create_date}%')";
                }
                else if (income_item == "admin")
                {
                    query = $"select * " +
                            $"from r_coupon " +
                            $"where if('{txt_s_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '{s_create_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '1999-01-01%' and '{e_create_date}%') and " +
                                    $"r_type_name = 'admin'";
                }
                else if (income_item == "staff")
                {
                    query = $"select * " +
                            $"from r_coupon " +
                            $"where if('{txt_s_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '{s_create_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_create_date}' = '{""}', r_coupon_c_date = r_coupon_c_date, r_coupon_c_date between '1999-01-01%' and '{e_create_date}%') and " +
                                    $"r_type_name = 'staff'";
                }

                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    Sconn.conn.Close();
                    coupon_report_class = (from row in dt.AsEnumerable()
                                           select new CouponReportClass
                                           {
                                               r_coupon_id = row.Field<int>("r_coupon_id"),
                                               r_coupon_username = row.Field<string>("r_coupon_username"),
                                               r_coupon_c_date = row.Field<string>("r_coupon_c_date"),
                                               r_coupon_total_real_amount = row.Field<double>("r_coupon_total_real_amount"),
                                               r_coupon_total_free_amount = row.Field<double>("r_coupon_total_free_amount"),
                                               r_coupon_remaining_amount = row.Field<double>("r_coupon_remaining_amount"),
                                               r_coupon_create_by = row.Field<int>("r_coupon_create_by"),
                                               r_staff_username = row.Field<string>("r_staff_username"),
                                               r_group_id = row.Field<int>("r_group_id"),
                                               r_group_name = row.Field<string>("r_group_name"),
                                               r_type_id = row.Field<int>("r_type_id"),
                                               r_type_name = row.Field<string>("r_type_name")
                                           }).ToList();

                    //dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                    //foreach (DataRow data in dt.Rows)
                    //{
                    //    data["new_v_all_remaining_amount"] = string.Format("{0:#,##0.##}", double.Parse(data["v_all_remaining_amount"].ToString()));
                    //}

                    IoC.ReportViewer.rpt = new ReportDocument();
                    IoC.ReportViewer.rpt.Load("C:\\Users\\ธรณ์ธันย์\\Desktop\\My_Project\\ICMS_Project\\ICMS_Server\\ViewModels\\Reports\\CrystalReports\\CouponReport.rpt");

                    IoC.ReportViewer.rpt.SetDataSource(dt);
                    IoC.ReportViewer.rpt.SetParameterValue("create_s_date", txt_s_create_date);
                    IoC.ReportViewer.rpt.SetParameterValue("create_e_date", txt_e_create_date);
                    //มีปัญหาไม่ล้างค่า
                    IoC.ReportViewer.report_viewer = new ReportViewer();
                    IoC.ReportViewer.report_viewer.item_report.ViewerCore.ReportSource = IoC.ReportViewer.rpt;
                    IoC.ReportViewer.report_viewer.Show();
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
                }
                finally
                {
                    Sconn.conn.Close();
                }
            });
        }
        #endregion

        #region Other method
        private void GoItemIncome(object p)
        {
            income_data = p as ComboBox;

            cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
            {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("all"), "all"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("days"), "days"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("months"), "months"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("years"), "years")
            };

            income_data.ItemsSource = cmb_data;
            income_data.SelectedValuePath = "Value";
            income_data.DisplayMemberPath = "Key";
            income_data.SelectedIndex = 0;
            if (income_item == null)
            {
                income_item = ((KeyValuePair<string, string>)income_data.SelectedItem).Value;
            }
        }

        private void GoItemIncomeChanged(object p)
        {
            income_data = p as ComboBox;
            if (income_data.ItemsSource != null)
            {
                income_item = ((KeyValuePair<string, string>)income_data.SelectedItem).Value;
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
