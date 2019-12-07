using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class GenerateCouponViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();

        public bool option_coupon_check { get; set; } = true;
        public bool grid_g_coupon { get; set; } = true;
        public bool finished { get; set; }
        public DataTable data { get; set; }
        public DataGrid grid_data { get; set; }
        public DataRowView coupon_item { get; set; }
        public string txt_create_amount { get; set; } = "1";
        public string coupon_s_date { get; set; }
        public string coupon_e_date { get; set; }
        public string coupon_c_date { get; set; }
        public string day { get; set; }
        public int row_count { get; set; }

        public int number { get; set; }
        public string coupon_username {get;set;}
        public string coupon_password { get; set; }
        public string group_id { get; set; }
        public string coupon_id { get; set; }
        public string seconds { get; set; }
        public string coupon_hr_rate { get; set; }
        public string coupon_real_amount { get; set;}
        public string coupon_free_amount { get; set; }
        public string coupon_total_amount { get; set; }
        public string coupon_time { get; set; }
        public string op_c_id { get; set; }

        public List<string> query_a = new List<string>();

        public class list
        {
            public int number { get; internal set; }
            public string coupon_username { get; internal set; }
            public string coupon_password { get; internal set; }
            public string coupon_price { get; internal set; }
            public string coupon_end_date { get; internal set; }
            public string coupon_time { get; internal set; }
            public string group_id { get; internal set; }
            public string coupon_free_money { get; internal set; }
            public string coupon_total_amount { get; internal set; }
            public string coupon_create_date { get; internal set; }
            public string coupon_start_date { get; internal set; }
            public string day { get; internal set; }
            public string op_c_id { get; internal set; }
        }
        #endregion

        #region Commands        
        public ICommand btn_save { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_option_coupon { get; set; }
        public ICommand btn_create_coupon { get; set; }
        public ICommand item_coupon { get; set; }
        public ICommand item_coupon_changed { get; set; }
        public ICommand data_coupon { get; set; }
        public ICommand data_coupon_changed { get; set; }
        #endregion

        #region Constructor
        public GenerateCouponViewModel()
        {
            item_coupon = new RelayCommand(p => GoItemCoupon(p));
            item_coupon_changed = new RelayCommand(p => GoItemCouponChanged(p));
            data_coupon = new RelayCommand(p => GoDataCoupon(p));

            btn_option_coupon = new RelayCommand(p=> 
            {
                IsClear();
                IoC.Application.DialogHostMain = false;
                DialogHost.Show(new OptionCouponView(), "Main");
            });

            btn_create_coupon = new RelayCommand(p =>
            {
                Random rnd = new Random();
                string[] coupon = new string[1];
                string[] password = new string[1];

                if (coupon_item["v_op_c_s_date"].ToString() == "true")
                {
                    coupon_c_date = coupon_s_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false));
                    day = DateTime.Now.AddDays(int.Parse(coupon_item["v_op_c_e_date"].ToString())).ToString("yyyy-MM-dd HH:mm:ss");
                    coupon_e_date = DateTime.Parse(day).ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
                    //MessageBox.Show($"ตามวันที่สร้าง");
                }
                else if (coupon_item["v_op_c_s_date"].ToString() == "false")
                {
                    coupon_s_date = null;
                    coupon_c_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false));
                    day = coupon_item["v_op_c_e_date"].ToString() + " " + GetLocalizedValue<string>("after_use"); ;
                    coupon_e_date = day;
                    //MessageBox.Show($"ตามวันที่ถูกใช้ครั้งแรก");
                }

                if (txt_create_amount == "" || txt_create_amount == null || txt_create_amount == "0")
                {
                    grid_g_coupon = false;
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("create");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    for (int n = 0; n < int.Parse(txt_create_amount); n++)
                    {
                        for (int i = 0; i < coupon.Length; i++)
                        {
                            coupon[i] = GenerateCoupon(5, rnd);
                            password[i] = GenerateCoupon(8, rnd);
                        }
                        coupon_username = coupon_item["v_op_c_name"] + String.Join(Environment.NewLine, coupon);
                        coupon_password = String.Join(Environment.NewLine, password);
                        group_id = coupon_item["v_group_id"].ToString();
                        coupon_hr_rate = coupon_item["v_group_rate"].ToString();
                        coupon_real_amount = coupon_item["v_op_c_real_amount"].ToString();
                        coupon_free_amount = coupon_item["v_op_c_free_amount"].ToString();
                        coupon_total_amount = (float.Parse(coupon_real_amount) + float.Parse(coupon_free_amount)).ToString();
                        op_c_id = coupon_item["v_op_c_id"].ToString();

                        var group_rate = (3600 / float.Parse(coupon_hr_rate)).ToString();//เรทราคา
                        seconds = (float.Parse(group_rate) * float.Parse(coupon_total_amount)).ToString();
                        coupon_time = (Math.Floor(float.Parse(seconds) / 3600)) + " " + GetLocalizedValue<string>("hr") + " " + (Math.Round((float.Parse(seconds) / 60) % 60)) + " " + GetLocalizedValue<string>("mn"); //DateTime.Parse((Math.Floor(float.Parse(seconds) / 3600)) + " : " + (Math.Round((float.Parse(seconds) / 60) % 60))).ToString("HH:mm");
                                                                                                                                                                                                                        //string.Format("{0}hr {1}mn {2}sec",(int)span.TotalHours, span.Minutes, span.Seconds);
                                                                                                                                                                                                                        //rr_amount = reader["oc_rr_amount"].ToString();
                        grid_data.Items.Add(new list
                        {
                            number = grid_data.Items.Count+1,
                            coupon_username = coupon_username,
                            coupon_password = coupon_password,
                            group_id = group_id,
                            coupon_price = coupon_real_amount,
                            coupon_free_money = coupon_free_amount,
                            coupon_total_amount = coupon_total_amount,
                            coupon_create_date = coupon_c_date,
                            coupon_start_date = coupon_s_date,
                            coupon_end_date = coupon_e_date,
                            coupon_time = coupon_time,
                            day = day,
                            op_c_id = op_c_id
                        });
                    }
                }
            });

            btn_save = new RelayCommand(p =>
            {
                grid_g_coupon = false;
                if (grid_data.Items.Count < 1)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("no_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    DialogHost.Show(new ProgressBarView(), "InMain");
                    
                    int num = 0;
                    Task.Factory.StartNew(() =>
                    {
                        for (int i = 0; i < grid_data.Items.Count; i++)
                        {
                            var row = grid_data.Items[i];
                            var item = row as list;

                            if (item.coupon_end_date != item.day)
                            {
                                var end_date = DateTime.Parse(item.coupon_end_date).ToString("yyyy-MM-dd", new CultureInfo("us-US", false)) + " " + DateTime.Now.ToString("HH:mm:ss", new CultureInfo("us-US", false));
                                item.coupon_end_date = end_date;
                            }

                            string EncryptionKey = "test123456key";
                            byte[] clearBytes = Encoding.Unicode.GetBytes(item.coupon_password);
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
                                    item.coupon_password = Convert.ToBase64String(ms.ToArray());
                                }
                            }

                            string query = $"insert into coupon set " +
                                           $"coupon_username = '{item.coupon_username}', " +
                                           $"coupon_password = '{item.coupon_password}', " +
                                           $"coupon_s_date = '{item.coupon_start_date}', " +
                                           $"coupon_e_date = '{item.coupon_end_date}', " +
                                           $"coupon_c_date = '{item.coupon_create_date}', " +
                                           $"coupon_create_by = '{IoC.LoginView.login_id}', " +
                                           $"op_c_id = '{item.op_c_id}';" +

                                           $"insert into coupon_top_up set " +
                                           $"ct_coupon_id = (select max(coupon_id) from coupon), " +
                                           $"ct_by = '{IoC.LoginView.login_id}', " +
                                           $"ct_ordinal = '1', " +
                                           $"ct_real_amount = '{item.coupon_price}', " +
                                           $"ct_free_amount = '{item.coupon_free_money}', " +
                                           $"ct_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}' ";

                            query_a.Add(query);
                        }

                    }).ContinueWith((previousTask) =>
                    {
                        if (IsInsert() == true)
                        {
                            IoC.Application.DialogHostInMain = false;
                            IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                            DialogHost.Show(new WarningView(), "Msg", SuccessClosingEventHandler);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            });

            btn_cancel = new RelayCommand(p=> 
            {
                IsClear();
                IoC.Application.DialogHostMain = false;
            });
        }
        #endregion

        #region Other method
        public bool IsInsert()
        {
            int num = 0;

            try
            {
                if (OpenConnection() == true)
                {
                    for (int i = 0; i < query_a.Count; i++)
                    {
                        MySqlCommand cmd = new MySqlCommand(query_a[i], Sconn.conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        
                        if (i == (grid_data.Items.Count - 1))
                        {
                            num = 1;
                            reader.Close();
                            Sconn.conn.Close();
                        }
                        else
                        {
                            reader.Close();
                            Sconn.conn.Close();
                            Sconn.conn.Open();
                        }
                    }

                    if (num == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

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
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                return false;
            }
        }

        private void SuccessClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
                grid_g_coupon = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
            }
        }

        private void UnsuccessClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_g_coupon = true;
            }
        }

        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_g_coupon = true;
            }
        }

        private void IsClear()
        {
            coupon_item = null;
            coupon_id = null;
            data = null;
            txt_create_amount = "1";
        }

        private void GoDataCoupon(object p)
        {
            //IsClear();
            grid_data = p as DataGrid;
        }

        public static string GenerateCoupon(int length, Random random)
        {
            string characters = "9876543210ABCDEFGHIJKLMNOPQRSTUVWXYZ9876543210ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        private void GoItemCouponChanged(object p)
        {
            var item = p as ComboBox;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_id = coupon_item["v_op_c_id"].ToString();
        }

        private void GoItemCoupon(object p)
        {
            var item = p as ComboBox;
            string query = $"select *, concat(v_op_c_name, ', ', '{GetLocalizedValue<string>("price")}',' ', " +
                           $"v_op_c_real_amount,' ','{GetLocalizedValue<string>("baht")}',', ', " +
                           $"'{GetLocalizedValue<string>("time")}',' ',v_all_hr,' ','{GetLocalizedValue<string>("hr")}',' ',v_all_mn,' ','{GetLocalizedValue<string>("mn")}') as data " +
                           $"from v_option_coupon";
            
            try
            {
                if (OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds, "v_option_coupon");
                    item.ItemsSource = ds.Tables[0].DefaultView;
                    item.SelectedValuePath = ds.Tables[0].Columns["v_op_c_id"].ToString();
                    item.DisplayMemberPath = ds.Tables[0].Columns["data"].ToString();
                    item.SelectedIndex = 0;
                    coupon_item = item.SelectedItem as DataRowView;
                    if (coupon_id == null)
                    {
                        coupon_id = coupon_item["v_op_c_id"].ToString();
                    }
                    else
                    {
                        item.SelectedValue = coupon_id;
                    }
                    Sconn.conn.Close();
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
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
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
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
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
