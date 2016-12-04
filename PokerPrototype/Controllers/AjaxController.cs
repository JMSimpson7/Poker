﻿using PokerPrototype.Models;
using PokerGame;
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
        public ActionResult changeAvatar(string src)
        {
            AvatarModel avatar = new AvatarModel(Convert.ToInt32(Session["id"]), src);

            return Json(avatar, JsonRequestBehavior.AllowGet);
        }

        public ActionResult changePassword(string oldPassword, string newPassword, string Confirm)
        {
            PasswordModel password = new PasswordModel(Convert.ToInt32(Session["id"]), oldPassword, newPassword, Confirm);

            return Json(password, JsonRequestBehavior.AllowGet);
        }

        public ActionResult changeEmail(string email, string password)
        {
            EmailModel model = new EmailModel(Convert.ToInt32(Session["id"]), email, password);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        // GET: Ajax
        public ActionResult Lobby()
        {
            
            return Json(new RoomList(), JsonRequestBehavior.AllowGet);
        }

        public  void GameInit()
        {
            GameManager game = new GameManager();
            game.join("1", 100, "Bob");
            game.join("2", 150, "Joe");
            game.init();
            game.updateState();
        }
    }
}