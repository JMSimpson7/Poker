﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokerPrototype.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult Index(string roomid)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(roomid, @"^\d+$"))
            {
                ViewBag.RoomID = roomid;
                return View();
            }
            else
            {
                ViewBag.MessageType = "warning";
                ViewBag.Message = "Page not found";
                return View("/Views/Home/Landing.cshtml");
            }
        }

    }
}