using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class RFIDController : Controller
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: RFID
        //------------------RFID---------------
        [HttpGet]
        public JsonResult AllShowEPC()
        {
            try
            {
                var stall = (Stall)Session["Stalls"];
                var store = (Stall)Session["Store"];
                var a = (from b in db.DetailEPCs.Where(x => x.IdEPC.Length > 0 && x.IdStall == stall.Id && x.Idstore == store.Id)
                         select new
                         {
                             id = b.IdEPC
                         }).ToList();
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CompareEPC(string epc)
        {
            try
            {
                var a = (from b in db.EPCs.Where(x => x.IdEPC == epc)
                         select new
                         {
                             idgood = b.IdGoods
                         }).ToList();
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteEPC(string epc)
        {
            try
            {
                var a = db.DetailEPCs.Find(epc);
                db.DetailEPCs.Remove(a);
                db.SaveChanges();
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public string Post(Root root)
        {
            var stall = (Stall)Session["Stalls"];
            var store = (Stall)Session["Store"];
            var tags = root.tag_reads.ToList();
            foreach (var tag in tags)
            {
                DetailEPC t = new DetailEPC
                {
                    IdEPC = tag.epc
                };
                t.IdStall = stall.Id;
                t.Idstore = store.Id;
                if (!db.DetailEPCs.Any(x => x.IdEPC.Equals(t.IdEPC)))
                    db.DetailEPCs.Add(t);
            }

            db.SaveChanges();

            return ("");
        }
        [HttpPost]
        public JsonResult getBarcodeByEPC(string[] tags)
        
        {
            List<string> lstBar = new List<string>();
            try
            {
                foreach (var tag in tags)
                {
                    var barcode = db.EPCs.FirstOrDefault(b => b.IdEPC == tag);
                    if(barcode != null)
                    {
                        if (!lstBar.Contains(barcode.IdGoods))
                        {
                            lstBar.Add(barcode.IdGoods);
                        }
                       
                    }
                 
                }
                return Json(new { code = 200, barcode = lstBar}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}