using SellingwithEntityframwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellingwithEntityframwork.Controllers
{
    public class LoginController : Controller
    {
        private testEntities1 db = new testEntities1();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            try
            {

                Session.Abandon();
                return RedirectToAction("Login", "Login");

            }
            catch
            {
                Session.Abandon();
                return RedirectToAction("Login", "Login");
            }
        }
        public JsonResult Inputlogin(string user ,string pass) 
        {
            var dbuser = db.pd_user.Where(x => x.USERNAME == user && x.PASSWORD == pass).ToList();
            bool checklogin = false;
            if (dbuser.ToList().Count != 0)
            {
                checklogin = true;
                ViewBag.msg = dbuser.FirstOrDefault().USERNAME;
                Session["user"] = dbuser.FirstOrDefault().USERNAME;
            }
            else 
            {
                checklogin = false;
            }

            return Json(new {  success = checklogin, msg = ViewBag.msg });
        }
    }
}