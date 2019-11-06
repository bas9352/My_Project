using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Client
{
    public class ConfirmViewModel : BaseView
    {
        #region Properties
        public string msg_title { get; set; }
        public string msg_text { get; set; }
        #endregion

        #region Command
        public ICommand btn_ok { get; set; }

        public ICommand btn_cancel { get; set; }
        #endregion
        public ConfirmViewModel()
        {
        }
    }
}
