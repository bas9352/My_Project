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
        public MySqlConnection conn = new MySqlConnection();
        public Database()
        {
            MessageBox.Show($"{"7"}");
            string Strconn = "server='127.0.0.1'; username='root'; password=''; database='icms'; sslmode='none'; CharSet='utf8'; convertzerodatetime=true; Connection Timeout=5;";
            conn = new MySqlConnection(Strconn);
        }
    }
}
