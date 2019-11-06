using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddEditGroupView.xaml
    /// </summary>
    public partial class AddEditGroupView : UserControl
    {
        Database Sconn = new Database();
        public AddEditGroupView()
        {
            InitializeComponent();
            DataContext = IoC.AddEditGroupView;
            //BindingComboBox();
        }

        //public void BindingComboBox()
        //{
        //    //type_item = p as ComboBox;
        //    string query = $"select * from type where not type_name = 'admin' and not type_name = 'staff' order by type_id";

        //    if (OpenConnection() == true)
        //    {
        //        try
        //        {
        //            MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
        //            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            adp.Fill(ds, "type");
        //            item_type.ItemsSource = ds.Tables[0].DefaultView;
        //            item_type.SelectedValuePath = ds.Tables[0].Columns["type_id"].ToString();
        //            item_type.DisplayMemberPath = ds.Tables[0].Columns["type_name"].ToString();
        //            item_type.SelectedItem = 0;
        //            Sconn.conn.Close();
        //        }
        //        catch (MySqlException ex)
        //        {
        //            MessageBox.Show(ex.ToString());
        //            Sconn.conn.Close();
        //        }
        //        finally
        //        {
        //            Sconn.conn.Close();
        //        }
        //    }
        //    else
        //    {
        //        Sconn.conn.Close();
        //    }
        //}

        //public bool OpenConnection()
        //{
        //    try
        //    {
        //        Sconn.conn.Open();
        //        return true;
        //    }
        //    catch (MySqlException ex)
        //    {
        //        switch (ex.Number)
        //        {
        //            case 0:
        //                MessageBox.Show("ไม่มีการเชื่อมต่อ");
        //                break;
        //            case 1045:
        //                MessageBox.Show("เชื่อมต่อสำเร็จ");
        //                break;
        //        }
        //        return false;
        //    }
        //}

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }
        public static bool IsTextNumeric(string str)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void NumericTextBoxInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }
    }
}
