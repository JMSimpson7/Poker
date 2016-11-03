using PokerPrototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokerPrototype.Controllers
{
    public class AjaxController : Controller
    {
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            LoginModel model = new LoginModel(username, password);
            if (model.id > 0)
                Session["id"] = model.id;

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Register(string email, string username, string password, string passwordConfirm)
        {
            RegisterModel register = new RegisterModel(email, username, password, passwordConfirm);
            if (register.success > 0)
            {
                Login(username, password);
            }
            return Json(register, JsonRequestBehavior.AllowGet);
        }


        // GET: Ajax
        public ActionResult Lobby()
        {
            
            return Json(new RoomList(), JsonRequestBehavior.AllowGet);
        }
    }
}