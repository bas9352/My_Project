using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS_Client
{
    public class Database
    {
        public MySqlConnection conn = new MySqlConnection();
        public Database()
        {
            string Strconn = "server='127.0.0.1'; username='root'; password=''; database='icms'; sslmode='none'; CharSet='utf8'; convertzerodatetime=true;";
            conn = new MySqlConnection(Strconn);
        }
    }
}
