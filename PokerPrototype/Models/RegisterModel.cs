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
        public int success { get; set; }
        public string usernameError { get; set; }
        public string passwordError { get; set; }
        public string passwordConfirmError { get; set; }
        public string emailError { get; set; }
        public RegisterModel(string email, string username, string password, string passwordConfirm)
        {
            success = -1;
            //NEED TO ADD ERRORS FOR OTHER FIELDS
            usernameError = passwordError = "";
            if (password.Length == 0)
            {
                passwordError = "Enter a password";
            }
            else
            if (passwordConfirm.Length == 0)
            {
                passwordConfirmError = "Confirm your password";
            }
            else
            if (username.Length == 0)
            {
                usernameError = "Enter a username";
            }
            else
            if (email.Length == 0)
            {
                emailError = "Enter an email";
            }
            else
            if (password.Equals(passwordConfirm) == false)
            {
                passwordConfirmError = "Passwords do not match";
            }
            else
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
                    cmd.CommandText = "INSERT into users(username, password) VALUES (@user,@pass) ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);
                    success = cmd.ExecuteNonQuery();


                    /*Shouldn't need this chunk but leaving it here just in case
                    if (rdr.Read())
                    {
                        id = Convert.ToInt32(rdr[0]);
                    } else
                    {
                        //need just enough info to give users something to say 
                        //to support without giving enough for attacks
                        //maybe use animal codenames?
                        passwordError = "Something went wrong!";
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