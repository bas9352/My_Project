using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ICMS_Server
{
    public class AppViewModel : BaseView
    {

        #region Properties

        public Window MainApp { get; set; }
        public bool list_menu { get; set; } = false;

        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostMain { get; set; }
        public bool DialogHostInMain { get; set; }
        public bool DialogHostMsg { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Login;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        #endregion

        #region Commands
        public ICommand btn_main { get; set; }
        public ICommand btn_option { get; set; }
        public ICommand btn_logout { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor

        public AppViewModel()
        {
            btn_main = new RelayCommand(p =>
            {
                IoC.MainView.CurrPage = ApplicationPage.Control;
                CurrPage = ApplicationPage.Main;
            });

            btn_option = new RelayCommand(p =>
            {
                IoC.OptionView.btn_officer.Execute("");
                CurrPage = ApplicationPage.Option;
            });

            btn_logout = new RelayCommand(p =>
            {
                IoC.LoginView.IsClear();
                list_menu = false;
                CurrPage = ApplicationPage.Login;                
            });

        }

        #endregion

        #region Orther Mathod


        #endregion

    }
}
