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
    public class ReportViewModel
    {
        #region Properties
        public Window MainApp { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.MemberReport;
        Database Sconn = new Database();
        public DataRowView member_item { get; set; }
        public DataTable data_mt { get; set; }
        public DataTable data_online { get; set; }
        public DataGrid member_data { get; set; }
        public DataRow mt_data { get; set; }
        public DataRow online_data { get; set; }
        public int member_index { get; set; }

        #endregion

        #region Commands
        public ICommand btn_member_report { get; set; }
        public ICommand item_member_change { get; set; }

        #endregion

        #region Constructor
        public ReportViewModel()
        {
            btn_member_report = new RelayCommand(p=>
            {
                CurrPage = ApplicationPage.MemberReport;
            });
        }
        #endregion
    }
}
