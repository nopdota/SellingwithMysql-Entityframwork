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
    public class PD_PRODUCTController : Controller
    {
        private testEntities1 db = new testEntities1();

        // GET: pd_product ดึงข้อม฿ลด้วย Entity Framwork
        public ActionResult Index()
        {
            ViewData["listTYPE"] = db.pd_type.ToList();
            return View(db.pd_product.ToList());
        }
        // GET: ส่ง ID ผ่าน parameter เข้าสู่ Details 
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_product pd_product = db.pd_product.Find(id);
            if (pd_product == null)
            {
                return HttpNotFound();
            }
            return View(pd_product);
        }
        // GET: หน้าเพิ่มข้อมูล
        public ActionResult Create()
        {
            ViewData["listTYPE"] = db.pd_type.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า create
        public ActionResult Create([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] pd_product pd_product)
        {
            pd_product.PD_ID = "1";
            var lastid = db.pd_product.OrderByDescending(x => x.PD_ID).FirstOrDefault();
            if (lastid != null)
            {
                string lastnumberid = lastid.PD_ID.Substring(2);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                string PD_ID = "PD";
                for (int i = checkmaxlenght; i < 4; i++) 
                {
                    PD_ID += "0";
                }
                PD_ID += lastnumberidINT;

                pd_product.PD_ID = PD_ID;
            }
            else
            {
                pd_product.PD_ID ="PD0001";
            }

            if (ModelState.IsValid)
            {
                db.pd_product.Add(pd_product);
                db.SaveChanges();
                return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ pd_product คอนโทรเลอร์
            }

            return View(pd_product);
        }
        // ส่ง ID ผ่าน parameter มาที่ หน้า Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_product pd_product = db.pd_product.Find(id);
            if (pd_product == null)
            {
                return HttpNotFound();
            }
            ViewData["listTYPE"] = db.pd_type.ToList();
            return View(pd_product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //ส่งข้อมูลผ่าน การsubmit ด้วยการ post จากหน้า edit
        public ActionResult Edit([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] pd_product pd_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pd_product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ pd_product คอนโทรเลอร์
            }
            return View(pd_product);
        }
        // ส่ง ID ผ่าน parameter มาที่ หน้า Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_product pd_product = db.pd_product.Find(id);
            if (pd_product == null)
            {
                return HttpNotFound();
            }
            return View(pd_product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            pd_product pd_product = db.pd_product.Find(id);
            db.pd_product.Remove(pd_product);
            db.SaveChanges();
            return RedirectToAction("Index"); //รีไดเรคกลับไปหน้า Index ของ pd_product คอนโทรเลอร์
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
