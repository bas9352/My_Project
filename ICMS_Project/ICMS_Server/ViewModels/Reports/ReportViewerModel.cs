using CrystalDecisions.CrystalReports.Engine;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Collections.Generic;
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
    public class ReportViewerModel : BaseView
    {
        #region Properties
        public CrystalReportsViewer report_data { get; set; }
        public ReportViewer report_viewer { get; set; }
        public ReportDocument rpt { get; set; }

        #endregion

        #region Commands        
        public ICommand item_report { get; set; }
        #endregion

        #region Constructor
        public ReportViewerModel()
        {
            item_report = new RelayCommand(p =>GoItemReport(p));
        }        
        #endregion

        #region Other method
        private void GoItemReport(object p)
        {
            var item = p as CrystalReportsViewer;
            report_data = item;
            //report_viewer = new ReportViewer();
            //report_viewer.item_report.ViewerCore.ReportSource = rpt;
            //report_data.ViewerCore.ReportSource = rpt;
        }
        #endregion
    }
}
