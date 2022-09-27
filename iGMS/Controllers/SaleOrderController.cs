using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class SaleOrderController : BaseController
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: SaleOrder
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult TonKho(string H,string id)
        {
            try
            {
                var c = db.DetailWareHouses.Where(x => (x.IdWareHouse == H || x.IdStore == H) && x.IdGoods.Contains(id) && x.Status==true);

                return Json(new { code = 200, c = c.Count(), }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Customer()
        {
            try
            {
                var c = (from b in db.Customers.Where(x => x.Id.Length > 0)
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
        public JsonResult UserSale()
        {
            try
            {
                var c = (from b in db.Users.Where(x => x.Id.Length > 0)
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
        public JsonResult Good(string id,string H)
        {
            try
            {
                var a = db.DetailWareHouses.Where(x => (x.IdWareHouse == H || x.IdStore == H) && x.IdGoods.Contains(id)&&x.Status==true);
                if (a.Count()>0)
                {
                    var c = (from b in db.Goods.Where(x => x.Id.Contains(id))
                             select new
                             {
                                 id = b.Id.Substring(0,b.Id.Length-8),
                                 name = b.Name,
                                 size = b.Size.Name,
                                 price = b.Price,
                             }).ToList();
                    return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 1, msg="Chưa Có Hàng Trong Nơi Xuất" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(string name,string H,string customerKH,string user,int paymethod,
                               float sumprice,float partialpay,float liabilities,DateTime datepay,DateTime deliverydate,string des)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;

                    var d = new SalesOrder();
                    d.Name = name;
                if (H.Contains("CH"))
                {
                    d.IdStore = H;
                }
                else
                {
                    d.IdWareHouse = H;
                }
                d.IdCustomer = customerKH;
                d.IdUser = user;
                d.IdPayMethod = paymethod;
                d.Receivable = sumprice;
                d.PartialPay = partialpay;
                d.Liabilities = liabilities;
                d.DatePay = datepay;
                d.DeliveryDate = deliverydate;
                d.Description = des;
                d.Status = false;
                d.CreateDate = DateTime.Now;
                    db.SalesOrders.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddDetails(string id,string H,int amount,float price,float sumpricegoods)
        {
            try
            {
                var sta = db.Goods.OrderBy(x => x.Id.Contains(id)).ToList();
                var a = db.SalesOrders.OrderBy(x => x.Id).ToList().LastOrDefault();
                var b = new DetailSaleOrder();
                b.IdGoods = id;
                b.IdSaleOrder = a.Id;
                b.Amount = amount;
                b.Amount1 = amount;
                b.Price = price;
                b.IdSaleOrder = a.Id;
                b.SumPrice = sumpricegoods;
                db.DetailSaleOrders.Add(b);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Bill(string H)
        {
            try
            {
                object a = null;
                if (H.Contains("CH"))
                {
                    a = (from b in db.Stores.Where(x => x.Id == H)
                         select new
                         {
                             name = b.Name
                         }).ToList();
                }
                else
                {
                    a = (from b in db.WareHouses.Where(x => x.Id == H)
                         select new
                         {
                             name = b.Name
                         }).ToList();
                }
                var d = db.SalesOrders.OrderBy(x => x.Id).ToList().LastOrDefault();
                var c = (from b in db.SalesOrders.Where(x => x.Id == d.Id)
                         select new
                         {
                             id = b.Id,
                             createdate = b.CreateDate.Value.Day+"/"+ b.CreateDate.Value.Month+"/"+ b.CreateDate.Value.Year,
                             customer = b.Customer.Name,
                             address = b.Customer.AddRess,
                             paymethod = b.PaymentMethod.Name,
                             datepay = b.DatePay.Value.Day+"/" + b.DatePay.Value.Month+"/" + b.DatePay.Value.Year,
                             sumprice = b.Receivable,
                         }).ToList();
                var e = (from b in db.DetailSaleOrders.Where(x => x.IdSaleOrder == d.Id)
                         select new
                         {
                             id = b.Good.Id,
                             name = b.Good.Name,
                             size = b.Good.Size.Name,
                             amount = b.Amount,
                             price=b.Price,
                             sumprice = b.SumPrice,

                         }).ToList();

                return Json(new { code = 200, a=a,c=c,e=e }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}