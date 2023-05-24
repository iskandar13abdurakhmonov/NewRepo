using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class Connect
    {
        public static MySqlConnection conn;
        public void getConnection()
        {
            //replace server,port,user and password with your mysql server,port,user and password
            conn = new MySqlConnection("server=localhost;database=store;port=3306;user=root;password=mdist");
        }
        
    }
}
