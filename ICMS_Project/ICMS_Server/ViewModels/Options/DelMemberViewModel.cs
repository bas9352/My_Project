using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class DelMemberViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public DataGrid member_data { get; set; }
        public DataRowView member_item { get; set; }
        public int member_index { get; set; }

        public bool select_all { get; set; } = false;
        public bool select_check { get; set; } = false;

        public bool cb_max_balance { get; set; } = true;
        public bool cb_start_date { get; set; } = true;
        public bool cb_end_date { get; set; } = true;
        public bool cb_debt { get; set; } = true;
        public bool cb_exp_start_date { get; set; } = true;
        public bool cb_exp_end_date { get; set; } = true;
        public string txt_max_balance { get; set; } = null;
        public string txt_start_date { get; set; } = null;
        public string txt_end_date { get; set; } = null;
        public string txt_debt { get; set; } = null;
        public string txt_exp_start_date { get; set; } = null;
        public string txt_exp_end_date { get; set; } = null;

        private int conn_number;
        private Timer timer;
        public ArrayList ar { get; set; }

        #endregion
        #region Commands
        public ICommand btn_del { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_search { get; set; }
        public ICommand item_member_changed { get; set; }
        public ICommand btn_select_all { get; set; }
        public ICommand btn_select_changed { get; set; }
        #endregion
        #region Constructor
        public DelMemberViewModel()
        {
            btn_search = new RelayCommand(p => GoMemberItem(p));

            btn_select_all = new RelayCommand(p =>
            {
                if (member_data != null)
                {

                    var firstCol = member_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);

                    if (select_all == true)
                    {
                        foreach (var list in member_data.Items)
                        {
                            var chBx = firstCol.GetCellContent(list) as CheckBox;
                            if (chBx == null)
                            {
                                continue;
                            }
                            chBx.IsChecked = true;
                        }
                    }
                    else if (select_all == false)
                    {
                        //timer.Enabled = false;

                        foreach (var list in member_data.Items)
                        {
                            var chBx = firstCol.GetCellContent(list) as CheckBox;
                            if (chBx == null)
                            {
                                continue;
                            }
                            chBx.IsChecked = false;
                        }
                    }

                    select_check = true;
                }
            });

            btn_del = new RelayCommand(p =>
            {
                if (member_data != null)
                {
                    int num = 0, i = 0;
                    ar = new ArrayList();
                    for (i = 0; i < member_data.Items.Count; i++)
                    {
                        var firstCol = member_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                        var list = member_data.Items[i];
                        var chBx = firstCol.GetCellContent(list) as CheckBox;

                        if (chBx.IsChecked == false)
                        {
                            num++;
                        }
                        else
                        {
                            ar.Add(i);
                            //index[n] = i;
                            //n++;
                            //MessageBox.Show($"{ar[i]}");
                        }
                    }

                    if (num == member_data.Items.Count)
                    {
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                        DialogHost.Show(new WarningView(), "Msg");
                    }
                    else
                    {
                        IoC.ConfirmView.msg_title = GetLocalizedValue<string>("title_confirm");
                        IoC.ConfirmView.msg_text = GetLocalizedValue<string>("del_confirm");
                        DialogHost.Show(new ConfirmView(), "Msg", ExtendedClosingEventHandler);

                    }
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("no_data");
                    DialogHost.Show(new WarningView(), "Msg");


                }
            });

            btn_cancel = new RelayCommand(p =>
            {
                IsClear();
                IoC.Application.CurrPage = ApplicationPage.Reset;
                IoC.Application.CurrPage = ApplicationPage.Main;
            });
        }

        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            int num = 0;
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                    if (IsDelete() == true)
                    {
                        num = 1;

                    }
                    else
                    {
                        num = 0;
                    }
                }).ContinueWith((previousTask) => {
                    if (num == 1)
                    {
                        btn_search.Execute(member_data);
                        IoC.Application.DialogHostMsg = false;
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("del_success");
                        DialogHost.Show(new WarningView(), "Msg");
                        //IoC.OptionView.CurrPage = ApplicationPage.Reset;
                        //IoC.OptionView.CurrPage = ApplicationPage.DelCoupon;
                        //IsClear();
                        //IsSelect();
                    }
                    else
                    {
                        if (conn_number == 0)
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
                            IoC.WarningView.msg_text = GetLocalizedValue<string>("del_false");
                            DialogHost.Show(new WarningView(), "Msg");
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
        }

        private bool IsDelete()
        {
            int i;


            string query = $"select * from member";
            //MessageBox.Show($"{data}");
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
                    for (i = 0; i < ar.Count; i++)
                    {
                        var index = (int)ar[i];

                        DataRow dr = dt.Rows[index];
                        dr.Delete();

                    }
                    adp.Update(dt);
                    //btn_search = new RelayCommand(p => GoCouponItem(p));
                    Sconn.conn.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    conn_number = ex.Number;
                    //MessageBox.Show(ex.ToString());
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

        private string query;
        private string start_date, end_date, exp_start_date, exp_end_date;
        private object param;
        public void GoMemberItem(object p)
        {
            select_all = false;
            //MessageBox.Show($"{IoC.LoginView.login_data["staff_id"]}");
            //IsClear();
            //var item = p as DataGrid;
            member_data = p as DataGrid;

            if (txt_start_date == null || txt_start_date == "")
            {
                start_date = "";
            }
            else
            {
                start_date = DateTime.Parse(txt_start_date).ToString("yyyy-MM-dd");
            }

            if (txt_end_date == null || txt_end_date == "")
            {
                end_date = "";
            }
            else
            {
                end_date = DateTime.Parse(txt_end_date).ToString("yyyy-MM-dd");
            }

            if (txt_exp_start_date == null || txt_exp_start_date == "")
            {
                exp_start_date = "";
            }
            else
            {
                exp_start_date = DateTime.Parse(txt_exp_start_date).ToString("yyyy-MM-dd");
            }

            if (txt_exp_end_date == null || txt_exp_end_date == "")
            {
                exp_end_date = "";
            }
            else
            {
                exp_end_date = DateTime.Parse(txt_exp_end_date).ToString("yyyy-MM-dd");
            }
            query = $"select * " +
                        $"from v_member " +
                        //$"where v_coupon_username = if('{txt_coupon_name_s}' = '{""}' , v_coupon_username ,'{txt_coupon_name_s}') " +
                        $"where if('{txt_max_balance}' = '{""}' or '{cb_max_balance}' = 'False', v_member_total_remaining_amount = v_member_total_remaining_amount, v_member_total_remaining_amount <= '{txt_max_balance}') and " +
                                $"if('{start_date}' = '{""}' or '{cb_start_date}' = 'False', v_member_c_date = v_member_c_date, v_member_c_date between '{start_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                $"if('{end_date}' = '{""}' or '{cb_end_date}' = 'False', v_member_c_date = v_member_c_date, v_member_c_date between '1999-01-01%' and '{end_date}%') and " +
                                $"if('{txt_debt}' = '{""}' or '{cb_debt}' = 'False', v_member_total_debt_remaining_amount = v_member_total_debt_remaining_amount, v_member_total_debt_remaining_amount <= '{txt_debt}') and " +
                                $"if('{exp_start_date}' = '{""}' or '{cb_exp_start_date}' = 'False', v_member_e_date = v_member_e_date, v_member_e_date between '{exp_start_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                $"if('{exp_end_date}' = '{""}' or '{cb_exp_end_date}' = 'False', v_member_e_date = v_member_e_date, v_member_e_date between '1999-01-01%' and '{exp_end_date}%') " +
                        $"order by v_member_username";
            //MessageBox.Show($"{query}");


            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    member_data.ItemsSource = dt.DefaultView;
                    Sconn.conn.Close();
                }
                catch (MySqlException ex)
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

        private bool OpenConnection()
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
                        MessageBox.Show("ไม่มีการเชื่อมต่อ");
                        break;
                    case 1045:
                        MessageBox.Show("เชื่อมต่อสำเร็จ");
                        break;
                }
                return false;
            }
        }

        public void IsClear()
        {
            if (member_data != null)
            {

                member_data.ItemsSource = null;
                member_data = null;
            }
            else
            {
                member_data = null;
            }
            select_all = false;
            cb_debt = true;
            cb_start_date = true;
            cb_end_date = true;
            cb_max_balance = true;
            cb_exp_start_date = true;
            cb_exp_end_date = true;
            txt_debt = null;
            txt_start_date = null;
            txt_end_date = null;
            txt_max_balance = null;
            txt_exp_start_date = null;
            txt_exp_end_date = null;
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
