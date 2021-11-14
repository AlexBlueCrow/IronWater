using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WindowsFormsApp1
{
    class DBcontrol
    {
        SqlConnection connection;
        public SqlConnection connect()
        {
            string str = @"Data Source=(LocalDB)\MSSQLLocalDB;
                           
                           Integrated Security = True;
                           AttachDbFilename='D:\working place\WindowsFormsApp1\Database1.mdf'";
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            return connection;
        }

        public SqlCommand command(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, connect());
            return cmd;
        }

        public int Execute (string sql)
        {
            return command(sql).ExecuteNonQuery();
        }

        public SqlDataReader read(string sql)
        {
            return command(sql).ExecuteReader();
        }
        
        public void Close()
        {
            connection.Close();
        }
    }
}
