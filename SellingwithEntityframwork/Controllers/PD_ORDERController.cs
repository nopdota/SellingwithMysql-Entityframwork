using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SellingwithEntityframwork.Models;

namespace SellingwithEntityframwork.Controllers
{
    public class PD_ORDERController : Controller
    {
        private testEntities1 db = new testEntities1();

        // GET: pd_order
        public ActionResult MainOrder()
        {
            if (db.pd_order.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList().Count != 0)
            {
                ViewData["listORder"] = db.pd_order.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList();
            }
            else
            {
                ViewData["listORder"] = null;
            }
            int s = db.pd_type.ToList().Count;
            ViewData["listTYPE"] = db.pd_type.ToList();
            ViewData["listPRODUCT"] = db.pd_product.ToList();
            return View();
        }
        public class GetOrderDataClass
        {

            public int Number { get; set; }
            public string ProdcutID { get; set; }
            public string ProdcutName { get; set; }
            public string Quantity { get; set; }
            public string Price { get; set; }
            public string Type { get; set; }
            public string Unit { get; set; }
        }
        public JsonResult GetOrderData(string oderid)
        {

            List<GetOrderDataClass> Data = new List<GetOrderDataClass>();
            int i = 1;
            var dbproduct = db.pd_product.ToList();
            var dbtype = db.pd_type.ToList();
            foreach (var item in db.pd_order.AsEnumerable().Where(x => x.OD_ID == oderid).ToList())
            {
                GetOrderDataClass dt = new GetOrderDataClass();
                dt.Number = i;
                dt.ProdcutID = item.PD_ID;
                dt.ProdcutName = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().PD_NAME;
                dt.Quantity = item.OD_QUANTITY.ToString();
                dt.Price = item.PRICE.ToString();
                dt.Type = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().TP_ID + " " + dbtype.Where(x => x.TP_ID == dbproduct.Where(q => q.PD_ID == item.PD_ID).FirstOrDefault().TP_ID).FirstOrDefault().TP_NAME;
                dt.Unit = dbproduct.Where(x => x.PD_ID == item.PD_ID).FirstOrDefault().PD_UNIT;
                i++;
                Data.Add(dt);

            }

            string strdatajsonOrder = JsonConvert.SerializeObject(Data);

            return Json(new { strjsondata = strdatajsonOrder, orderid = oderid });
        }
        public class ClassGetGroupOrderData
        {
            public int Number { get; set; }
            public string OrderID { get; set; }
            public string Date { get; set; }
            public string pricesum { get; set; }
        }
        public JsonResult GetGroupOrderData()
        {
            List<ClassGetGroupOrderData> data = new List<ClassGetGroupOrderData>();
            int i = 1;
            var pd_orderDB = db.pd_order.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).ToList();
            var Groppd_order = db.pd_order.AsEnumerable().Where(x => (x.OD_DATE.HasValue ? x.OD_DATE.Value.ToShortDateString() : "") == DateTime.Now.ToShortDateString()).GroupBy(x => x.OD_ID);
            foreach (var item in Groppd_order)
            {
                ClassGetGroupOrderData dt = new ClassGetGroupOrderData();
                dt.Number = i;
                dt.OrderID = item.Key;
                dt.Date = pd_orderDB.Where(x => x.OD_ID == item.Key).FirstOrDefault().OD_DATE.Value.ToShortDateString();
                dt.pricesum = pd_orderDB.Where(x => x.OD_ID == item.Key).Sum(x => x.PRICE).ToString();
                i++;
                data.Add(dt);
            }
            string strdatajsonOrder = JsonConvert.SerializeObject(data);
            return Json(new { strjsondata = strdatajsonOrder});
        }
        public ActionResult Index()
        {
            ViewData["listTYPE"] = db.pd_type.ToList();
            ViewData["listPRODUCT"] = db.pd_product.ToList();
            return View(db.pd_order.ToList());
        }

        public ActionResult Details(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            pd_order pd_order = db.pd_order.Where(x => x.OD_ID == id1 && x.PD_ID == id2).FirstOrDefault();
            if (pd_order == null)
            {
                return HttpNotFound();
            }
            return View(pd_order);
        }

        public class Iteminorder 
        { 
            public string PD_ID { get; set; }
            public string TP_ID { get; set; }
            public string QUNTITY { get; set; }
            public string PRICE { get; set; }
        }

        public JsonResult InputProductToOrder(string JsonlistProduct) 
        {
            List<pd_product> listProduct = JsonConvert.DeserializeObject<List<pd_product>>(JsonlistProduct);
            var Iteminsame = listProduct.GroupBy(x => new { x.PD_ID }).ToList();
            List<Iteminorder> iteminorder = new List<Iteminorder>();
            var dbproduct = db.pd_product.ToList();
            string OD_ID = "";
            bool checkfuntion = false;
            foreach (var item in Iteminsame) 
            {
                if(dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault() != null) 
                {
                    Iteminorder dt = new Iteminorder();
                    dt.PD_ID = item.Key.PD_ID;
                    dt.TP_ID = dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault().TP_ID;
                    dt.QUNTITY = listProduct.Where(x => x.PD_ID == item.Key.PD_ID).ToList().Count.ToString();
                    decimal price = (dbproduct.Where(x => x.PD_ID == item.Key.PD_ID).FirstOrDefault().PD_PRICE??0);
                    string strprice = (price.ToString()  ?? "0") ;
                    dt.PRICE = (decimal.Parse(strprice) * decimal.Parse(dt.QUNTITY)).ToString();
                    iteminorder.Add(dt);
                }

            }
            
            if (db.pd_order.ToList().Count!=0)
            {
                var lastid = db.pd_order.OrderByDescending(x => x.OD_ID).FirstOrDefault();
                string lastnumberid = lastid.OD_ID.Substring(6);
                int lastnumberidINT = int.Parse(lastnumberid) + 1;
                int checkmaxlenght = lastnumberidINT.ToString().Length;
                OD_ID = "OD" + DateTime.Now.Year.ToString();
                for (int i = checkmaxlenght; i < 4; i++)
                {
                    OD_ID += "0";
                }
                OD_ID += lastnumberidINT;
         
                foreach (var item in iteminorder) 
                {
                    pd_order pd_order = new pd_order();
                    pd_order.OD_ID = OD_ID;
                    pd_order.PD_ID = item.PD_ID;
                    pd_order.OD_QUANTITY = decimal.Parse(item.QUNTITY);
                    pd_order.PRICE = decimal.Parse(item.PRICE);
                    pd_order.OD_DATE = DateTime.Now;
                    CreateToOrder(pd_order);
                    pd_product pd_product = db.pd_product.Find(item.PD_ID);
                    pd_product.PD_VALUE = pd_product.PD_VALUE - pd_order.OD_QUANTITY;
                    FuntionEdit(pd_product);
                    checkfuntion = true;
                }
               
            }
            else
            {
                OD_ID = "OD" + DateTime.Now.Year.ToString() +"0001";

                foreach (var item in iteminorder)
                {
                    pd_order pd_order = new pd_order();
                    pd_order.OD_ID = OD_ID;
                    pd_order.PD_ID = item.PD_ID;
                    pd_order.OD_QUANTITY = decimal.Parse(item.QUNTITY);
                    pd_order.PRICE = decimal.Parse(item.PRICE);
                    pd_order.OD_DATE = DateTime.Now;
                    CreateToOrder(pd_order);
                    pd_product pd_product = db.pd_product.Find(item.PD_ID);
                    pd_product.PD_VALUE = pd_product.PD_VALUE - pd_order.OD_QUANTITY;
                    FuntionEdit(pd_product);
                    checkfuntion = true;
                }
               
            }
            return Json(new { success = checkfuntion, orderid = OD_ID });
        }
        public string FuntionEdit([Bind(Include = "PD_ID,PD_NAME,PD_VALUE,PD_PRICE,TP_ID,PD_UNIT")] pd_product pd_product)
        {
            string ok = "";
            if (ModelState.IsValid)
            {
                db.Entry(pd_product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ok = "Update Store ok";
            }

            return ok;
        }

        public string CreateToOrder([Bind(Include = "OD_ID,PD_ID,OD_DATE,OD_QUANTITY,PRICE")] pd_order pd_order)
        {
            string ok = "";
            if (ModelState.IsValid)
            {
                db.pd_order.Add(pd_order);
                db.SaveChanges();
                ok = "Insert Order ok";
            }

            return ok;
        }
        public ActionResult Edit(string id1,string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_order pd_order = db.pd_order.Where(x=>x.OD_ID==id1&&x.PD_ID==id2).FirstOrDefault();
            if (pd_order == null)
            {
                return HttpNotFound();
            }
            return View(pd_order);
        }

        // POST: pd_order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OD_ID,PD_ID,OD_DATE,OD_QUANTITY,PRICE")] pd_order pd_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pd_order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pd_order);
        }

        // GET: pd_order/Delete/5
        public ActionResult Delete(string id1, string id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pd_order pd_order = db.pd_order.Where(x => x.OD_ID == id1 && x.PD_ID == id2).FirstOrDefault();
            if (pd_order == null)
            {
                return HttpNotFound();
            }
            return View(pd_order);
        }

        // POST: pd_order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            pd_order pd_order = db.pd_order.Find(id);
            db.pd_order.Remove(pd_order);
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
