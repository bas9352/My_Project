using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ICMS_Server
{
    public class Database
    {
        public MySqlConnection conn { get; set; }
        public Database()
        {
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "127.0.0.1";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "icms";
            conn_string.SslMode = MySqlSslMode.None;
            conn_string.CharacterSet = "utf8";
            conn_string.ConvertZeroDateTime = true;
            conn_string.ConnectionReset = false;
            conn_string.Pooling = true;
            conn_string.MinimumPoolSize = 10;
            conn_string.MaximumPoolSize = 50;

            conn = new MySqlConnection(conn_string.ToString());
        }
    }
}
