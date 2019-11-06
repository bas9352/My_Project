using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
    public class MainViewModel : BaseView
    {
        #region Properties
        
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Control;

        //public string user_login { get; set; } = IoC.LoginView.username;

        #endregion

        #region Commands
        public ICommand login_item { get; set; }

        public ICommand btn_control { get; set; }

        public ICommand btn_member_coupon { get; set; }

        public ICommand btn_report { get; set; }

        public ICommand btn_log_history { get; set; }
        #endregion
        #region Constructor

        public MainViewModel()
        {
            login_item = new RelayCommand(p=>GoLoginItem(p));

            btn_control = new RelayCommand(p =>            
            {
                CurrPage = ApplicationPage.Control;
                //DialogHost.Show(new ControlView(), "RootDialogMain");
            });

            btn_member_coupon = new RelayCommand(p =>            
            {
                IoC.MemberCouponView.CurrPage = ApplicationPage.Member;
                CurrPage = ApplicationPage.Reset;
                CurrPage = ApplicationPage.MemberCoupon;
                //DialogHost.Show(new SettingView(), "RootDialogMain");
            });

            btn_report = new RelayCommand(p =>           
            {

                CurrPage = ApplicationPage.Report;
                //DialogHost.Show(new SettingView(), "RootDialogMain");
            });

            btn_log_history = new RelayCommand(p =>           
            {

                CurrPage = ApplicationPage.LogHistory;
                //DialogHost.Show(new SettingView(), "RootDialogMain");
            });

        }

        private void GoLoginItem(object p)
        {
            var item = p as TextBlock;
            item.Text = GetLocalizedValue<string>("login_with") +" : "+ IoC.LoginView.username;
        }
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
