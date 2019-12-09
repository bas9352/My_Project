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
using System.Windows.Input;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class OptionViewModel : BaseView
    {
        #region Properties

        public Window MainApp { get; set; }
        public bool officer { get; set; } = true;
        public bool control { get; set; } = false;
        public bool pricing { get; set; } = false;
        public bool additional { get; set; } = false;

        public bool admin_check { get; set; } = true;
        public bool user_group_check { get; set; } = true;
        public bool promotion_check { get; set; } = true;


        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostOpen { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Admin;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        #endregion

        #region Commands
        public ICommand btn_officer { get; set; }
        public ICommand btn_admin { get; set; }
        public ICommand btn_staff { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }
        public ICommand btn_control { get; set; }
        public ICommand btn_log_history { get; set; }
        public ICommand btn_database { get; set; }
        public ICommand btn_user_group { get; set; }
        public ICommand btn_promotion { get; set; }
        public ICommand btn_additional { get; set; }
        public ICommand btn_del_member { get; set; }
        public ICommand btn_del_coupon { get; set; }



        public ICommand ToggleBaseCommand { get; set; }
        public ICommand btn_switchMode { get; set; }
        public ICommand btn_ViewHelp { get; set; }
        public ICommand btn_About { get; set; }
        public ICommand btn_setting { get; set; }
        public ICommand btn_sendFeedBack { get; set; }
        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public OptionViewModel()
        {
            btn_officer = new RelayCommand(p =>
            {
                if (IoC.LoginView.login_data["type_name"].ToString() == "admin")
                {
                    officer = true;
                    control = false;
                    additional = false;
                    CurrPage = ApplicationPage.Admin;
                }
                else
                {
                    officer = true;
                    control = false;
                    additional = false;
                    CurrPage = ApplicationPage.Staff;
                }                
            });

            btn_promotion = new RelayCommand(p =>
            {
                officer = false;
                control = false;
                additional = false;
                CurrPage = ApplicationPage.Promotion;
            });

            btn_additional = new RelayCommand(p =>
            {
                additional = true;
                officer = false;
                control = false;
                CurrPage = ApplicationPage.DelMember;
            });

            btn_admin = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.Admin;
            });

            btn_staff = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.Staff;
            });

            btn_database = new RelayCommand(p =>
            {
                officer = false;
                control = false;
                additional = false;
                CurrPage = ApplicationPage.Database;
            });

            btn_user_group = new RelayCommand(p =>
            {
                officer = false;
                control = false;
                additional = false;
                CurrPage = ApplicationPage.UserGroup;
            });

            btn_promotion = new RelayCommand(p =>
            {
                officer = false;
                control = false;
                additional = false;
                CurrPage = ApplicationPage.Promotion;
            });

            btn_del_member = new RelayCommand(p =>
            {
                IoC.DelMemberView.IsClear();
                CurrPage = ApplicationPage.DelMember;
            });

            btn_del_coupon = new RelayCommand(p =>
            {
                IoC.DelCouponView.IsClear();
                CurrPage = ApplicationPage.DelCoupon;
            });
        }
        #endregion
    }
}
