using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerPrototype.Models
{
    public class UserModel
    {
        public string username { get; set; }
        public int currency { get; set; }
        public string avatar;
        private string email { get; set; }
        public UserModel(int id)
        {
            MySqlConnection Conn = new MySqlConnection("server=sql9.freemysqlhosting.net;database=sql9140372;user=sql9140372;password=WSx2C8iRZx;");
            var cmd = new MySql.Data.MySqlClient.MySqlCommand();
            Conn.Open();
            cmd.Connection = Conn;
            cmd.CommandText = "SELECT username,currency,avatar FROM users WHERE id = @id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                username = rdr[0].ToString();
                currency = Convert.ToInt32(rdr[1]);
                avatar = rdr[2].ToString();
            }
            else
            {
                username = "DEBUGGING MODE";
                currency = 0;
                avatar = "";
            }
        }
        public string getEmail()
        {
            return email;
        }
    }
}