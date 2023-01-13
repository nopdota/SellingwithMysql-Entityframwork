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
    public class PD_TYPEController : Controller
    {
        private testEntities1 db = new testEntities1();

        // GET: pd_type ดึงข้อม฿ลด้วย Entity Framwork
        public ActionResult Index()
        {
            return View(db.pd_type.ToList());
        }

        // GET: ส่ง ID ผ่าน parameter เข้าสู่ Details 
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_type pd_type = db.pd_type.Find(id);
            if (pd_type == null)
            {
                return HttpNotFound();
            }
            return View(pd_type);
        }

        // GET: หน้าเพิ่มข้อมูล
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า create
        public ActionResult Create([Bind(Include = "TP_ID,TP_NAME")] pd_type pd_type)
        {
            pd_type.TP_ID = "1";
            var lastid = db.pd_product.OrderByDescending(x => x.PD_ID).FirstOrDefault();
            if (lastid != null)
            {
                string lastnumberid = lastid.PD_ID.Substring(2);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                string TP_ID = "TP";
                for (int i = checkmaxlenght; i < 3; i++)
                {
                    TP_ID += "0";
                }
                TP_ID += lastnumberidINT;

                pd_type.TP_ID = TP_ID;
            }
            else
            {
                pd_type.TP_ID = "TP001";
            }
            if (ModelState.IsValid)
            {
                db.pd_type.Add(pd_type);
                db.SaveChanges();
                return RedirectToAction("Index");//รีไดเรคกลับไปหน้า Index ของ pd_type คอนโทรเลอร์
            }

            return View(pd_type);
        }

        // ส่ง ID ผ่าน parameter มาที่ หน้า Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_type pd_type = db.pd_type.Find(id);
            if (pd_type == null)
            {
                return HttpNotFound();
            }
            return View(pd_type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า Edit
        public ActionResult Edit([Bind(Include = "TP_ID,TP_NAME")] pd_type pd_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pd_type).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pd_type);
        }


        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_type pd_type = db.pd_type.Find(id);
            if (pd_type == null)
            {
                return HttpNotFound();
            }
            return View(pd_type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            pd_type pd_type = db.pd_type.Find(id);
            db.pd_type.Remove(pd_type);
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
