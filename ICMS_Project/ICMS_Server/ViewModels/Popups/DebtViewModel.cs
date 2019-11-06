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
    public class DebtViewModel : BaseView
    {
        #region Properties
        public bool AppIsWorking { get; set; } = false;
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Lend;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        #endregion

        #region Commands
        public ICommand btn_lend { get; set; }
        public ICommand btn_pay_debt { get; set; }



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
        public DebtViewModel()
        {
            btn_lend = new RelayCommand(p=>
            {
                CurrPage = ApplicationPage.Lend;
            });

            btn_pay_debt = new RelayCommand(p =>
            {
                CurrPage = ApplicationPage.PayDebt;
            });

        }
        #endregion
    }
}
