using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerPrototype.Models
{
    public class ProfileModel
    {
        public UserModel User;
        public string email;
        public ProfileModel(int id)
        {
            User = new UserModel(id);
            email = User.getEmail();
        }
    }
}