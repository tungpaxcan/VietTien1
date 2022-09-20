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
        public JsonResult AddDetail(int amount,string goods,float price,float discount,float pricediscount,float tax,float pricetax,float sumpricegoods)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var idpurchaseorder = db.PurchaseOrders.OrderBy(x => x.Id);
                var d = new DetailGoodOrder();
                d.IdGoods = goods;
                d.PurchaseOrder = idpurchaseorder.ToList().LastOrDefault();
                d.Price = price;
                d.Discount = discount;
                d.PriceDiscount = pricediscount;
                d.TaxGTGT = tax;
                d.PriceTax = pricetax;
                d.Amount = amount;
                d.Amount1 = amount;
                d.Sumprice = sumpricegoods;
                d.CreateDate = DateTime.Now;
                d.CreateBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
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
        public JsonResult EditDetailSupplierGoods(string supplier,string goods,float price,float tax,float discount)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.DetailSupplierGoods.SingleOrDefault(x=>x.IdSupplier==supplier&&x.IdGoods==goods);
                d.PurchasePrice = price;
                d.PurchaseTax = tax;
                d.PurchaseDiscount = discount;
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
                             name = b.Good.Name,
                             unit = b.Good.Unit.Name,
                             amount = b.Amount,
                             price = b.Price,
                             discount = b.Discount,
                             tax = b.TaxGTGT,
                             pricediscount = b.PriceDiscount,
                             pricetax = b.PriceTax,
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
                             unit = b.Good.Unit.Name,
                             purchaseprice = b.PurchasePrice==null?0:b.PurchasePrice,
                             purchasediscount= b.PurchaseDiscount==null?0:b.PurchaseDiscount,
                             purchasetax = b.PurchaseTax == null ? 10 : b.PurchaseTax
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
    }
}