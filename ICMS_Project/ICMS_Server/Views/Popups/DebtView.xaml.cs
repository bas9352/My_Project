using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// Interaction logic for DebtView.xaml
    /// </summary>
    public partial class DebtView : UserControl
    {
        private DataRowView member_item;

        public DebtView()
        {
            InitializeComponent();
            DataContext = IoC.DebtView;
        }
    }
}
