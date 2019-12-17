using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Server
{
    public class PromotionViewModel : BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public DataRowView promo_item { get; set; } = null;
        public DataGrid promo_data { get; set; } = null;
        public string promo_id { get; set; } = null;
        public int promo_index { get; set; }

        private int conn_number;

        #endregion

        #region Commands
        public ICommand btn_add { get; set; }
        public ICommand btn_edit { get; set; }
        public ICommand btn_del { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        public ICommand item_promo { get; set; }
        public ICommand item_promo_change { get; set; }

        public ICommand DialogHostLoaded { get; set; }
        public ICommand WindowLoadedCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }
        #endregion

        #region Constructor
        public PromotionViewModel()
        {
            item_promo = new RelayCommand(p => GoPromoItem(p));
            item_promo_change = new RelayCommand(p => GoPromoItemChange(p));

            btn_add = new RelayCommand(p =>
            {
                IoC.AddEditPromotionView.title = GetLocalizedValue<string>("add_promo");
                IoC.AddEditPromotionView.promo_id = null;
                DialogHost.Show(new AddEditPromotionView(), "Main");
            });
            btn_edit = new RelayCommand(p =>
            {
                IoC.AddEditPromotionView.title = GetLocalizedValue<string>("edit_promo");
                if (promo_item == null)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                }
                else
                {
                    IoC.AddEditPromotionView.txt_promo_name = promo_item["promo_name"].ToString();
                    IoC.AddEditPromotionView.txt_promo_rate_point = promo_item["promo_rate_point"].ToString();
                    IoC.AddEditPromotionView.txt_promo_rate = promo_item["promo_rate"].ToString();
                    IoC.AddEditPromotionView.on_off_promo = bool.Parse(promo_item["promo_status"].ToString());
                    IoC.AddEditPromotionView.promo_id = promo_item["promo_id"].ToString();
                    //CurrPage = ApplicationPage.Admin;
                    DialogHost.Show(new AddEditPromotionView(), "Main");
                }

            });

            btn_del = new RelayCommand(p =>
            {
                if (promo_item == null)
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
            });
        }
        #endregion

        #region Other method
        public void IsDelete(object sender, DialogClosingEventArgs eventArgs)
        {
            int num = 0;
            if ((bool)eventArgs.Parameter == true)
            {
                Task.Factory.StartNew(() =>
                {
                }).ContinueWith((previousTask) => {
                    string query = $"select * from promotion";

                    try
                    {
                        Sconn.conn.Open();

                        MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                        MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                        MySqlCommandBuilder cmdb = new MySqlCommandBuilder(adp);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        Sconn.conn.Close();
                        Console.WriteLine(cmdb.GetDeleteCommand().CommandText);
                        DataRow dr = dt.Rows[promo_index];
                        dr.Delete();
                        adp.Update(dt);
                        item_promo.Execute(promo_data);
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

        private void IsClear()
        {
            promo_item = null;
            promo_id = null;
            promo_index = 0;
    }

        public bool IsDelete()
        {
            string query = $"select * from promotion";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                MySqlCommandBuilder cmdb = new MySqlCommandBuilder(adp);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Console.WriteLine(cmdb.GetDeleteCommand().CommandText);
                DataRow dr = dt.Rows[promo_index];
                dr.Delete();
                adp.Update(dt);
                Sconn.conn.Close();
                return true;
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
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public void GoPromoItemChange(object p)
        {
            var item = p as DataGrid;
            promo_item = item.SelectedItem as DataRowView;
            promo_index = item.SelectedIndex;
        }

        public void GoPromoItem(object p)
        {
            IsClear();
            promo_data = p as DataGrid;
            string query = $"select * from promotion order by promo_id";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                promo_data.ItemsSource = dt.DefaultView;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", conn_fail);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", conn_fail);
                }
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        private void conn_fail(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.Application.DialogHostMsg = false;
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                }).ContinueWith((previousTask) =>
                {
                    item_promo.Execute(promo_data);
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
