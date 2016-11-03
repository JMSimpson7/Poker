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
        // GET: Ajax
        public ActionResult Lobby()
        {
            
            return Json(new RoomList(), JsonRequestBehavior.AllowGet);
        }
    }
}