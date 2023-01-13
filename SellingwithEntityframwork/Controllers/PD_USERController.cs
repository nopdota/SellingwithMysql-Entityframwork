using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SellingwithEntityframwork.Models;

namespace SellingwithEntityframwork.Controllers
{
    public class PD_USERController : Controller
    {
        private testEntities1 db = new testEntities1();

        // GET: pd_user
        public ActionResult Index()
        {
            return View(db.pd_user.ToList());
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_user pd_user = db.pd_user.Find(id);
            if (pd_user == null)
            {
                return HttpNotFound();
            }
            return View(pd_user);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USERNAME,PASSWORD")] pd_user pd_user)
        {
            bool checkuser = false;
            if (pd_user.USERNAME != null) 
            { 
                int checkusercount =  db.pd_user.Where(x => x.USERNAME == pd_user.USERNAME).ToList().Count;
                if (checkusercount == 0)
                {
                    checkuser = false;
                }
                else 
                {
                    checkuser = true;
                }
            }
            if (checkuser == false)
            {
                if (ModelState.IsValid)
                {
                    db.pd_user.Add(pd_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else 
            {
                ViewBag.alertmsg = "username ถูกใช้ไปแล้ว";
            }
            return View(pd_user);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_user pd_user = db.pd_user.Find(id);
            if (pd_user == null)
            {
                return HttpNotFound();
            }
            return View(pd_user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USERNAME,PASSWORD")] pd_user pd_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pd_user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pd_user);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_user pd_user = db.pd_user.Find(id);
            if (pd_user == null)
            {
                return HttpNotFound();
            }
            return View(pd_user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            pd_user pd_user = db.pd_user.Find(id);
            db.pd_user.Remove(pd_user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
