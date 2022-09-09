﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class LoginController : Controller
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: Login
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
     
        [HttpGet]
        public JsonResult LoginiGMS(string user,string pass)
        {
            try
            {
                var a = db.Users.SingleOrDefault(x => x.User1 == user && x.Pass == pass);
                if (a != null)
                {
                    Session["user"] = a;
                    return Json(new { code = 200, Url= "/Home/Index" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300,msg="Tài Khoản Hoặc Mật Khẩu không Đúng !!!" }, JsonRequestBehavior.AllowGet);
                }       
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Scan(string id)
        {
            try
            {
                var a = db.Users.SingleOrDefault(x => x.Id==id);
                if (a != null)
                {
                    Session["user"] = a;
                    return Json(new { code = 200, Url = "/Home/Index" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Tài Khoản Hoặc Mật Khẩu không Đúng !!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}