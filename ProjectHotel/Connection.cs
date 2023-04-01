using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectHotel
{
    internal class Connection
    {
        string conectionstring = "Server=localhost;Database=db_hotel;Uid=root;Pwd=;";
        MySqlConnection con;
        public void OpenConnection()
        {
            con = new MySqlConnection(conectionstring);
            con.Open();
        }
        public void CloseConnection()
        {
            con.Close();
        }
        public void ExecuteQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, con);
            command.ExecuteNonQuery();
        }
        public object ShowData(string query)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conectionstring);
            DataSet data = new DataSet();
            adapter.Fill(data);
            object bebas = data.Tables[0];
            return bebas;
        }
    }
}
