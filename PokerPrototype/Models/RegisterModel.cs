using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PokerPrototype.Controllers
{
    public class RegisterModel
    {
        [System.Web.Script.Serialization.ScriptIgnore]
        public bool success { get; set; }
        public string usernameError { get; set; }
        public string passwordError { get; set; }
        public string confirmError { get; set; }
        public string emailError { get; set; }
        public RegisterModel(string email, string username, string password, string confirm)
        {
            success = true;
            //NEED TO ADD ERRORS FOR OTHER FIELDS
            usernameError = passwordError = "";
            if (password.Length == 0)
            {
                success = false;
                passwordError = "Enter a password";
            }
            if (confirm.Length == 0)
            {
                success = false;
                confirmError = "Confirm your password";
            }
            if (username.Length == 0)
            {
                success = false;
                usernameError = "Enter a username";
            }
            if (email.Length == 0)
            {
                success = false;
                emailError = "Enter an email";
            }
            if (password.Equals(confirm) == false)
            {
                success = false;
                confirmError = "Passwords do not match";
            }
            if (success)
            {
                try
                {
                    MySqlConnection Conn = new MySqlConnection("server=sql9.freemysqlhosting.net;database=sql9140372;user=sql9140372;password=WSx2C8iRZx;");
                    var cmd = new MySql.Data.MySqlClient.MySqlCommand();
                    Conn.Open();
                    cmd.Connection = Conn;
                    //Below line WILL NOT work withot correct db information
                    //I do not know how DB is organized, adjust INSERT command to fit
                    //For now, I'm copying the login example
                    //It goes without saying, but in the future we should avoid storing passwords
                    //Also, since I assume there's no field for it, email info is currently
                    //being tossed away
                    cmd.CommandText = "INSERT into users(username, password, email) VALUES (@user,@pass,@email) ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@email", email);
                    success = cmd.ExecuteNonQuery() > 0;


                    /*Shouldn't need this chunk but leaving it here just in case
                    if (rdr.Read())
                    {
                        id = Convert.ToInt32(rdr[0]);
                    } else
                    {
                    }*/
                }
                catch (Exception ex)
                {
                    passwordError = ex.Message;
                }
            }
        }
    }
}