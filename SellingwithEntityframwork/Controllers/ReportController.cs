using ClosedXML.Excel;
using OfficeOpenXml;
using SellingwithEntityframwork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellingwithEntityframwork.Controllers
{

    public class ReportController : Controller
    {
        private testEntities1 db = new testEntities1();
        public class ClassReportIndex
        {
            public string OR_ID {get;set;}
            public string PD_ID { get; set; }
            public string PD_NAME { get; set; }
            public string OD_QUANTITY { get; set; }
            public string PRICE { get; set; }
            public string OD_DATE { get; set; }
            public string TP_ID { get; set; }
            public string TP_NAME { get; set; }
            public string UNIT { get; set; }
        }
        public ActionResult ReportIndex()
        {
            string a = DateTime.Now.Month.ToString();
            List<ClassReportIndex> data = new List<ClassReportIndex>();
            var GropOP = from ORDER in db.pd_order.AsEnumerable() join PRDOCUT in db.pd_product.AsEnumerable() on new { PD_ID = ORDER.PD_ID } equals new { PD_ID = PRDOCUT.PD_ID } select new { OR_ID = ORDER.OD_ID, PD_ID = ORDER.PD_ID, PD_NAME = PRDOCUT.PD_NAME, OD_QUANTITY = ORDER.OD_QUANTITY, PRICE = ORDER.PRICE, OD_DATE = ORDER.OD_DATE.Value.ToShortDateString(), TP_ID = PRDOCUT.TP_ID ,UNIT = PRDOCUT.PD_UNIT };
            var GropFulljoin = from order in GropOP join TYPE in db.pd_type on new { TP_ID = order.TP_ID } equals new { TP_ID = TYPE.TP_ID } select new ClassReportIndex { OR_ID = order.OR_ID, PD_ID = order.PD_ID, PD_NAME = order.PD_NAME, OD_QUANTITY = order.OD_QUANTITY.ToString(), PRICE = order.PRICE.ToString(), OD_DATE = order.OD_DATE, TP_ID = order.TP_ID, TP_NAME = TYPE.TP_NAME,UNIT = order.UNIT };
            data.AddRange(GropFulljoin);
             ViewData["ReportIndex"] = data;
            return View();
        }
        public class ClassGroupsumPriceOrderInMount 
        { 

            public string Mount { get; set; }
            public string Price { get; set; }

        }
        public ActionResult GroupsumPriceOrderInMount(string year) 
        {
            
            var GropOP = from ORDER in db.pd_order.AsEnumerable() join PRDOCUT in db.pd_product.AsEnumerable() on new { PD_ID = ORDER.PD_ID } equals new { PD_ID = PRDOCUT.PD_ID } select new { OR_ID = ORDER.OD_ID, PD_ID = ORDER.PD_ID, PD_NAME = PRDOCUT.PD_NAME, OD_QUANTITY = ORDER.OD_QUANTITY, PRICE = ORDER.PRICE, OD_DATE = ORDER.OD_DATE.Value.ToShortDateString(), TP_ID = PRDOCUT.TP_ID };
            var GropFulljoin = from order in GropOP join TYPE in db.pd_type on new { TP_ID = order.TP_ID } equals new { TP_ID = TYPE.TP_ID } select new { OR_ID = order.OR_ID, PD_ID = order.PD_ID, PD_NAME = order.PD_NAME, OD_QUANTITY = order.OD_QUANTITY, PRICE = order.PRICE, OD_DATE = order.OD_DATE, TP_ID = order.TP_ID, TP_NAME = TYPE.TP_NAME };
            GropFulljoin = GropFulljoin.Where(x=> DateTime.Parse(x.OD_DATE).Year.ToString() == year).ToList();
            List<ClassGroupsumPriceOrderInMount> data = new List<ClassGroupsumPriceOrderInMount> ();
            for (int i = 1; i <= 12; i++) 
            {
                ClassGroupsumPriceOrderInMount dt = new ClassGroupsumPriceOrderInMount();
                dt.Mount = i + " " + new DateTime(int.Parse(year), i, 1).ToString("MMMM");
                dt.Price = GropFulljoin.Where(x => DateTime.Parse(x.OD_DATE).Month.ToString() == i.ToString()).Sum(x => x.PRICE).ToString();
                data.Add(dt);
            }
            //GropFulljoin = GropFulljoin.GroupBy(x=> DateTime.Parse(x.OD_DATE).Month ==)
            ViewData["GroupsumPriceOrderInMount"] = data.ToList();

            return PartialView("GroupsumPriceOrderInMount");
        }
        public ActionResult ExportExcel()
        {
            var GropOP = from ORDER in db.pd_order.AsEnumerable() join PRDOCUT in db.pd_product.AsEnumerable() on new { PD_ID = ORDER.PD_ID } equals new { PD_ID = PRDOCUT.PD_ID } select new { OR_ID = ORDER.OD_ID, PD_ID = ORDER.PD_ID, PD_NAME = PRDOCUT.PD_NAME, OD_QUANTITY = ORDER.OD_QUANTITY, PRICE = ORDER.PRICE, OD_DATE = ORDER.OD_DATE.Value.ToShortDateString(), TP_ID = PRDOCUT.TP_ID, UNIT = PRDOCUT.PD_UNIT };
            var GropFulljoin = from order in GropOP join TYPE in db.pd_type on new { TP_ID = order.TP_ID } equals new { TP_ID = TYPE.TP_ID } select new ClassReportIndex { OR_ID = order.OR_ID, PD_ID = order.PD_ID, PD_NAME = order.PD_NAME, OD_QUANTITY = order.OD_QUANTITY.ToString(), PRICE = order.PRICE.ToString(), OD_DATE = order.OD_DATE, TP_ID = order.TP_ID, TP_NAME = TYPE.TP_NAME, UNIT = order.UNIT };

            ViewData["ReportIndex"] =  GropFulljoin.ToList();
            // Get the data to export
            var workbook = new XLWorkbook();

            Response.ClearContent();

            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition",
                "attachment; filename=" + "สรุปการขาย" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            //string fileCss = Server.MapPath("~/css/daoChuCSS.css");
            //string cssText = string.Empty;
            //StreamReader sr = new StreamReader(fileCss);
            //var line = string.Empty;
            //while ((line = sr.ReadLine()) != null)
            //{
            //    cssText += line;
            //}
            //sr.Close();
            // Response.Write("<style>" + cssText + "</style>");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Report Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");


            View("~/Views/Report/MainReport.cshtml", ViewData["ReportIndex"]).ExecuteResult(this.ControllerContext);
            Response.Flush();
            Response.End();
            return View();
        }

    }
}