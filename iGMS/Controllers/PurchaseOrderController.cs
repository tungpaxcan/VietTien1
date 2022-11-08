using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using iGMS.Models;
using OfficeOpenXml;

namespace iGMS.Controllers
{
    public class PurchaseOrderController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListImport()
        {
            try
            {
                var c = (from b in db.PurchaseOrders.Where(x => x.Status == true && x.TypeStatu.Name== "Order")
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, Count_Import = c.Count(),List_Import=c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ListExport()
        {
            try
            {
                var c = (from b in db.PurchaseOrders.Where(x => x.Status == false && x.TypeStatu.Name == "Transfer")
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                var Sale_Orders = (from b in db.SalesOrders.Where(x => x.Status == true)
                                  select new
                                  {
                                      id = b.Id,
                                      name = b.Name
                                  }).ToList();
                var Count_Export = c.Count() + Sale_Orders.Count();
                return Json(new { code = 200, Count_Export = Count_Export,c=c, Sale_Orders= Sale_Orders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
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
                d.IdTypeStatus = 1;
                d.IdSupplier = supplier ;
                d.Status = true;
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
        public JsonResult AddDetail(float price,float sumpricegoods,string epc,string goods,string H)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var idpurchaseorder = db.PurchaseOrders.OrderBy(x => x.Id);
                var idpu = idpurchaseorder.ToList().LastOrDefault();
                var good = db.Goods.OrderBy(x => x.IdGood.Replace(".","")==goods).ToList().LastOrDefault();
                var id = idpu.Id;
                var e = new Good();
                var epcs = Encode.EPC(epc);
                var ea = db.Goods.Find(epc);
                if(ea == null)
                {
                    e.Id = epc;
                    e.IdGood = good.IdGood;
                    e.IdWareHouse = good.IdWareHouse;
                    e.Material = good.Material;
                    e.IdSeason = good.IdSeason;
                    e.IdColor = good.IdColor;
                    e.IdSize = good.IdSize;
                    e.IdStyle = good.IdStyle;
                    e.IdCate = good.IdCate;
                    e.IdGender = good.IdGender;
                    e.IdGroupGood = good.IdGroupGood;
                    e.SKU = good.SKU;
                    e.IdGender = good.IdGender;
                    e.Company = good.Company;
                    e.Name = good.Name;
                    e.IdCoo = good.IdCoo;
                    e.Price = price;
                    e.PriceNew = price;
                    db.Goods.Add(e);
                    db.SaveChanges();
                }
            
                var g = new EPC();
                var ga = db.EPCs.SingleOrDefault(x => x.IdGoods == epc);
                if (ga == null)
                {
                    g.IdGoods = epc;
                    g.IdEPC = epcs;
                    g.Status = false;
                    db.EPCs.Add(g);
                    db.SaveChanges();
                }              
                var f = new DetailWareHouse();
                var fa = db.DetailWareHouses.SingleOrDefault(x => x.IdGoods == epc);
                if (fa == null)
                {
                    if (H.Contains("CH"))
                    {
                        f.IdStore = H;
                    }
                    else
                    {
                        f.IdWareHouse = H;
                    }
                    f.IdGoods = epc;
                    f.Inventory = 1;
                    f.IdWareHouse = H;
                    f.Status = false;
                    f.StatusWait = true;
                    db.DetailWareHouses.Add(f);
                    db.SaveChanges();
                }
            
                var d = new DetailGoodOrder();
                var da = db.DetailGoodOrders.SingleOrDefault(x => x.IdGoods == epc);
                if (da == null)
                {
                    d.EPC = epcs;
                    d.IdGoods = epc;
                    d.Price = price;
                    d.IdPurchaseOrder = id;
                    d.Sumprice = sumpricegoods;
                    d.Description = "";
                    d.CreateDate = DateTime.Now;
                    d.CreateBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.Status = true;
                    db.DetailGoodOrders.Add(d);
                    db.SaveChanges();
                }
            
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
                             id = b.IdGoods,
                             idgood = b.Good.IdGood.Replace(".", ""),
                             epc = b.EPC,
                             name = b.Good.Name,
                             size = b.Good.Size.Name,
                             price = b.Price,
                             sumprice = b.Sumprice
                         }).ToList();
                var d = (from b in db.DetailTransferOrders.Where(x => x.IdPuchaseOrder == id)
                         join bb in db.Goods on b.IdGood.Replace(".", "") equals bb.IdGood.Replace(".", "")
                         select new
                         {
                             id = b.IdGoods,
                             idgood = b.IdGood.Replace(".", ""),
                             name = bb.Name,
                             amount = b.Amount,
                         }).ToList();
                return Json(new { code = 200, c = c,d=d }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ListGoods(string supplier, string seach)
        {
            try
            {
                var c = (from b in db.DetailSupplierGoods.Where(x => x.IdSupplier == supplier)
                         select new
                         {
                             id = b.Good.Id,
                             idgood = b.Good.IdGood.Replace(".", ""),
                             name = b.Good.Name,
                             size = b.Good.Size.Name,
                             purchaseprice = b.PurchasePrice == null ? 0 : b.PurchasePrice,
                             purchasediscount = b.PurchaseDiscount == null ? 0 : b.PurchaseDiscount,
                             purchasetax = b.PurchaseTax == null ? 0 : b.PurchaseTax
                         }).ToList().Where(x => x.idgood.Contains(seach)||x.idgood.ToLower().Contains(seach));
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ListGoods1(string warehouse, string seach)
        {
            try
            {
                var c = (from b in db.DetailWareHouses.Where(x => x.WareHouse.Id == warehouse&&x.Status==true&&x.StatusWait==true)
                         select new
                         {
                             id = b.Good.Id,
                             idgood = b.Good.IdGood.Replace(".", ""),
                             name = b.Good.Name,
                         }).ToList().Where(x => x.idgood.Contains(seach) || x.idgood.ToLower().Contains(seach));
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
        public JsonResult ValidateAmount(string warehouse,string idgood )
        {
            try
            {
                var c = db.DetailWareHouses.Where(x => x.IdWareHouse == warehouse && x.Good.IdGood.Replace(".", "") == idgood&&x.StatusWait==true&&x.Status==true).ToList();
                return Json(new { code = 200, c = c.Count, }, JsonRequestBehavior.AllowGet);
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
                var c = (from b in db.EPCs.Where(x => x.Status == true)
                         select new
                         {
                             epc = b.IdEPC
                         }).ToList();
                var d = (from b in db.DetailGoodOrders.Where(x => x.Status==true)
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
        [HttpPost]
        public JsonResult Add1(string name,string des,string H)
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
                d.IdTypeStatus = 2;
                d.Name = name;
                d.Status = false;

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
        public JsonResult AddDetail1( string goods, string H,int amount)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var idpurchaseorder = db.PurchaseOrders.OrderBy(x => x.IdTypeStatus==2);
                var idpu = idpurchaseorder.ToList().LastOrDefault();
                var good = db.Goods.OrderBy(x => x.IdGood.Replace(".", "") == goods).ToList().LastOrDefault();
                var id = idpu.Id;
                var d = new DetailTransferOrder();
                var da = db.DetailTransferOrders.SingleOrDefault(x => x.Good.IdGood.Replace(".", "") == goods);
                for(int i = 0; i < amount; i++)
                {
                    var dewa = db.DetailWareHouses.OrderBy(x => x.Good.IdGood.Replace(".", "") == goods&&x.StatusWait==true).ToList().LastOrDefault();
                    dewa.StatusWait = false;
                    db.SaveChanges();
                }              
                if (da == null)
                {
                    d.IdPuchaseOrder = id;
                    d.Amount = amount;
                    d.IdGood = goods;
                    d.Description = "";
                    d.CreateDate = DateTime.Now;
                    d.CreateBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.StatusExport = false;
                    d.Status = true;
                    db.DetailTransferOrders.Add(d);
                    db.SaveChanges();
                }

                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}