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
        public UserModel(int id)
        {
            if (id == 1)
            {
                username = "josh";
                currency = 777;
                avatar = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSB8XnaVskUDWYJwdEA4OG0g8SUuubshqXrA5xA9px9_NgrORMHzHpmMBA";
            }
        }
    }
}