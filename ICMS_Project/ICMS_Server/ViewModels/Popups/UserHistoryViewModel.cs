using System;
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
    public class UserHistoryViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public bool grid_p_check { get; set; } = true;
        public string txt_username { get; set; }
        public string txt_debt { get; set; }
        public string member_id { get; set; }
        public string txt_pay_debt { get; set; }

        private int conn_number;
        public int ordinal { get; set; }
        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        #endregion

        #region Constructor
        public UserHistoryViewModel()
        {

        }
        #endregion
    }
}
