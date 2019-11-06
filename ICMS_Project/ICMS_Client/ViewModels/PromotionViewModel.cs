using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using WPFLocalizeExtension.Extensions;

namespace ICMS_Client
{
   
    public class PromotionViewModel : BaseView 
    {
        #region Properties
        public Database Sconn = new Database();
        private DataRowView promo_item;
        private int promo_index;

        public Window mainWindow { get; set; }
        public Window MainApp { get; set; }
        public ApplicationPage CurrPage { get; set; } = ApplicationPage.Promotion;

        #endregion

        #region Commands
        public ICommand item_promo { get; set; }
        public ICommand item_promo_change { get; set; }
        public ICommand btn_ok { get; set; }
        public ICommand btn_cancel { get; set; }
        #endregion

        #region Constructor
        public PromotionViewModel()
        {
            item_promo = new RelayCommand(p=> GoItemPromo(p));
            item_promo_change = new RelayCommand(p => GoItemPromoChanged(p));

            btn_ok = new RelayCommand(p =>
            {
                if (promo_item == null)
                {
                    MessageBox.Show($"no data");
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg");
                    //MessageBox.Show($"{IoC.MainView.DialogHostMsg}");

                }
                else
                {
                    if (IsUpdate() == true)
                    {
                        MessageBox.Show($"finisha");
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("success");
                        DialogHost.Show(new WarningView(), "Main", ExtendedClosingEventHandler);
                    }
                    else
                    {
                        MessageBox.Show($"dont finish");
                        IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                        IoC.WarningView.msg_text = GetLocalizedValue<string>("unsuccess");
                        DialogHost.Show(new WarningView(), "Main");
                    }
                }
            });

            btn_cancel = new RelayCommand(p =>
            {
                //DialogHost.Show(new WarningView(), "Msg");
                IoC.Application.MainApp.Close();
            });
        }

        public bool IsUpdate()
        {
            //string query = $"update member set member_remaining_point = '{float.Parse(IoC.MainView.txt_bonus) - float.Parse(promo_item["promo_rate_point"].ToString())}', member_total_amount ='{float.Parse(IoC.MainView.txt_total_amount) + float.Parse(promo_item["promo_rate"].ToString())}' ,member_remaining_free_amount ='{float.Parse(IoC.MainView.txt_remaining_free_amount) + float.Parse(promo_item["promo_rate"].ToString())}',member_total_free_amount ='{float.Parse(IoC.MainView.txt_total_free_amount) + float.Parse(promo_item["promo_rate"].ToString())}' where member_id ='{IoC.MainView.member_id}'";
            
            //if (OpenConnection() == true)
            //{
            //    try
            //    {
            //        MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
            //        MySqlDataReader reader = cmd.ExecuteReader();

            //        if (!reader.Read())
            //        {
            //            reader.Close();
            //            Sconn.conn.Close();
            //            return true;
            //        }
            //        else
            //        {
            //            reader.Close();
            //            Sconn.conn.Close();
            //            return false;
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        Sconn.conn.Close();
            //        return false;
            //    }
            //    finally
            //    {
            //        Sconn.conn.Close();
            //    }

            //}
            //else
            //{
            //    Sconn.conn.Close();
            //    return false;
            //}
            return true;
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                MessageBox.Show($"finisha");
                IsClear();
                IoC.Application.MainApp.Close();
                //IoC.StaffView.IsSelect();
            }
        }

        private void IsClear()
        {
        }

        public void GoItemPromoChanged(object p)
        {
            var item = p as DataGrid;
            promo_item = item.SelectedItem as DataRowView;
            promo_index = item.SelectedIndex;
        }

        public void GoItemPromo(object p)
        {
            var item = p as DataGrid;
            string query = $"select * from promotion order by promo_id";

            if (OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    item.ItemsSource = dt.DefaultView;
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

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
