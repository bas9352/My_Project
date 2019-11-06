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
    /// Interaction logic for DelCouponView.xaml
    /// </summary>
    public partial class DelCouponView : UserControl
    {
        public DelCouponView()
        {
            InitializeComponent();
            DataContext = IoC.DelCouponView;
        }

        private bool first_c ;
        private int i = 0, num = 0;
        void OnChecked(object sender, RoutedEventArgs e)
        {
            var data = sender as DataGridCell;
            var check = data.Content as CheckBox;
            //MessageBox.Show($"{check.IsChecked}");
            if (IoC.DelCouponView.select_check == true)
            {
                for (i = 0; i < coupon_data.Items.Count; i++)
                {
                    var firstCol = coupon_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    var list = coupon_data.Items[i];
                    var chBx = firstCol.GetCellContent(list) as CheckBox;
                    
                    if (chBx.IsChecked == true)
                    {
                        num++;
                    }

                }

                if (num == coupon_data.Items.Count)
                {
                    IoC.DelCouponView.select_all = true;
                }
                else
                {
                    IoC.DelCouponView.select_all = false;
                }
                num = 0;
            }
            
        }

        void OnUnChecked(object sender, RoutedEventArgs e)
        {
            var data = sender as DataGridCell;
            var check = data.Content as CheckBox;
            //MessageBox.Show($"{check.IsChecked}");
            if (IoC.DelCouponView.select_check == true)
            {
                for (i = 0; i < coupon_data.Items.Count; i++)
                {
                    var firstCol = coupon_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    var list = coupon_data.Items[i];
                    var chBx = firstCol.GetCellContent(list) as CheckBox;

                    if (chBx.IsChecked == true)
                    {
                        num++;
                    }
                }

                if (num == coupon_data.Items.Count)
                {
                    IoC.DelCouponView.select_all = true;
                }
                else
                {
                    IoC.DelCouponView.select_all = false;
                }
                num = 0;
            }
                
        }

        //public class ViewModel : INotifyPropertyChanged
        //{
        //    public List<Entry> Entries
        //    {
        //        get => _entries;
        //        set
        //        {
        //            if (Equals(value, _entries)) return;
        //            _entries = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public ViewModel()
        //    {
        //        // Just some demo data
        //        Entries = new List<Entry>
        //{
        //    new Entry(),
        //    new Entry(),
        //    new Entry(),
        //    new Entry()
        //};

        //        // Make sure to listen to changes. 
        //        // If you add/remove items, don't forgat to add/remove the event handlers too
        //        foreach (Entry entry in Entries)
        //        {
        //            entry.PropertyChanged += EntryOnPropertyChanged;
        //        }
        //    }

        //    private void EntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        //    {
        //        // Only re-check if the IsChecked property changed
        //        if (args.PropertyName == nameof(Entry.IsChecked))
        //            RecheckAllSelected();
        //    }

        //    private void AllSelectedChanged()
        //    {
        //        // Has this change been caused by some other change?
        //        // return so we don't mess things up
        //        if (_allSelectedChanging) return;

        //        try
        //        {
        //            _allSelectedChanging = true;

        //            // this can of course be simplified
        //            if (AllSelected == true)
        //            {
        //                foreach (Entry kommune in Entries)
        //                    kommune.IsChecked = true;
        //            }
        //            else if (AllSelected == false)
        //            {
        //                foreach (Entry kommune in Entries)
        //                    kommune.IsChecked = false;
        //            }
        //        }
        //        finally
        //        {
        //            _allSelectedChanging = false;
        //        }
        //    }

        //    private void RecheckAllSelected()
        //    {
        //        // Has this change been caused by some other change?
        //        // return so we don't mess things up
        //        if (_allSelectedChanging) return;

        //        try
        //        {
        //            _allSelectedChanging = true;

        //            if (Entries.All(e => e.IsChecked))
        //                AllSelected = true;
        //            else if (Entries.All(e => !e.IsChecked))
        //                AllSelected = false;
        //            else
        //                AllSelected = null;
        //        }
        //        finally
        //        {
        //            _allSelectedChanging = false;
        //        }
        //    }

        //    public bool? AllSelected
        //    {
        //        get => _allSelected;
        //        set
        //        {
        //            if (value == _allSelected) return;
        //            _allSelected = value;

        //            // Set all other CheckBoxes
        //            AllSelectedChanged();
        //            OnPropertyChanged();
        //        }
        //    }

        //    private bool _allSelectedChanging;
        //    private List<Entry> _entries;
        //    private bool? _allSelected;
        //    }
        //}
        //private void FieldDataGridChecked(object sender, RoutedEventArgs e)
        //{
        //    foreach (FieldViewModel model in _fields)
        //    {
        //        model.IsChecked = true;
        //    }
        //}

        //private void FieldDataGridUnchecked(object sender, RoutedEventArgs e)
        //{
        //    foreach (FieldViewModel model in _fields)
        //    {
        //        model.IsChecked = false;
        //    }
        //}


    }
}
