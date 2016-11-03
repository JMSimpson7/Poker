using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PokerPrototype.Models
{
    public class LoginModel
    {
        [System.Web.Script.Serialization.ScriptIgnore]
        public int id { get; set; }
        public string usernameError { get; set; }
        public string passwordError { get; set; }
        public LoginModel(string username, string password)
        {
            id = 0;
            usernameError = passwordError = "";
            if (password.Length == 0)
            {
                passwordError = "Enter a password";
            }
            if (username.Length == 0)
            {
                usernameError = "Enter a username";
            }
            else
            {
                try
                {
                    MySqlConnection Conn = new MySqlConnection("server=sql9.freemysqlhosting.net;database=sql9140372;user=sql9140372;password=WSx2C8iRZx;");
                    var cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    Conn.Open();
                    cmd.Connection = Conn;
                    cmd.CommandText = "SELECT id FROM users WHERE username = @user AND password = @pass";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        id = Convert.ToInt32(rdr[0]);
                    } else
                    {
                        passwordError = "Username and password didn't match";
                    }
                }
                catch (Exception ex)
                {
                    passwordError = ex.Message;
                }
            }
        }
    }
}