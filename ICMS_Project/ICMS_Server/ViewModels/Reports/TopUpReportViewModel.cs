using CrystalDecisions.CrystalReports.Engine;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public class TopUpReportViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public string txt_s_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_e_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public ComboBox staff_data { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        public string staff_item { get; set; }
        public List<TopUpReportClass> top_up_report_class = new List<TopUpReportClass>();
        #endregion

        #region Commands
        public ICommand item_staff { get; set; }
        public ICommand item_staff_changed { get; set; }
        public ICommand btn_open_report { get; set; }

        #endregion

        #region Constructor
        public TopUpReportViewModel()
        {
            item_staff = new RelayCommand(p => GoItemStaff(p));
            item_staff_changed = new RelayCommand(p => GoItemStaffChanged(p));
            btn_open_report = new RelayCommand(p =>
            {
                string query = null;
                string s_date = null;
                string e_date = null;
                if (string.IsNullOrEmpty(txt_s_date) == false)
                {
                    s_date = DateTime.Parse(txt_s_date).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(txt_e_date) == false)
                {
                    e_date = DateTime.Parse(txt_e_date).ToString("yyyy-MM-dd");
                }
                if (staff_item == "all")
                {
                    query = $"select * " +
                            $"from r_all_top_up " +
                            $"where if('{txt_s_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '{s_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '1999-01-01%' and '{e_date}%') " +
                                    $"order by r_top_up_date";
                }
                else if (staff_item == "admin")
                {
                    query = $"select * " +
                            $"from r_all_top_up " +
                            $"where if('{txt_s_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '{s_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '1999-01-01%' and '{e_date}%') and " +
                                    $"r_staff_type_name = 'admin' " +
                                    $"order by r_top_up_date";
                }
                else if (staff_item == "staff")
                {
                    query = $"select * " +
                            $"from r_all_top_up " +
                            $"where if('{txt_s_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '{s_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '1999-01-01%' and '{e_date}%') and " +
                                    $"r_staff_type_name = 'staff' " +
                                    $"order by r_top_up_date";
                }
                else if (staff_item == "member")
                {
                    query = $"select * " +
                            $"from r_all_top_up " +
                            $"where if('{txt_s_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '{s_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                    $"if('{txt_e_date}' = '{""}', r_top_up_date = r_top_up_date, r_top_up_date between '1999-01-01%' and '{e_date}%') and " +
                                    $"r_staff_type_name = 'member' " +
                                    $"order by r_top_up_date";
                }

                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    Sconn.conn.Close();
                    top_up_report_class = (from row in dt.AsEnumerable()
                                           select new TopUpReportClass
                                           {
                                               r_mt_member_id = row.Field<int?>("r_mt_member_id").ToString(),
                                               r_ct_coupon_id = row.Field<int?>("r_ct_coupon_id").ToString(),
                                               r_all_username = row.Field<string>("r_all_username"),
                                               r_all_type_name = row.Field<string>("r_all_type_name"),
                                               r_top_up_by = row.Field<int?>("r_top_up_by").ToString(),
                                               r_staff_username = row.Field<string>("r_staff_username"),
                                               r_staff_type_name = row.Field<string>("r_staff_type_name"),
                                               r_top_up_real_amount = row.Field<double?>("r_top_up_real_amount").ToString(),
                                               r_top_up_free_amount = row.Field<double?>("r_top_up_free_amount").ToString(),
                                               r_mt_debt_amount = row.Field<double?>("r_mt_debt_amount").ToString(),
                                               r_mt_pay_debt = row.Field<double?>("r_mt_pay_debt").ToString(),
                                               r_mt_bonus_id = row.Field<int?>("r_mt_bonus_id").ToString(),
                                               r_bonus_point = row.Field<double?>("r_bonus_point").ToString(),
                                               r_mt_promotion_id = row.Field<int?>("r_mt_promotion_id").ToString(),
                                               r_promo_rate = row.Field<double?>("r_promo_rate").ToString(),
                                               r_top_up_date = DateTime.Parse(row.Field<string>("r_top_up_date"))
                                           }).ToList();

                    //dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                    //foreach (DataRow data in dt.Rows)
                    //{
                    //    data["new_v_all_remaining_amount"] = string.Format("{0:#,##0.##}", double.Parse(data["v_all_remaining_amount"].ToString()));
                    //}
                    //for (int i = 0; i < top_up_report_class.Count; i++)
                    //{
                    //    MessageBox.Show($"{top_up_report_class[i].r_top_up_date}");
                    //}
                    IoC.ReportViewer.rpt = new ReportDocument();
                    IoC.ReportViewer.rpt.ReportClientDocument.LocaleID = CrystalDecisions.ReportAppServer.DataDefModel.CeLocale.ceLocaleEnglishUS;
                    IoC.ReportViewer.rpt.Load("C:\\Users\\ธรณ์ธันย์\\Desktop\\My_Project\\ICMS_Project\\ICMS_Server\\ViewModels\\Reports\\CrystalReports\\TopUpReport.rpt");

                    IoC.ReportViewer.rpt.SetDataSource(dt);
                    IoC.ReportViewer.rpt.SetParameterValue("top_up_s_date", txt_s_date);
                    IoC.ReportViewer.rpt.SetParameterValue("top_up_e_date", txt_e_date);
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
        private void GoItemStaff(object p)
        {
            staff_data = p as ComboBox;

            cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
            {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("all"), "all"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("admin"), "admin"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("staff"), "staff"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("member"), "member")
            };

            staff_data.ItemsSource = cmb_data;
            staff_data.SelectedValuePath = "Value";
            staff_data.DisplayMemberPath = "Key";
            staff_data.SelectedIndex = 0;
            if (staff_item == null)
            {
                staff_item = ((KeyValuePair<string, string>)staff_data.SelectedItem).Value;
            }
        }

        private void GoItemStaffChanged(object p)
        {
            staff_data = p as ComboBox;
            if (staff_data.ItemsSource != null)
            {
                staff_item = ((KeyValuePair<string, string>)staff_data.SelectedItem).Value;
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
