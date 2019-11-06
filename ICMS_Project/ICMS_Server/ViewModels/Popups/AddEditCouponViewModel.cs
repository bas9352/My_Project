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
    public class AddEditCouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public DateTime s_put_date { get; set; } = DateTime.Now;
        public string title { get; set; }
        public bool is_debt { get; set; } = false;
        public bool is_remaining_amount { get; set; } = false;
        public bool is_remaining_free_amount { get; set; } = false;

        public bool grid_add_edit_c_check { get; set; } = true;

        private int conn_number;

        public bool group { get; set; } = true;
        public bool create_date { get; set; } = false;
        public bool start_date { get; set; } = false;
        public bool end_date { get; set; } = false;
        public bool IsCheck { get; set; } = false;
        public DataTable data { get; set; }
        public DataRowView group_item { get; set; }


        public string coupon_id { get; set; }
        public string txt_username { get; set; } = null;
        public string txt_password;
        public string group_id { get; set; } = null;
        public string txt_c_date { get; set; } = null;
        public string txt_s_date { get; set; } = null;
        public string txt_e_date { get; set; } = null;

        public string txt_debt { get; set; }
        public string txt_total_real_amount { get; set; }
        public string txt_use_real_free_amount { get; set; }
        public string txt_remaining_real_amount { get; set; }
        public string txt_total_free_amount { get; set; }
        public string txt_remaining_free_amount { get; set; }
        public string txt_remaining_point { get; set; }

        public string use_real_amount { get; set; }
        public string use_free_amount { get; set; }


        #endregion

        #region Commands
        public ICommand btn_check_box { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand pass { get; set; }
        public ICommand item_group { get; set; }
        public ICommand item_group_change { get; set; }
        #endregion

        #region Constructor
        public AddEditCouponViewModel()
        {
            pass = new RelayCommand(p => GoPassChanged(p));
            item_group = new RelayCommand(p => GoItemGroup(p));
            item_group_change = new RelayCommand(p => GoItemGroupChanged(p));

            btn_check_box = new RelayCommand(p =>
            {
                if (IsCheck == true)
                {
                    end_date = true;
                }
                else
                {
                    end_date = false;
                }
            });
            btn_ok = new RelayCommand(p =>
            {
                grid_add_edit_c_check = false;
                if (txt_username == null || txt_password == null || txt_username == "" || txt_password == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_user_pass");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        int num = 0;
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            DataRow row = data.Rows[i];
                            if (row["coupon_username"].ToString() == txt_username && row["coupon_id"].ToString() != coupon_id)
                            {
                                num = 1;
                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                IoC.WarningView.msg_text = GetLocalizedValue<string>("coupon_name_unsuccess");
                                DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                            }
                        }

                        if (num == 0)
                        {
                            if (IsUpdate() == true)
                            {
                                IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                                IoC.WarningView.msg_text = GetLocalizedValue<string>("edit_success");
                                DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                            }
                        }
                    }
                }

            });
            btn_cancel = new RelayCommand(p =>
            {
                IoC.Application.DialogHostMain = false;
                IsClear();
            });
        }

        public void IsClear()
        {
            group = true;
            create_date = false;
            start_date = false;
            end_date = false;
            IsCheck = false;
            data = null;
            group_item = null;

            is_debt = false;
            is_remaining_amount = false;
            is_remaining_free_amount = false;


            coupon_id = null;
            txt_username = null;
            txt_password = null;
            group_id = null;
            txt_c_date = null;
            txt_s_date = null;
            txt_e_date = null;

            txt_debt = null;
            txt_total_real_amount = null;
            txt_use_real_free_amount = null;
            txt_remaining_real_amount = null;
            txt_total_free_amount = null;
            txt_remaining_free_amount = null;
            txt_remaining_point = null;

            use_real_amount = null;
            use_free_amount = null;
        }
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
                grid_add_edit_c_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_c_check = true;
            }
        }

        public void GoPassChanged(object p)
        {
            var passwordBox = p as PasswordBox;

            string EncryptionKey = "test123456key";
            byte[] clearBytes = Encoding.Unicode.GetBytes(passwordBox.Password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    txt_password = Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public void GoItemGroup(object p)
        {
            var item = p as ComboBox;
            string query = $"select * from user_group inner join type on type.type_id = user_group.type_id where type_name like '%coupon%' order by group_id";

            

            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "user_group");
                    item.ItemsSource = ds.Tables[0].DefaultView;
                    item.SelectedValuePath = ds.Tables[0].Columns["group_id"].ToString();
                    item.DisplayMemberPath = ds.Tables[0].Columns["group_name"].ToString();
                    item.SelectedIndex = 0;
                    Sconn.conn.Close();
                    group_item = item.SelectedItem as DataRowView;
                    if (group_id == null)
                    {
                        group_id = group_item["group_id"].ToString();
                    }
                    else
                    {
                        //MessageBox.Show($"{selectedItem}");
                        item.SelectedValue = group_id;
                    }
                    //selectedIndex = group_item.SelectedValue.ToString();
                    
                }
                else
                {
                    Sconn.conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
            }
        }

        public void GoItemGroupChanged(object p)
        {
            var item = p as ComboBox;
            group_item = item.SelectedItem as DataRowView;
            group_id = group_item["group_id"].ToString();
        }

        public bool IsUpdate()
        {
            if (IsCheck == false)
            {
                txt_e_date = null;
            }
            else
            {
                var e_date = DateTime.Parse(txt_e_date).ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                txt_e_date = e_date;
            }
            string query = $"update coupon set " +
                           $"coupon_username = '{txt_username}', " +
                           $"coupon_password = '{txt_password}', " +
                           $"coupon_e_date = '{txt_e_date}' " +
                           $"where coupon_id = '{coupon_id}'";
            //= $"update member set member_username='{txt_username}' , member_password='{txt_password}' , member_name='{txt_name}' , member_lastname='{txt_lastname}' , member_nickname='{txt_nickname}' , member_birthday='{txt_birthday}' , member_tel='{txt_tel}' , member_email='{txt_email}' , member_address='{txt_address}' , member_id_card='{txt_id_card}', member_e_date='{txt_e_date}',group_id ='{group_id}' where member_id = '{member_id}'";

            
            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();
                    return true;
                }
                else
                {
                    Sconn.conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                return false;
            }

        }

        public bool IsSelect()
        {
            string query = $"select * from coupon order by coupon_id;";

            
            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    data = dt;
                    Sconn.conn.Close();
                    return true;
                }
                else
                {
                    Sconn.conn.Close();
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                return false;
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
                Sconn.conn.Close();
                if (ex.Number == 0)
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    //IoC.Application.DialogHostMsg = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
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
