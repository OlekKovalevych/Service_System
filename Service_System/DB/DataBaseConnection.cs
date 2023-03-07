using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_System.DB
{
    public class DataBaseConnection
    {
        private const string ConnectionStr = "server=localhost;user=root;database=service_sistem_db;password=Alyakoval;";
        private static readonly MySqlConnection Connection = new MySqlConnection(ConnectionStr);

        private DataBaseConnection() { }

        public static MySqlConnection GetConection()
        {
            return Connection;
        }
       
    }
}
