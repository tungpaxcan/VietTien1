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
        private iPOSEntities db = new iPOSEntities();
        // GET: Receipt
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Session_Id_Purchase(string id)
        {
            try
            {
                Session["Id_Purchase"] = id;
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Session_Id_Sales(string id)
        {
            try
            {
                Session["Id_Sales"] = id;
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Ready_Session()
        {
            try
            {
                 var Id_Purchase = Session["Id_Purchase"];
                var Id_Sales = Session["Id_Sales"];
                return Json(new { code = 200, Id_Purchase= Id_Purchase, Id_Sales= Id_Sales }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ChangeGood(string id)
        {
            try
            {
                var c = (from b in db.Goods.Where(x => x.Id == id)
                         select new
                         {
                             id = b.Id,
                             idgood = b.IdGood.Replace(".",""),
                             name = b.Name,
                             size = b.Size.Name,
                             price = b.Price,
                             discount = b.Discount,
                             categoods = b.CateGood.Name
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(string id,int purchaseorder,string user1,string user2,string des,int method)
        {
            try
            {

                var session = (User)Session["user"];
                var nameAdmin = session.Name;
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
                    d.Status = true;
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
        public JsonResult DaNhan(string id, string amounttext, int purchaseorder, string[] epcshow)
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
                for (int i = 0; i < epcshow.Length; i++)
                {
                    var idepc = epcshow[i];
                    var f = db.EPCs.SingleOrDefault(x => x.IdGoods == idepc);
                    if (f != null)
                    {
                        var idf = f.IdGoods;
                        var c = db.DetailGoodOrders.SingleOrDefault(x => x.IdGoods == idf && x.IdPurchaseOrder == purchaseorder && x.Status == true);
                        var d = db.DetailWareHouses.SingleOrDefault(x => x.IdGoods == idf && x.Status == false);
                        var e = db.EPCs.SingleOrDefault(x => x.IdGoods == idf && x.Status == false);
                        e.Status = true;
                        d.Status = true;
                        c.Status = false;
                        db.SaveChanges();
                    }
                }
                var Status_Detail_GoodOrder = db.DetailGoodOrders.Where(x => x.Status == true && x.IdPurchaseOrder == purchaseorder).ToList();
                if (Status_Detail_GoodOrder.Count() == 0)
                {
                    db.PurchaseOrders.Find(purchaseorder).Status = false;
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

                object c = null;
                var type_Statuses = db.PurchaseOrders.Find(purchaseorder);
                var Id_Type_Status = type_Statuses.TypeStatu.Name;
                if(Id_Type_Status== "Order")
                {
                    c = (from b in db.DetailGoodOrders.Where(x => x.IdPurchaseOrder == purchaseorder && x.Status == true)
                             select new
                             {
                                 id = b.Good.Id,
                                 idgood = b.Good.IdGood.Replace(".",""),
                                 name = b.Good.Name,
                                 coo = b.Good.Coo.Name,
                             }).ToList();
                }else if(Id_Type_Status== "Transfer")
                {
                     c = (from b in db.DetailTransferOrders.Where(x => x.IdPuchaseOrder == purchaseorder && x.Status == true&&x.StatusExport==true)
                             select new
                             {
                                 id = b.Good.Id,
                                 idgood = b.Good.IdGood.Replace(".", ""),
                                 name = b.Good.Name,
                                 coo = b.Good.Coo.Name,
                             }).ToList();
                }             
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