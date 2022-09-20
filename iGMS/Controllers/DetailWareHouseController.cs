using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class DetailWareHouseController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: DetailWareHouse
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        [HttpGet]
        public JsonResult TonKho(string seach)
        {
            try
            {
                var c = (from b in db.DetailWareHouses.Where(x => x.Id > 0)
                         select new
                         {
                             id = b.Good.Id,
                             K = b.IdWareHouse == null ? b.Store.Name : b.WareHouse.Name,
                             name = b.Good.Name,
                             inventory = b.Inventory
                         }).ToList().Where(x => x.id.ToLower().Contains(seach) || x.name.Contains(seach));
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Receipt()
        {
            try
            {
                var c = (from b in db.Receipts.Where(x => x.Status == false)
                         select new
                         {
                             id = b.Id,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(string idwarehouse,string idsupplier,string idgood,float amount,string idReceipt)
        {
            try
            {

                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var e = db.DetailWareHouses.SingleOrDefault(x => (x.IdWareHouse == idwarehouse || x.IdStore == idwarehouse) && x.IdGoods == idgood);
                var f = db.Receipts.Find(idReceipt);
                var c = db.DetailGoodOrders.SingleOrDefault(x => x.IdGoods == idgood && x.IdPurchaseOrder == f.IdPurchaseOrder);
                if (c.Amount == 0)
                {
                    db.DetailGoodOrders.Remove(c);
                }
                f.Status = true;
                db.SaveChanges();
                if (e != null)
                {
                    e.Inventory += amount;
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var d = new DetailWareHouse();
                    if (idwarehouse.Substring(0, 1) == "K")
                    {
                        d.IdWareHouse = idwarehouse;
                    }
                    else
                    {
                        d.IdStore = idwarehouse;
                    }
                    d.IdGoods = idgood;
                    d.Inventory = amount;
                    db.DetailWareHouses.Add(d);

                    db.SaveChanges();
                    return Json(new { code = 300, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddCT(string id, string sumpricetax, string sumpriceli)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new License();
                var ids = db.Licenses.Where(x => x.Id == id).ToList();
                if (ids.Count == 0)
                {
                    d.Id = id;
                    d.Seri = id;
                    d.PriceTax = int.Parse(sumpricetax);
                    d.SumPrice = int.Parse(sumpriceli);
                    d.CreateDate = DateTime.Now;
                    db.Licenses.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Trùng Mã Phiếu !!!" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Show(string idReceipt)
        {
            try
            {
                var a = db.Receipts.Find(idReceipt);
                var idpurchaseorder = a.IdPurchaseOrder;
                var c = (from b in db.Receipts.Where(x => x.Id == idReceipt)
                         select new
                         {
                             id = b.Id,
                             method = b.Method.Name,
                             datepaydh = b.PurchaseOrder.DatePay.Value.Day+"/" + b.PurchaseOrder.DatePay.Value.Month+"/" + b.PurchaseOrder.DatePay.Value.Year,
                             namesupplier = b.PurchaseOrder.Supplier.Name,
                             idsupplier = b.PurchaseOrder.Supplier.Id,
                             warehouse = b.PurchaseOrder.WareHouse.Name == null ? b.PurchaseOrder.Store.Name : b.PurchaseOrder.WareHouse.Name,
                             idwarehouse = b.PurchaseOrder.WareHouse.Id == null ? b.PurchaseOrder.Store.Id : b.PurchaseOrder.WareHouse.Id,
                             sumprice = b.PurchaseOrder.Sumprice
                         }).ToList();
                var d = (from b in db.DetailGoodOrders.Where(x => x.IdPurchaseOrder == idpurchaseorder)
                         select new
                         {
                             id = b.Good.Id,
                             name = b.Good.Name,
                             unit = b.Good.Unit.Name,
                             amount = b.Amount,
                             price = b.Price,
                             discount = b.Discount,
                             pricediscount = b.PriceDiscount,
                             tax = b.TaxGTGT,
                             pricetax = b.PriceTax,
                             sumprice = b.Sumprice,

                         }).ToList();
                return Json(new { code = 200, c = c,d=d }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Last()
        {
            try
            {
                var idlast = db.DetailWareHouses.OrderBy(x => x.Id).ToList().LastOrDefault();
                var idpn = idlast.Id;
                return Json(new { code = 200, idpn = idpn,}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Show2(int id)
        {
            try
            {
                var a = (from b in db.SalesOrders.Where(x => x.Id == id)
                         select new
                         {
                             K = b.IdWareHouse==null?b.Store.Name:b.WareHouse.Name,
                             Kid = b.IdWareHouse==null?b.IdStore:b.IdWareHouse,
                             customer = b.Customer.Name,

                         }).ToList();
                var c = (from b in db.DetailSaleOrders.Where(x => x.IdSaleOrder == id)
                         select new
                         {
                             amount=b.Amount,
                             id = b.IdGoods,
                             name=b.Good.Name,

                         }).ToList();
                return Json(new { code = 200, a = a,c=c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Tru(string id,int idsaleorder,string amount,string K)
        {
            try
            {
                var a = db.DetailSaleOrders.SingleOrDefault(x => x.IdGoods == id && x.IdSaleOrder == idsaleorder);
                
                var H = "";
                var E = "";
                if (K.Contains("CH"))
                {
                    H = K;
                    E = null;
                }
                else
                {
                    E = K;
                    H = null;
                }
                var b = db.DetailWareHouses.Single(x=>x.IdGoods==id&&x.IdWareHouse==E&&x.IdStore==H);
                if (b == null)
                {
                }
                else
                {
                    if (b.Inventory < float.Parse(amount))
                    {
                        return Json(new { code = 1, msg = "Không Đủ hàng Giảm Số Lượng !!!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        a.Amount -= int.Parse(amount);
                        b.Inventory -= float.Parse(amount);
                        db.SaveChanges();
                    }
                  
                }
                return Json(new { code = 200, msg = "Chưa Có Hàng Trong kho !!!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Chưa Có Hàng Trong kho !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}