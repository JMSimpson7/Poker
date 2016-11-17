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
        public ActionResult Register(string email, string username, string password, string confirm)
        {
            RegisterModel register = new RegisterModel(email, username, password, confirm);
            if (register.success)
            {
                Login(username, password);
            }
            return Json(register, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Payment(string amount, string name, string cardNumber, string cvc, string expires, string password)
        {
            PaymentModel payment = new PaymentModel(Convert.ToInt32(Session["id"]), amount, name, cardNumber, cvc, expires, password);
            if (payment.success)
            {
                
            }
            return Json(payment, JsonRequestBehavior.AllowGet);
        }
        /*public ActionResult Currency(string email, string avatar)
        {
            EditProfile change = new EditProfile(email, avatar);
            if (change.success > 0)
                

            return Json(change, JsonRequestBehavior.AllowGet);
        }*/


        // GET: Ajax
        public ActionResult Lobby()
        {
            
            return Json(new RoomList(), JsonRequestBehavior.AllowGet);
        }
    }
}