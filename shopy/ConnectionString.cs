using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shopy
{
    public static class ConnectionString
    {
        public static MySqlConnection connection = new MySqlConnection(string.Format("server={3};uid={0};pwd={1};database={2};", Properties.Settings.Default.dbUser, Properties.Settings.Default.dbPassword, Properties.Settings.Default.dbName, Properties.Settings.Default.dbServer));
        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlDataAdapter adapter = new MySqlDataAdapter();
        public static MySqlTransaction transaction;
    }
}
