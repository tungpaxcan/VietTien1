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
                var a = (from b in db.DetailEPCs.Where(x => x.IdEPC.Length == 20 &&x.Status==true/* && x.IdStall == stall.Id && x.Idstore == store.Id*/)
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
                var a = (from b in db.DetailEPCs.Where(x => x.IdEPC == epc&&x.Status==true)
                         select new
                         {
                             idgood = b.IdEPC.Substring(0,b.IdEPC.Length-8)
                         }).ToList();
                var c = db.DetailEPCs.SingleOrDefault(x => x.IdEPC == epc && x.Status == true);
                if (c != null)
                {
                    c.Status = false;
                    db.SaveChanges();
                }
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CompareReceipt(string epc)
        {
            try
            {
                var a = (from b in db.DetailEPCs.Where(x => x.IdEPC == epc && x.Status == true)
                         select new
                         {
                             idgood = b.IdEPC.Substring(0, b.IdEPC.Length - 8)
                         }).ToList();
                var c = db.DetailEPCs.SingleOrDefault(x => x.IdEPC == epc && x.Status == true);
                if (c != null)
                {
                    c.Status = false;
                    db.SaveChanges();
                }
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Refresh()
        {
            try
            {
                var a = db.DetailEPCs.Where(x => x.Status == false).ToList();
                for (int i = 0; i < a.Count(); i++)
                {
                    var b = db.DetailEPCs.OrderBy(x => x.Status == false).ToList().LastOrDefault();
                    db.DetailEPCs.Remove(b);
                    db.SaveChanges();
                }
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult StatusEPC(string epc)
        {
            try
            {
                var c = db.DetailEPCs.SingleOrDefault(x => x.IdEPC == epc && x.Status == true);
                if (c != null)
                {
                    c.Status = false;
                    db.SaveChanges();
                }
                else
                {

                }                   
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteEPC()
        {
            try
            {
                var a = db.DetailEPCs.OrderBy(x=>x.Status).ToList().LastOrDefault();
                var b = db.DetailEPCs.Where(x => x.Status == false).ToList();
                for(int i = 0; i < b.Count(); i++)
                {
                    db.DetailEPCs.Remove(a);
                    db.SaveChanges();
                }
                
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
            //var stall = (Stall)Session["Stalls"];
            //var store = (Stall)Session["Store"];
            var tags = root.tag_reads.ToList();
            foreach (var tag in tags)
            {
                DetailEPC t = new DetailEPC
                {
                    IdEPC = tag.epc,
                    Status = true
                };

                //t.IdStall = stall.Id;
                //t.Idstore = store.Id;
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
        [HttpPost]
        public JsonResult checkRFID(string[] tags)
        {
            List<EPC> lstEPC = new List<EPC>();
            try
            {
                foreach (var tag in tags)
                {
                    var UnPaidECP = db.EPCs.FirstOrDefault(e => e.IdEPC == tag);
                    if (UnPaidECP != null)
                    {
                        if ((bool)UnPaidECP.Status)
                            lstEPC.Add(UnPaidECP);
                    }

                }
                var unpaids = lstEPC.Select(x => new { x.IdEPC }).ToList();
                return Json(new { unpaids }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}