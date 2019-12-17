using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    public class AddEditPromotionViewModel :BaseView
    {
        #region Properties
        public Database Sconn = new Database();
        public bool grid_add_edit_p_check { get; set; } = true;
        public bool on_off_promo { get; set; } = false;
        public DataTable data { get; set; }
        public DataTable data_promo { get; set; }
        public DataRowView promo_item { get; set; } = null;
        public string promo_id { get; set; } = null;
        public string txt_promo_name { get; set; } = null;
        public string txt_promo_rate_point { get; set; } = null;
        public string txt_promo_rate { get; set; } = null;

        public int promo_index { get; set; }

        public string title { get; set; }

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
        public AddEditPromotionViewModel()
        {
            btn_ok = new RelayCommand(p =>
            {
                grid_add_edit_p_check = false;
                if (txt_promo_name == null || 
                    txt_promo_rate_point == null || 
                    txt_promo_rate == null ||
                    txt_promo_name == "" ||
                    txt_promo_rate_point == "" ||
                    txt_promo_rate == "")
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("enter_info");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    if (IsSelect() == true)
                    {
                        if (promo_id == null)
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_promo_name"].ToString() == txt_promo_name &&
                                    row["v_promo_status"].ToString() == "true" && 
                                    on_off_promo == true)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("promo_name_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                            }

                            if (num == 0)
                            {
                                if (IsInsert() == true)
                                {
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_success");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("add_success");
                                    DialogHost.Show(new WarningView(), "Msg", ExtendedClosingEventHandler);
                                }
                            }
                        }
                        else
                        {
                            int num = 0;
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                DataRow row = data.Rows[i];
                                if (row["v_promo_name"].ToString() == txt_promo_name &&
                                    row["v_promo_status"].ToString() == "true" &&
                                    on_off_promo == true &&
                                    row["v_promo_id"].ToString() != promo_id)
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("promo_name_unsuccess");
                                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                                }
                                else if(row["v_mt_promo"].ToString() == "true" &&
                                        row["v_promo_id"].ToString() == promo_id &&
                                        (row["v_promo_rate"].ToString() != txt_promo_rate ||
                                        row["v_promo_rate_point"].ToString() != txt_promo_rate_point))
                                {
                                    num = 1;
                                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                                    IoC.WarningView.msg_text = GetLocalizedValue<string>("cant_edit_promo");
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
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                IoC.PromotionView.item_promo.Execute(IoC.PromotionView.promo_data);
                grid_add_edit_p_check = true;
                IoC.Application.DialogHostMain = false;
                IsClear();
            }
            else
            {
                grid_add_edit_p_check = true;
            }
        }
        private void ConfirmClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == true)
            {
                grid_add_edit_p_check = true;
            }
        }

        public void IsClear()
        {
            promo_id = null;
            txt_promo_name = null;
            txt_promo_rate_point = null;
            txt_promo_rate = null;
            data = null;
            promo_item = null;
            promo_id = null;
            txt_promo_name = null;
            txt_promo_rate_point = null;
            txt_promo_rate = null;
            promo_index = 0;
        }

        public bool IsInsert()
        {
            string query = $"insert into promotion set " +
                           $"promo_name = '{txt_promo_name}', " +
                           $"promo_rate_point = '{txt_promo_rate_point}', " +
                           $"promo_rate = '{txt_promo_rate}', " +
                           $"promo_c_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("us-US", false))}', " +
                           $"promo_status = '{on_off_promo}' ";

            try
            {
                Sconn.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Close();
                Sconn.conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public bool IsUpdate()
        {
            string query = $"update promotion set " +
                           $"promo_name='{txt_promo_name}' , " +
                           $"promo_rate_point='{txt_promo_rate_point}' , " +
                           $"promo_rate = '{txt_promo_rate}', " +
                           $"promo_status = '{on_off_promo}' " +
                           $"where promo_id = '{promo_id}'";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Close();
                Sconn.conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public bool IsSelect()
        {
            string query = $"select * from v_promotion;";

            try
            {
                Sconn.conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, Sconn.conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                Sconn.conn.Close();
                data = dt;
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 0)
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                else
                {
                    IoC.WarningView.msg_title = GetLocalizedValue<string>("title_false");
                    IoC.WarningView.msg_text = GetLocalizedValue<string>("conn_unsuccess");
                    DialogHost.Show(new WarningView(), "Msg", ConfirmClosingEventHandler);
                }
                return false;
            }
            finally
            {
                Sconn.conn.Close();
            }
        }

        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":resLang:" + key);
        }
        #endregion
    }
}
