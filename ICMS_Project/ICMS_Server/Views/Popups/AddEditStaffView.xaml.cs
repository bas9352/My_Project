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
    /// Interaction logic for AddEditStaffView.xaml
    /// </summary>
    public partial class AddEditStaffView : UserControl
    {
        Database Sconn = new Database();
        public AddEditStaffView()
        {
            InitializeComponent();
            DataContext = IoC.AddEditStaffView;
            password.Password = IoC.AddEditStaffView.txt_password;
            //BindingComboBox();
        }

        //public void BindingComboBox()
        //{
        //    //group_item = p as ComboBox;
        //    string query = $"select * from user_group inner join type on type.type_id = user_group.type_id where type_name like '%staff%' order by group_id";

        //    if (OpenConnection() == true)
        //    {
        //        try
        //        {
        //            MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
        //            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            adp.Fill(ds, "user_group");
        //            item_group.ItemsSource = ds.Tables[0].DefaultView;
        //            item_group.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
        //            item_group.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
        //            item_group.SelectedIndex = 0;
        //            //selectedIndex = group_item.SelectedValue.ToString();
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
