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
    public class MemberReportViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public ComboBox staff_data { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        public string staff_item { get; set; }
        #endregion

        #region Commands
        public ICommand item_staff { get; set; }
        public ICommand item_staff_changed { get; set; }
        public ICommand btn_open_report { get; set; }
        #endregion

        #region Constructor
        public MemberReportViewModel()
        {
            item_staff = new RelayCommand(p => GoItemStaff(p));
            item_staff_changed = new RelayCommand(p => GoItemStaffChanged(p));
            btn_open_report = new RelayCommand(p =>
            {
                string query = $"select * " +
                               $"from r_member";
                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    adp.Fill(ds, "DataSetReportMember");
                    Sconn.conn.Close();
                    dt = ds.Tables[0];
                    //dt.Columns.Add("new_v_all_remaining_amount", typeof(string));
                    //foreach (DataRow data in dt.Rows)
                    //{
                    //    data["new_v_all_remaining_amount"] = string.Format("{0:#,##0.##}", double.Parse(data["v_all_remaining_amount"].ToString()));
                    //}

                    IoC.ReportViewer.rpt = new ReportDocument();
                    IoC.ReportViewer.rpt.Load("C:\\Users\\ธรณ์ธันย์\\Desktop\\My_Project\\ICMS_Project\\ICMS_Server\\ViewModels\\Reports\\CrystalReports\\MemberReport.rpt");
                    IoC.ReportViewer.rpt.SetDataSource(dt);
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
                    new KeyValuePair < string , string > (GetLocalizedValue<string>("staff"), "staff")
            };

            staff_data.ItemsSource = cmb_data;
            staff_data.SelectedValuePath = "Value";
            staff_data.DisplayMemberPath = "Key";
            staff_data.SelectedIndex = 0;
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
