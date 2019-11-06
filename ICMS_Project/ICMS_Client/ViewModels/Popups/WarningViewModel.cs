using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ICMS_Client
{
    public class WarningViewModel : BaseView
    {
        #region Properties
        public string msg_title { get; set; }
        public string msg_text { get; set; }
        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        //public ICommand old_pass { get; set; }
        //public ICommand new_pass { get; set; }
        //public ICommand confirm_pass { get; set; }

        //public ICommand DialogHostLoaded { get; set; }
        //public ICommand WindowLoadedCommand { get; set; }
        //public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public WarningViewModel()
        {
        }
        #endregion
    }
}
