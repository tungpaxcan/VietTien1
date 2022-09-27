using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class ReceiptController : BaseController
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: Receipt
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(string id,int purchaseorder,string user1,string user2,string des,int method)
        {
            try
            {

                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var c = db.PurchaseOrders.Find(purchaseorder);
                var d = new Receipt();
                var ids = db.Receipts.Where(x => x.Id == id).ToList();
                if (ids.Count == 0)
                {
                    d.Id = id;
                    d.IdPurchaseOrder = purchaseorder;
                    d.IdMethod = method;
                    d.IdUser1 = user1 == "-1" ? null : user1;
                    d.IdUser2 = user2 == "-1" ? null : user2;
                    d.Description = des;
                    d.CreateDate = DateTime.Now;
                    d.Status = false;
                    c.Status = true;
                    db.Receipts.Add(d);

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
        [HttpPost]
        public JsonResult AddCT(string id,string sumpricetax,string sumpriceli)
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
        [HttpPost]
        public JsonResult DaNhan(string id, string amounttext, int purchaseorder,string idd)
        {
            try
            {
                for(int i = 0; i < int.Parse(amounttext); i++)
                {
                    var c = db.DetailGoodOrders.OrderBy(x => x.IdGoods.Contains(id) && x.IdPurchaseOrder == purchaseorder&&x.Status==true).ToList().LastOrDefault();
                    var d = db.DetailWareHouses.OrderBy(x => x.IdGoods.Contains(id) && x.Status == false).ToList().LastOrDefault();
                    var e = db.EPCs.OrderBy(x => x.IdGoods.Contains(id) && x.Status == false).ToList().LastOrDefault();
                    e.Status = true;
                    d.Status = true;
                    c.Status = false;
                    db.SaveChanges();
                }
              
             
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PurchaseOrder()
        {
            try
            {
                var c = (from b in db.PurchaseOrders.Where(x => x.Status == false)
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
        public JsonResult Method()
        {
            try
            {
                var c = (from b in db.Methods.Where(x => x.Id > 0)
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
        public JsonResult UserNV()
        {
            try
            {
                var c = (from b in db.Users.Where(x => x.Id.Length > 0)
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
        public JsonResult DetailGoodOrder(int purchaseorder)
        {
            try
            {
                var c = (from b in db.DetailGoodOrders.Where(x => x.IdPurchaseOrder == purchaseorder&&x.Status==true)
                         select new
                         {
                             id = b.Good.Id.Substring(0, b.Good.Id.Length-8),
                             idgood = b.Good.IdGood,
                             name = b.Good.Name,
                             coo = b.Good.Coo.Name,
                         }).ToList();
                var d = (from b in db.PurchaseOrders.Where(x => x.Id == purchaseorder)
                         select new
                         {
                            idpurchaseorder = b.Id,
                            paymethod  =b.PaymentMethod.Name,
                            createdate = b.CreateDate.Value.Day+"/"+b.CreateDate.Value.Month+"/"+b.CreateDate.Value.Year,
                            datepay = b.DatePay.Value.Day + "/" + b.DatePay.Value.Month + "/" + b.DatePay.Value.Year,
                            supplier = b.Supplier.Name,
                            warehouse = b.WareHouse.Name == null ? b.Store.Name : b.WareHouse.Name,
                            sumprice = b.Sumprice,
                             addresssupplier = b.Supplier.AddRess,
                             phonesupplier = b.Supplier.Phone,
                             faxsupplier = b.Supplier.Fax,
                         }
                        ).ToList();
                return Json(new { code = 200, c = c,d=d}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ReceipLast(string id)
        {
            try
            {
                var a = (from b in db.Receipts.Where(x => x.Id == id) select new { id = b.Id, datepn = b.CreateDate.Value.Day+"/"+ b.CreateDate.Value.Month+"/"+ b.CreateDate.Value.Year }).ToList();
                return Json(new { code = 200,a=a}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}