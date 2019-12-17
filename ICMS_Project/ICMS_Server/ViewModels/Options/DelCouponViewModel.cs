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
    public class DelCouponViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public DataGrid coupon_data { get; set; }
        public DataRowView coupon_item { get; set; }
        public int coupon_index { get; set; }

        public bool select_all { get; set; } = false;
        public bool select_check { get; set; } = false;

        public bool cb_coupon_name_s { get; set; } = true;
        public bool cb_start_date { get; set; } = true;
        public bool cb_end_date { get; set; } = true;
        public bool cb_max_balance { get; set; } = true;
        public bool cb_exp_start_date { get; set; } = true;
        public bool cb_exp_end_date { get; set; } = true;
        public string txt_coupon_name_s { get; set; } = null;
        public string txt_start_date { get; set; } = null;
        public string txt_end_date { get; set; } = null;
        public string txt_max_balance { get; set; } = null;
        public string txt_exp_start_date { get; set; } = null;
        public string txt_exp_end_date { get; set; } = null;
        public ArrayList ar { get; set; }
        #endregion

        #region Commands
        public ICommand btn_del { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand btn_search { get; set; }
        public ICommand item_coupon_changed { get; set; }
        public ICommand btn_select_all { get; set; }
        public ICommand btn_select_changed { get; set; }
        #endregion

        #region Constructor
        public DelCouponViewModel()
        {
            btn_del = new RelayCommand(p =>
            {
                if (coupon_data != null)
                {
                    int num = 0, i = 0;
                    ar = new ArrayList();
                    for ( i= 0; i < coupon_data.Items.Count; i++)
                    {
                        var firstCol = coupon_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                        var list = coupon_data.Items[i];
                        var chBx = firstCol.GetCellContent(list) as CheckBox;
                        if (chBx == null)
                        {
                            continue;
                        }
                        if (chBx.IsChecked == false)
                        {
                            num++;
                        }
                        else
                        {
                            ar.Add(i);
                        }
                    }

                    if (num == coupon_data.Items.Count)
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
                IoC.Application.btn_main.Execute("");
            });

            btn_search = new RelayCommand(p => GoCouponItem(p));
            item_coupon_changed = new RelayCommand(p => GoCouponItemChanged(p));

            btn_select_all = new RelayCommand(p =>
            {
                if (coupon_data != null)
                {

                    firstCol = coupon_data.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);

                    if (select_all == true)
                    {
                        foreach (var list in coupon_data.Items)
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
                        foreach (var list in coupon_data.Items)
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
        }
        #endregion

        #region Other method
        public DataGridCheckBoxColumn firstCol { get; set; }

        public void IsClear()
        {
            if (coupon_data != null)
            {

                coupon_data.ItemsSource = null;
                coupon_data = null;
            }
            else
            {
                coupon_data = null;
            }
            select_all = false;
            cb_coupon_name_s = true;
            cb_start_date = true;
            cb_end_date = true;
            cb_max_balance = true;
            cb_exp_start_date = true;
            cb_exp_end_date = true;
            txt_coupon_name_s = null;
            txt_start_date = null;
            txt_end_date = null;
            txt_max_balance = null;
            txt_exp_start_date = null;
            txt_exp_end_date  = null;
        }

        private string query;
        private string start_date, end_date, exp_start_date, exp_end_date;
        public void GoCouponItem(object p)
        {
            select_all = false;
            coupon_data = p as DataGrid;

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
                        $"from v_all_customer " +
                        $"where if('{txt_coupon_name_s}' = '{""}' or '{cb_coupon_name_s}' = 'False', v_all_username = v_all_username, v_all_username like '%{txt_coupon_name_s}%') and " +
                                $"if('{start_date}' = '{""}' or '{cb_start_date}' = 'False', v_all_c_date = v_all_c_date, v_all_c_date between '{start_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                $"if('{end_date}' = '{""}' or '{cb_end_date}' = 'False', v_all_c_date = v_all_c_date, v_all_c_date between '1999-01-01%' and '{end_date}%') and " +
                                $"if('{txt_max_balance}' = '{""}' or '{cb_max_balance}' = 'False', v_all_remaining_amount = v_all_remaining_amount, v_all_remaining_amount <= '{txt_max_balance}') and " +
                                $"if('{exp_start_date}' = '{""}' or '{cb_exp_start_date}' = 'False', v_all_e_date = v_all_e_date, v_all_e_date between '{exp_start_date} %' and '{DateTime.Now.Date.ToString("yyyy-MM-dd", new CultureInfo("us-US", false))}%') and " +
                                $"if('{exp_end_date}' = '{""}' or '{cb_exp_end_date}' = 'False', v_all_e_date = v_all_e_date, v_all_e_date between '1999-01-01%' and '{exp_end_date}%') and " +
                                $"v_all_type_name = 'coupon' " +
                        $"order by v_all_username";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                coupon_data.ItemsSource = dt.DefaultView;
                Sconn.conn.Close();
            }
            catch (MySqlException ex)
            {
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
            finally
            {
                Sconn.conn.Close();
            }
        }

        public void GoCouponItemChanged(object p)
        {
            var item = p as DataGrid;
            coupon_item = item.SelectedItem as DataRowView;
            coupon_index = item.SelectedIndex;
        }

        public void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                }).ContinueWith((previousTask) => {
                    int i;
                    
                    string query = $"select * from coupon";

                    try
                    {
                        Sconn.conn.Open();

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
                        Sconn.conn.Close();

                        btn_search.Execute(coupon_data);
                    }
                    catch (MySqlException ex)
                    {
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
                    finally
                    {
                        Sconn.conn.Close();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }
        }
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
