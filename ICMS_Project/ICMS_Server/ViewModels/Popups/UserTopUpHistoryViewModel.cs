using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ICMS_Server
{
    public class UserTopUpHistoryViewModel : BaseView
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
        public UserTopUpHistoryViewModel()
        {
        }
        #endregion
    }
}
