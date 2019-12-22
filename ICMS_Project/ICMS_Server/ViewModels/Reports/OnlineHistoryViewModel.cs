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
    public class OnlineHistoryReportViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public string txt_s_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public string txt_e_date { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
        public ComboBox type_data { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        public string type_item { get; set; }
        public List<OnlineHistoryReportClass> online_history_report_class = new List<OnlineHistoryReportClass>();
        #endregion

        #region Commands
        public ICommand item_type { get; set; }
        public ICommand item_type_changed { get; set; }
        public ICommand btn_open_report { get; set; }
        #endregion

        #region Constructor
        public OnlineHistoryReportViewModel()
        {
            item_type = new RelayCommand(p => GoItemIncome(p));
            item_type_changed = new RelayCommand(p => GoItemIncomeChanged(p));
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

                query = $"select * " +
                        $"from r_online_history " +
                        $"where if('{txt_s_date}' = '{""}', r_online_s_date = r_online_s_date, r_online_s_date between '{s_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                        $"if('{txt_e_date}' = '{""}', r_online_s_date = r_online_s_date, r_online_s_date between '1999-01-01%' and '{e_date}%')";

                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    Sconn.conn.Close();
                    online_history_report_class = (from row in dt.AsEnumerable()
                                           select new OnlineHistoryReportClass
                                           {
                                               r_online_id = row.Field<int?>("r_online_id").ToString(),
                                               r_member_id = row.Field<int?>("r_member_id").ToString(),
                                               r_coupon_id = row.Field<int?>("r_coupon_id").ToString(),
                                               r_all_username = row.Field<string>("r_all_username"),
                                               r_online_pc_id = row.Field<int?>("r_online_pc_id").ToString(),
                                               r_online_pc_name = row.Field<string>("r_online_pc_name"),
                                               r_online_status = row.Field<string>("r_online_status"),
                                               r_online_ordinal = row.Field<int?>("r_online_ordinal").ToString(),
                                               r_online_s_date = row.Field<string>("r_online_s_date"),
                                               r_online_s_time = row.Field<string>("r_online_s_time"),
                                               r_online_e_date = row.Field<string>("r_online_e_date"),
                                               r_online_e_time = row.Field<string>("r_online_e_time"),
                                               r_online_total_use_amount = row.Field<double?>("r_online_total_use_amount").ToString(),
                                               r_all_online_hr = row.Field<double?>("r_all_online_hr").ToString(),
                                               r_all_online_mn = row.Field<double?>("r_all_online_mn").ToString()
                                           }).ToList();

                    //dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                    //foreach (DataRow data in dt.Rows)
                    //{
                    //    data["new_v_all_remaining_amount"] = string.Format("{0:#,##0.##}", double.Parse(data["v_all_remaining_amount"].ToString()));
                    //}

                    IoC.ReportViewer.rpt = new ReportDocument();
                    IoC.ReportViewer.rpt.ReportClientDocument.LocaleID = CrystalDecisions.ReportAppServer.DataDefModel.CeLocale.ceLocaleEnglishUS;
                    IoC.ReportViewer.rpt.Load("C:\\Users\\ธรณ์ธันย์\\Desktop\\My_Project\\ICMS_Project\\ICMS_Server\\ViewModels\\Reports\\CrystalReports\\OnlineHistoryReport.rpt");

                    IoC.ReportViewer.rpt.SetDataSource(dt);
                    IoC.ReportViewer.rpt.SetParameterValue("s_date", txt_s_date);
                    IoC.ReportViewer.rpt.SetParameterValue("e_date", txt_e_date);
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
            type_data = p as ComboBox;

            cmb_data = new ObservableCollection<KeyValuePair<string, string>>()
            {
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("all"), "all"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("member"), "member"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("months"), "months"),
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("years"), "years")
            };

            type_data.ItemsSource = cmb_data;
            type_data.SelectedValuePath = "Value";
            type_data.DisplayMemberPath = "Key";
            type_data.SelectedIndex = 0;
            if (type_item == null)
            {
                type_item = ((KeyValuePair<string, string>)type_data.SelectedItem).Value;
            }
        }

        private void GoItemIncomeChanged(object p)
        {
            type_data = p as ComboBox;
            if (type_data.ItemsSource != null)
            {
                type_item = ((KeyValuePair<string, string>)type_data.SelectedItem).Value;
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
