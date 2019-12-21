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
    public class ReportViewModel : BaseView
    {
        #region Properties
        public Window MainApp { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.MemberReport;

        #endregion

        #region Commands
        public ICommand btn_member_report { get; set; }
        public ICommand btn_coupon_report { get; set; }
        public ICommand btn_top_up_report { get; set; }
        public ICommand btn_income_report { get; set; }
        public ICommand btn_online_history_report { get; set; }

        #endregion

        #region Constructor
        public ReportViewModel()
        {
            btn_member_report = new RelayCommand(p=>
            {
                CurrPage = ApplicationPage.MemberReport;
            });

            btn_coupon_report = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.CouponReport;
            });

            btn_top_up_report = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.TopUpReport;
            });

            btn_income_report = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.IncomeReport;
            });

            btn_online_history_report = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.OnlineHistoryReport;
            });
        }
        #endregion
    }
}
