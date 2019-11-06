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
    public class OptionCouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DataRowView coupon_item { get; set; }
        public int coupon_index { get; set; }
        public bool button_add { get; set; } = false;
        public bool grid_op_c_check { get; set; } = true;

        public DataGrid grid_data { get; set; }
        public Window MainApp { get; set; }
        public bool list_menu { get; set; } = false;

        public bool AppIsWorking { get; set; } = false;

        public bool DialogHostMain { get; set; }
        public bool DialogHostInMain { get; set; }
        public bool DialogHostMsg { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Member;

        public bool ToggleCheck { get; set; }

        public string strVersion { get; set; }

        public int index { get; set; }
        public string query { get; set; }

        #endregion

        #region Commands
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }

        public ICommand item_coupon { get; set; }
        public ICommand item_coupon_changed { get; set; }
        #endregion

        #region Constructor
        public OptionCouponViewModel()
        {
            item_coupon = new RelayCommand(p=> GoItemCoupon(p));
            item_coupon_changed = new RelayCommand(p => GoItemCouponChanged(p));

            btn_add = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                IoC.AddEditOptionCouponView.op_c_id = null;
                DialogHost.Show(new AddEditOptionCouponView(), "InMain");
            });

            btn_edit = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                if (coupon_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.AddEditOptionCouponView.op_c_id = coupon_item["op_c_id"].ToString();
                    IoC.AddEditOptionCouponView.txt_username = coupon_item["op_c_name"].ToString();
                    IoC.AddEditOptionCouponView.group_id = coupon_item["group_id"].ToString();
                    IoC.AddEditOptionCouponView.txt_hr_rate = coupon_item["group_rate"].ToString();
                    //MessageBox.Show($"{coupon_item["op_c_real_amount"].ToString()}");
                    if (coupon_item["op_c_real_amount"].ToString() == "0")
                    {
                        IoC.AddEditOptionCouponView.txt_hr_price = "";
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.txt_hr_price = coupon_item["op_c_real_amount"].ToString();
                    }
                    if (coupon_item["op_c_free_amount"].ToString() == "0")
                    {
                        IoC.AddEditOptionCouponView.txt_free_money = "";
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.txt_free_money = coupon_item["op_c_free_amount"].ToString();
                    }
                    IoC.AddEditOptionCouponView.g_hr_price = coupon_item["op_c_real_amount"].ToString();
                    IoC.AddEditOptionCouponView.g_free_money = coupon_item["op_c_free_amount"].ToString();
                    IoC.AddEditOptionCouponView.txt_exp_date = coupon_item["op_c_e_date"].ToString();
                    if (coupon_item["op_c_s_date"].ToString() == "True")
                    {
                        IoC.AddEditOptionCouponView.start_create_date = true;
                    }
                    else
                    {
                        IoC.AddEditOptionCouponView.start_first_use = true;
                    }
                    //IoC.AddEditCouponView.txt_total_amount = "0";
                    IoC.AddEditOptionCouponView.IsChanged();
                    IoC.AddEditOptionCouponView.txt_total_amount = (float.Parse(IoC.AddEditOptionCouponView.g_hr_price) + float.Parse(IoC.AddEditOptionCouponView.g_free_money)).ToString();
                    IoC.AddEditOptionCouponView.seconds = ((3600 / float.Parse(IoC.AddEditOptionCouponView.txt_hr_rate)) * float.Parse(IoC.AddEditOptionCouponView.txt_total_amount)).ToString();
                    IoC.AddEditOptionCouponView.txt_add_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.AddEditOptionCouponView.seconds) / 3600));
                    IoC.AddEditOptionCouponView.txt_add_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.AddEditOptionCouponView.seconds) / 60) % 60));
                    
                    
                    //IoC.AddEditCouponView.IsChanged();
                    ////IoC.AddEditStaffView.convertpass = IoC.AddEditStaffView.txt_password = reader["staff_password"].ToString();
                    ////string EncryptionKey = "test123456key";

                    ////byte[] cipherBytes = Convert.FromBase64String(reader["staff_password"].ToString());
                    ////Aes encryptor = Aes.Create();
                    ////Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    ////encryptor.Key = pdb.GetBytes(32);
                    ////encryptor.IV = pdb.GetBytes(16);
                    ////using (MemoryStream ms = new MemoryStream())
                    ////{
                    ////    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    ////    {
                    ////        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    ////        cs.Close();
                    ////    }
                    ////    IoC.AddEditStaffView.txt_password = Encoding.Unicode.GetString(ms.ToArray());
                    ////}
                    //IoC.AddEditStaffView.txt_password = staff_item["staff_password"].ToString();
                    //IoC.AddEditStaffView.group_id = staff_item["group_id"].ToString();
                    //IoC.AddEditStaffView.txt_name = staff_item["staff_name"].ToString();
                    //IoC.AddEditStaffView.txt_lastname = staff_item["staff_lastname"].ToString();
                    //IoC.AddEditStaffView.txt_nickname = staff_item["staff_nickname"].ToString();
                    //IoC.AddEditStaffView.txt_birthday = staff_item["staff_birthday"].ToString();
                    //IoC.AddEditStaffView.txt_tel = staff_item["staff_tel"].ToString();
                    //IoC.AddEditStaffView.txt_email = staff_item["staff_email"].ToString();
                    //IoC.AddEditStaffView.txt_address = staff_item["staff_address"].ToString();
                    //IoC.AddEditStaffView.txt_id_card = staff_item["staff_id_card"].ToString();
                    //IoC.AddEditStaffView.txt_c_date = staff_item["staff_c_date"].ToString();
                    //IoC.AddEditStaffView.txt_s_date = staff_item["staff_s_date"].ToString();

                    //if (staff_item["staff_e_date"].ToString() == null || staff_item["staff_e_date"].ToString() == "")
                    //{
                    //    IoC.AddEditStaffView.IsCheck = false;
                    //    IoC.AddEditStaffView.end_date = false;
                    //    IoC.AddEditStaffView.txt_e_date = staff_item["staff_e_date"].ToString();
                    //}
                    //else
                    //{
                    //    IoC.AddEditStaffView.IsCheck = true;
                    //    IoC.AddEditStaffView.end_date = true;
                    //    IoC.AddEditStaffView.txt_e_date = staff_item["staff_e_date"].ToString();
                    //}
                    ////IsRow();
                    //IoC.AddEditStaffView.staff_id = staff_item["staff_id"].ToString();
                    ////CurrPage = ApplicationPage.Admin;
                    DialogHost.Show(new AddEditOptionCouponView(), "InMain");
                }

            });

            btn_del = new RelayCommand(p =>
            {
                grid_op_c_check = false;
                if (coupon_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                    IoC.ConfirmView.msg_text = GetLocalizedValue<string>("del_confirm");
                    DialogHost.Show(new ConfirmView(), "Msg", ExtendedClosingEventHandler);
                }
            });
            btn_ok = new RelayCommand(p=> 
            {

            });

            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new GenerateCouponView(), "Main");
            });
        }

        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                if (IsDelete() == true)
                {
                    IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("del_success");
                    DialogHost.Show(new WarningView(), "Msg");
                    IoC.Application.DialogHostMain = false;
                    DialogHost.Show(new OptionCouponView(), "Main");
                    //IoC.OptionCouponView.CurrPage = ApplicationPage.Reset;
                    //IoC.OptionView.CurrPage = ApplicationPage.Staff;
                    //IsClear();
                    //IsSelect();
                }
                else
                {
                    IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
            }
            else
            {
                grid_op_c_check = true;
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_op_c_check = true;
            }
        }

        public bool IsDelete()
        {
            string query = $"select * from option_coupon";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    MySqlCommandBuilder cmdb = new MySqlCommandBuilder(adp);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    Console.WriteLine(cmdb.GetDeleteCommand().CommandText);
                    DataRow dr = dt.Rows[coupon_index];
                    dr.Delete();
                    adp.Update(dt);
                    Sconn.conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Sconn.conn.Close();
                    return false;
                }
                finally
                {
                    Sconn.conn.Close();
                }
            }
            else
            {
                Sconn.conn.Close();
                return false;
            }
        }

        private void GoItemCouponChanged(object p)
        {
            var item = p as DataGrid;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_index = item.SelectedIndex;
        }

        private void GoItemCoupon(object p)
        {
            //IsClear();
            var item = p as DataGrid;
            string query = $"select * from option_coupon " +
                           $"inner join user_group on user_group.group_id = option_coupon.group_id " +
                           $"order by option_coupon.op_c_id";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    item.ItemsSource = dt.DefaultView;
                    grid_data = item;
                    if (grid_data.Items.Count >= 10)
                    {
                        button_add = false;
                    }
                    else
                    {
                        button_add = true;
                    }
                    Sconn.conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Sconn.conn.Close();
                }
                finally
                {
                    Sconn.conn.Close();
                }
            }
            else
            {
                Sconn.conn.Close();
                //MessageBox.Show($"{Sconn.msg_con}");
            }
        }

        public bool OpenConnection()
        {
            try
            {
                Sconn.conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show(ex.ToString());
                        break;
                    case 1045:
                        MessageBox.Show(ex.ToString());
                        break;
                }
                return false;
            }
        }
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
