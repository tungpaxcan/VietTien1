using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class PurchaseOrderController : BaseController
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(int paymethod,string name,DateTime datepay,DateTime deliverydate,float sumprice,float liabilities,float partialpay,string des,string H,string supplier)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new PurchaseOrder();
                if (H.Contains("CH"))
                {
                    d.IdStore = H;
                }
                else
                {
                    d.IdWareHouse = H;
                }
                d.IdSupplier = supplier;
                d.Status = false;
                d.IdPayMethod = paymethod;
                d.Name = name;
                d.DatePay = datepay;
                d.DeliveryDate = deliverydate;
                d.Sumprice = sumprice;
                d.Liabilities = liabilities;
                d.PartialPay = partialpay;
                d.Description = des;
                d.CreateDate = DateTime.Now;
                d.CreateBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
                db.PurchaseOrders.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddDetail(string goods,float price,float sumpricegoods,string epc)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var idpurchaseorder = db.PurchaseOrders.OrderBy(x => x.Id);
                var idpu = idpurchaseorder.ToList().LastOrDefault();
                var EPC = db.DetailGoodOrders.SingleOrDefault(x => x.IdGoods == goods && x.EPC == epc);
                var id = idpu.Id;
                var d = new DetailGoodOrder();

                d.EPC = epc;
                d.IdGoods = goods;
                d.PurchaseOrder = idpurchaseorder.ToList().LastOrDefault();
                d.Price = price;
                d.Sumprice = sumpricegoods;
                d.CreateDate = DateTime.Now;
                d.CreateBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
                d.Status = true;
                db.DetailGoodOrders.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);        
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EditDetailSupplierGoods(string supplier,string goods,float price)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.DetailSupplierGoods.SingleOrDefault(x=>x.IdSupplier==supplier&&x.IdGoods==goods);
                d.PurchasePrice = price;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Bill(string supplier)
        {
            try
            {
                var c = (from b in db.Suppliers.Where(x => x.Id == supplier)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             address = b.AddRess,
                             phone = b.Phone,
                             fax = b.Fax
                         }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Bill1()
        {
            try
            {
                var a = db.PurchaseOrders.OrderBy(x => x.Id).ToList().LastOrDefault();
                var id = a.Id;
                var c = (from b in db.PurchaseOrders.Where(x => x.Id == id)
                                       select new
                                       {
                                           id = b.Id,
                                           datedh = b.CreateDate.Value.Day+"/"+b.CreateDate.Value.Month+"/"+b.CreateDate.Value.Year,
                                           paydh = b.PaymentMethod.Name,
                                           datepaydh = b.DatePay.Value.Day + "/" + b.DatePay.Value.Month + "/" + b.DatePay.Value.Year,
                                           sumpricedh = b.Sumprice
                                       }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Bill2()
        {
            try
            {
                var a = db.PurchaseOrders.OrderBy(x => x.Id).ToList().LastOrDefault();
                var id = a.Id;
                var c = (from b in db.DetailGoodOrders.Where(x => x.IdPurchaseOrder == id)
                         select new
                         {
                             idgoods = b.IdGoods,
                             epc = b.EPC,
                             name = b.Good.Name,
                             size = b.Good.Size.Name,
                             price = b.Price,
                             sumprice = b.Sumprice
                         }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Supplier()
        {
            try
            {
                var c = (from b in db.Suppliers.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ListGoods(string supplier,string seach)
        {
            try
            {
                var c = (from b in db.DetailSupplierGoods.Where(x => x.IdSupplier == supplier)
                         select new
                         {
                             id = b.Good.Id,
                             name = b.Good.Name,
                             size = b.Good.Size.Name,
                             purchaseprice = b.PurchasePrice==null?0:b.PurchasePrice,
                             purchasediscount= b.PurchaseDiscount==null?0:b.PurchaseDiscount,
                             purchasetax = b.PurchaseTax == null ? 0 : b.PurchaseTax
                         }).ToList().Where(x=>x.id.Contains(seach));
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PayMethod()
        {
            try
            {
                var c = (from b in db.PaymentMethods.Where(x => x.Id > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult WareHouse()
        {
            try
            {
                var c = (from b in db.WareHouses.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Store()
        {
            try
            {
                var c = (from b in db.Stores.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Active()
        {
            try
            {
                var c = (from b in db.Stores.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult EPCDaCo()
        {
            try
            {
                var c = (from b in db.EPCs.Where(x => x.Id > 0)
                         select new
                         {
                             epc = b.IdEPC
                         }).ToList();
                var d = (from b in db.DetailGoodOrders.Where(x => x.Id > 0)
                         select new
                         {
                             epc = b.EPC
                         }).ToList();
                return Json(new { code = 200, c = c,d=d }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}