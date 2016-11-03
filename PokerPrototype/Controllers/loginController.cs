using PokerPrototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokerPrototype.Controllers
{
    public class LoginController : Controller
    {
        // GET: login
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            LoginModel model = new LoginModel(username, password);
            if (model.id > 0)
                Session["id"] = model.id;
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}