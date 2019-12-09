using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    public class MemberCouponViewModel : BaseView
    {
        #region Properties
        Database Sconn = new Database();
        public Window MainApp { get; set; }
        public ObservableCollection<KeyValuePair<string, string>> cmb_data { get; set; }
        public ComboBox cmb { get; set; }
        public DataRowView search_item { get; set; }
        public bool list_menu { get; set; } = false;
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Member;

        public string txt_search { get; set; }
        public string txt_search_null { get; set; }
        public string search_text { get; set; }
        public int index { get; set; }
        public string query { get; set; }
        private int conn_number { get; set; }

        #endregion

        #region Commands
        public ICommand btn_member { get; set; }
        public ICommand btn_coupon { get; set; }
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }

        public ICommand btn_top_up { get; set; }
        public ICommand btn_free_top_up { get; set; }
        public ICommand btn_lend { get; set; }
        public ICommand btn_debt { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }

        public ICommand item_search { get; set; }
        public ICommand item_search_changed { get; set; }
        public ICommand txt_search_changed { get; set; }
        #endregion

        #region Constructor
        public MemberCouponViewModel()
        {
            item_search = new RelayCommand(p => GoItemSearch(p));
            item_search_changed = new RelayCommand(p => GoItemSearchChanged(p));
            txt_search_changed = new RelayCommand(p => GoTxtSearchChanged(p));

            btn_member = new RelayCommand(p =>
            {
                txt_search_null = null;
                search_text = null;
                cmb.ItemsSource = null;
                CurrPage = ApplicationPage.Reset;
                CurrPage = ApplicationPage.Member;
            });

            btn_coupon = new RelayCommand(p =>
            {
                txt_search_null = null;
                search_text = null;
                cmb.ItemsSource = null;
                CurrPage = ApplicationPage.Reset;
                CurrPage = ApplicationPage.Coupon;
            });

            btn_add = new RelayCommand(p =>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    IoC.AddEditMemberView.title = GetLocalizedValue<string>("add_member");
                    IoC.AddEditMemberView.member_id = null;

                    IoC.AddEditMemberView.member_add = true;
                    IoC.AddEditMemberView.member_edit = false;

                    DialogHost.Show(new AddEditMemberView(), "Main");
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    DialogHost.Show(new GenerateCouponView(), "Main");
                }
            });

            btn_edit = new RelayCommand(p =>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    IoC.AddEditMemberView.member_add = false;
                    IoC.AddEditMemberView.member_edit = true;

                    IoC.AddEditMemberView.is_debt = true;
                    IoC.AddEditMemberView.is_remaining_amount = true;
                    IoC.AddEditMemberView.is_remaining_free_amount = true;

                    if (IoC.MemberView.member_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        //1
                        IoC.AddEditMemberView.member_id = IoC.MemberView.member_item["v_member_id"].ToString();
                        IoC.AddEditMemberView.txt_username = IoC.MemberView.member_item["v_all_username"].ToString();
                        IoC.AddEditMemberView.txt_password = IoC.MemberView.member_item["v_all_password"].ToString();
                        IoC.AddEditMemberView.group_id = IoC.MemberView.member_item["v_all_group_id"].ToString();
                        IoC.AddEditMemberView.txt_c_date = IoC.MemberView.member_item["v_all_c_date"].ToString();
                        IoC.AddEditMemberView.txt_s_date = IoC.MemberView.member_item["v_all_s_date"].ToString();

                        if (IoC.MemberView.member_item["v_all_e_date"].ToString() == null || IoC.MemberView.member_item["v_all_e_date"].ToString() == "")
                        {
                            IoC.AddEditMemberView.IsCheck = false;
                            IoC.AddEditMemberView.end_date = false;
                            IoC.AddEditMemberView.txt_e_date = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
                        }
                        else
                        {
                            IoC.AddEditMemberView.IsCheck = true;
                            IoC.AddEditMemberView.end_date = true;
                            IoC.AddEditMemberView.txt_e_date = IoC.MemberView.member_item["v_all_e_date"].ToString();
                        }

                        //2
                        IoC.AddEditMemberView.txt_name = IoC.MemberView.member_item["v_member_name"].ToString();
                        IoC.AddEditMemberView.txt_nickname = IoC.MemberView.member_item["v_member_nickname"].ToString();
                        IoC.AddEditMemberView.txt_lastname = IoC.MemberView.member_item["v_member_lastname"].ToString();
                        IoC.AddEditMemberView.txt_birthday = IoC.MemberView.member_item["v_member_birthday"].ToString();
                        IoC.AddEditMemberView.txt_tel = IoC.MemberView.member_item["v_member_tel"].ToString();
                        IoC.AddEditMemberView.txt_email = IoC.MemberView.member_item["v_member_email"].ToString();
                        IoC.AddEditMemberView.txt_address = IoC.MemberView.member_item["v_member_address"].ToString();
                        IoC.AddEditMemberView.txt_id_card = IoC.MemberView.member_item["v_member_id_card"].ToString();

                        //3
                        IoC.AddEditMemberView.txt_debt = IoC.MemberView.member_item["v_remaining_debt_amount"].ToString();
                        IoC.AddEditMemberView.txt_total_real_amount = IoC.MemberView.member_item["v_total_real_top_up"].ToString();
                        IoC.AddEditMemberView.txt_use_real_free_amount = IoC.MemberView.member_item["v_all_use_real_amount"].ToString();
                        IoC.AddEditMemberView.txt_remaining_real_amount = IoC.MemberView.member_item["v_all_remaining_real_amount"].ToString();
                        IoC.AddEditMemberView.txt_total_free_amount = IoC.MemberView.member_item["v_total_free_top_up"].ToString();
                        IoC.AddEditMemberView.txt_remaining_free_amount = IoC.MemberView.member_item["v_all_remaining_free_amount"].ToString();
                        IoC.AddEditMemberView.txt_remaining_point = IoC.MemberView.member_item["v_remaining_bonus_amount"].ToString();

                        IoC.AddEditMemberView.title = GetLocalizedValue<string>("edit_member");
                        DialogHost.Show(new AddEditMemberView(), "Main");
                    }
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    IoC.AddEditCouponView.is_remaining_amount = true;
                    IoC.AddEditCouponView.is_remaining_free_amount = true;

                    if (IoC.CouponView.coupon_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        //1
                        IoC.AddEditCouponView.coupon_id = IoC.CouponView.coupon_item["v_coupon_id"].ToString();
                        IoC.AddEditCouponView.txt_username = IoC.CouponView.coupon_item["v_all_username"].ToString();
                        IoC.AddEditCouponView.txt_password = IoC.CouponView.coupon_item["v_all_password"].ToString();
                        IoC.AddEditCouponView.group_id = IoC.CouponView.coupon_item["v_all_group_id"].ToString();
                        IoC.AddEditCouponView.txt_c_date = IoC.CouponView.coupon_item["v_all_c_date"].ToString();
                        IoC.AddEditCouponView.txt_s_date = IoC.CouponView.coupon_item["v_all_s_date"].ToString();
                        
                                                
                        if (IoC.CouponView.coupon_item["v_all_e_date"].ToString() == null || IoC.CouponView.coupon_item["v_all_e_date"].ToString() == "")
                        {
                            IoC.AddEditCouponView.IsCheck = false;
                            IoC.AddEditCouponView.end_date = false;
                            IoC.AddEditCouponView.txt_e_date = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("us-US", false));
                        }
                        else
                        {
                            IoC.AddEditCouponView.IsCheck = true;
                            IoC.AddEditCouponView.end_date = true;
                            IoC.AddEditCouponView.txt_e_date = IoC.CouponView.coupon_item["v_all_e_date"].ToString();
                        }

                        //2
                        IoC.AddEditCouponView.txt_total_real_amount = IoC.CouponView.coupon_item["v_all_total_real_amount"].ToString();
                        IoC.AddEditCouponView.txt_use_real_free_amount = IoC.CouponView.coupon_item["v_all_use_real_amount"].ToString();
                        IoC.AddEditCouponView.txt_remaining_real_amount = IoC.CouponView.coupon_item["v_all_remaining_real_amount"].ToString();
                        IoC.AddEditCouponView.txt_total_free_amount = IoC.CouponView.coupon_item["v_all_total_free_amount"].ToString();
                        IoC.AddEditCouponView.txt_remaining_free_amount = IoC.CouponView.coupon_item["v_all_remaining_free_amount"].ToString();

                        IoC.AddEditCouponView.title = GetLocalizedValue<string>("edit_coupon");
                        DialogHost.Show(new AddEditCouponView(), "Main");
                    }
                }
            });

            btn_del = new RelayCommand(p =>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    if (IoC.MemberView.member_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                        IoC.ConfirmView.msg_text = GetLocalizedValue<string>("del_confirm");
                        DialogHost.Show(new ConfirmView(), "Msg", IsDelete);
                    }
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    if (IoC.CouponView.coupon_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                        IoC.ConfirmView.msg_text = GetLocalizedValue<string>("del_confirm");
                        DialogHost.Show(new ConfirmView(), "Msg", IsDelete);
                    }
                }
            });

            btn_top_up = new RelayCommand(p=>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    if (IoC.MemberView.member_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.TopUpView.member_id = IoC.MemberView.member_item["v_member_id"].ToString();//รหัสสมาชิก
                        IoC.TopUpView.bonus_status = IoC.MemberView.member_item["v_group_bonus_status"].ToString();//สถานะโบนัส เปิด หรือปิด
                        IoC.TopUpView.group_rate = (3600 / float.Parse(IoC.MemberView.member_item["v_all_group_rate"].ToString())).ToString();//เรทราคา
                        IoC.TopUpView.txt_username = IoC.MemberView.member_item["v_member_username"].ToString();//ชื่อผู้ใช้
                        IoC.TopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(IoC.MemberView.member_item["v_all_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.TopUpView.remaining_money = IoC.MemberView.member_item["v_all_remaining_amount"].ToString(); //รวมเงินคงเหลือ ใช้คำนวณ
                        IoC.TopUpView.seconds = (float.Parse(IoC.TopUpView.group_rate) * float.Parse(IoC.TopUpView.remaining_money)).ToString();//วินาทีจากเงินคงเหลือ
                        IoC.TopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.TopUpView.seconds) / 3600));
                        IoC.TopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.TopUpView.seconds) / 60) % 60));
                        IoC.TopUpView.ordinal = int.Parse(IoC.MemberView.member_item["v_all_ordinal_last"].ToString());
                        DialogHost.Show(new TopUpView(), "Main");
                    }
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    if (IoC.CouponView.coupon_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.TopUpView.coupon_id = IoC.CouponView.coupon_item["v_coupon_id"].ToString();
                        IoC.TopUpView.bonus_status = "false";
                        IoC.TopUpView.group_rate = (3600 / float.Parse(IoC.CouponView.coupon_item["v_group_rate"].ToString())).ToString();//เรทราคา
                        IoC.TopUpView.txt_username = IoC.CouponView.coupon_item["v_coupon_username"].ToString();
                        IoC.TopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(IoC.CouponView.coupon_item["v_coupon_total_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.TopUpView.remaining_money = IoC.CouponView.coupon_item["v_coupon_total_remaining_amount"].ToString(); //รวมเงินคงเหลือ ใช้คำนวณ
                        IoC.TopUpView.seconds = (float.Parse(IoC.TopUpView.group_rate) * float.Parse(IoC.TopUpView.remaining_money)).ToString();
                        IoC.TopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.TopUpView.seconds) / 3600));
                        IoC.TopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.TopUpView.seconds) / 60) % 60));
                        IoC.TopUpView.ordinal = int.Parse(IoC.CouponView.coupon_item["v_coupon_ordinal"].ToString());
                        DialogHost.Show(new TopUpView(), "Main");
                    }
                }
            });

            btn_free_top_up = new RelayCommand(p =>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    if (IoC.MemberView.member_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.FreeTopUpView.member_id = IoC.MemberView.member_item["v_member_id"].ToString();
                        IoC.FreeTopUpView.group_rate = (3600 / float.Parse(IoC.MemberView.member_item["v_group_rate"].ToString())).ToString();
                        IoC.FreeTopUpView.txt_username = IoC.MemberView.member_item["v_member_username"].ToString();
                        
                        IoC.FreeTopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(IoC.MemberView.member_item["v_member_total_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.FreeTopUpView.remaining_money = IoC.MemberView.member_item["v_member_total_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.FreeTopUpView.seconds = (float.Parse(IoC.FreeTopUpView.group_rate) * float.Parse(IoC.FreeTopUpView.remaining_money)).ToString();
                        IoC.FreeTopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.FreeTopUpView.seconds) / 3600));
                        IoC.FreeTopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.FreeTopUpView.seconds) / 60) % 60));
                        IoC.FreeTopUpView.ordinal = int.Parse(IoC.MemberView.member_item["v_member_ordinal"].ToString());
                        DialogHost.Show(new FreeTopUpView(), "Main");
                    }
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    if (IoC.CouponView.coupon_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.FreeTopUpView.coupon_id = IoC.CouponView.coupon_item["v_coupon_id"].ToString();
                        IoC.FreeTopUpView.group_rate = (3600 / float.Parse(IoC.CouponView.coupon_item["v_group_rate"].ToString())).ToString();
                        IoC.FreeTopUpView.txt_username = IoC.CouponView.coupon_item["v_coupon_username"].ToString();

                        IoC.FreeTopUpView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", float.Parse(IoC.CouponView.coupon_item["v_coupon_total_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.FreeTopUpView.remaining_money = IoC.CouponView.coupon_item["v_coupon_total_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.FreeTopUpView.seconds = (float.Parse(IoC.FreeTopUpView.group_rate) * float.Parse(IoC.FreeTopUpView.remaining_money)).ToString();
                        IoC.FreeTopUpView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.FreeTopUpView.seconds) / 3600));
                        IoC.FreeTopUpView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.FreeTopUpView.seconds) / 60) % 60));
                        IoC.FreeTopUpView.ordinal = int.Parse(IoC.CouponView.coupon_item["v_coupon_ordinal"].ToString());
                        DialogHost.Show(new FreeTopUpView(), "Main");
                    }
                }
            });

            btn_debt = new RelayCommand(p =>
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    if (IoC.MemberView.member_item == null)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.DebtView.CurrPage = ApplicationPage.Lend;
                        //เพิ่มหนี้
                        IoC.LendView.member_id = IoC.MemberView.member_item["v_member_id"].ToString();
                        IoC.LendView.group_rate = (3600 / float.Parse(IoC.MemberView.member_item["v_group_rate"].ToString())).ToString();
                        IoC.LendView.txt_total_remaining_amount = string.Format("{0:#,##0.##}", double.Parse(IoC.MemberView.member_item["v_member_total_remaining_amount"].ToString()));//รวมเงินคงเหลือ ใช้แสดง
                        IoC.LendView.remaining_money = IoC.MemberView.member_item["v_member_total_remaining_amount"].ToString();//เงินคงเหลือ รวมจากเงินจริงและฟรี
                        IoC.LendView.txt_credit_limit = IoC.MemberView.member_item["v_member_credit_limit"].ToString();//ยืมได้ไม่เกิน
                        IoC.LendView.txt_username = IoC.MemberView.member_item["v_member_username"].ToString();
                        IoC.LendView.txt_debt = IoC.MemberView.member_item["v_member_total_debt_remaining_amount"].ToString();
                        IoC.LendView.txt_credit_limit = IoC.MemberView.member_item["v_member_credit_limit"].ToString();
                        IoC.LendView.member_seconds = (float.Parse(IoC.LendView.group_rate) * float.Parse(IoC.LendView.remaining_money)).ToString();
                        IoC.LendView.txt_remaining_hh = string.Format("{0:0}" + " h", Math.Floor(float.Parse(IoC.LendView.member_seconds) / 3600));
                        IoC.LendView.txt_remaining_mm = string.Format("{0:0}" + " m", Math.Round((float.Parse(IoC.LendView.member_seconds) / 60) % 60));
                        IoC.LendView.ordinal = int.Parse(IoC.MemberView.member_item["v_member_ordinal"].ToString());
                        //ชำระหนี้
                        IoC.PayDebtView.member_id = IoC.MemberView.member_item["v_member_id"].ToString();
                        IoC.PayDebtView.txt_username = IoC.MemberView.member_item["v_member_username"].ToString();
                        IoC.PayDebtView.txt_debt = IoC.MemberView.member_item["v_member_total_debt_remaining_amount"].ToString();
                        IoC.PayDebtView.ordinal = int.Parse(IoC.MemberView.member_item["v_member_ordinal"].ToString());
                        DialogHost.Show(new DebtView(), "Main");
                    }
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("not_group_can_top_up");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                
            });
        }
        #endregion

        #region Orther Mathod
        private void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                if (CurrPage == ApplicationPage.Member)
                {
                    query = $"delete from member " +
                            $"where member_id = '{IoC.MemberView.member_item["v_member_id"].ToString()}' ";
                }
                else if (CurrPage == ApplicationPage.Coupon)
                {
                    query = $"delete from coupon " +
                            $"where coupon_id = '{IoC.CouponView.coupon_item["v_coupon_id"].ToString()}' ";
                }

                try
                {
                    Sconn.conn.Open();

                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    reader.Close();
                    Sconn.conn.Close();

                    if (CurrPage == ApplicationPage.Member)
                    {
                        IoC.MemberView.item_member.Execute(IoC.MemberView.member_data);
                    }
                    else if (CurrPage == ApplicationPage.Coupon)
                    {
                        IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false_in_use");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else if (ex.Number == 0)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                }
                finally
                {
                    Sconn.conn.Close();
                }
            }
        }

        public void IsClear()
        {
            if (CurrPage == ApplicationPage.Member)
            {
                IoC.MemberView.member_item = null;
                IoC.MemberView.member_index = 0;
            }
            else if (CurrPage == ApplicationPage.Coupon)
            {
                IoC.CouponView.coupon_item = null;
                IoC.CouponView.coupon_index = 0;
            }
        }
        public void GoItemSearch(object p)
        {
            var item = p as ComboBox;
            cmb = item;
        }

        public void GoItemSearchChanged(object p)
        {
            var item = p as ComboBox;
            if (item.ItemsSource != null)
            {
                search_text = ((KeyValuePair<string, string>)item.SelectedItem).Value;
            }
            
        }

        public void GoTxtSearchChanged(object p)
        {
            var item = p as TextBox;
            txt_search = item.Text;

            if (CurrPage == ApplicationPage.Member)
            {
                IoC.MemberView.item_member.Execute(IoC.MemberView.member_data);
            }
            else if(CurrPage == ApplicationPage.Coupon)
            {
                IoC.CouponView.item_coupon.Execute(IoC.CouponView.coupon_data);
            }
        }
        
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }

        #endregion
    }
}
