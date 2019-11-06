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
    /// <summary>
    /// Interaction logic for DelMemberView.xaml
    /// </summary>
    public partial class DelMemberView : UserControl
    {
        public DelMemberView()
        {
            InitializeComponent();
            DataContext = IoC.DelMemberView;
        }

        private int i = 0, num = 0;
        void OnChecked(object sender, RoutedEventArgs e)
        {
            var data = sender as DataGridCell;
            var check = data.Content as CheckBox;
            //MessageBox.Show($"{check.IsChecked}");

            if (IoC.DelMemberView.select_check == true)
            {
                for (i = 0; i < member_data.Items.Count; i++)
                {
                    var firstCol = member_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    var list = member_data.Items[i];
                    var chBx = firstCol.GetCellContent(list) as CheckBox;

                    if (chBx.IsChecked == true)
                    {
                        num++;
                    }
                }

                if (num == member_data.Items.Count)
                {
                    IoC.DelMemberView.select_all = true;
                }
                else
                {
                    IoC.DelMemberView.select_all = false;
                }
                num = 0;
            }
                
        }

        void OnUnChecked(object sender, RoutedEventArgs e)
        {
            var data = sender as DataGridCell;
            var check = data.Content as CheckBox;
            //MessageBox.Show($"{check.IsChecked}");
            if (IoC.DelMemberView.select_check == true)
            {
                for (i = 0; i < member_data.Items.Count; i++)
                {
                    var firstCol = member_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    var list = member_data.Items[i];
                    var chBx = firstCol.GetCellContent(list) as CheckBox;

                    if (chBx.IsChecked == true)
                    {
                        num++;
                    }
                }

                if (num == member_data.Items.Count)
                {
                    IoC.DelMemberView.select_all = true;
                }
                else
                {
                    IoC.DelMemberView.select_all = false;
                }
                num = 0;
            }
                
        }
    }
}
