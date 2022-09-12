using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class HomeController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult WareHouse()
        {
            try
            {
                var a = (WareHouse)Session["WareHouse"];
                var id = a == null ? "-1" : a.Id;
                var name = a == null ? "Chọn Kho Hàng" : a.Name;
                var c = (from b in db.WareHouses.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, c = c,id= id,name = name}, JsonRequestBehavior.AllowGet);
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
                var a = (Store)Session["Store"];
                var id = a == null ? "-1" : a.Id;
                var name = a == null ? "Chọn Cửa Hàng" : a.Name;
                var c = (from b in db.Stores.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, c = c, id =id ,name =name}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Stalls(string idstore)
        {
            try
            {
                var a = (Stall)Session["Stalls"];
                var id = a == null ? "1" : a.Id;
                var name = a == null ? "Chọn Quầy Bán" : a.Name;
                var c = (from b in db.Stalls.Where(x => x.IdStore == idstore)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, c = c,id =id,name=name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult SaveSetting(string WareHouses, string Store,string Stalls)
        {
            try
            {
                var a = db.WareHouses.SingleOrDefault(x=>x.Id==WareHouses);
                var b = db.Stores.SingleOrDefault(x => x.Id == Store);
                var c = db.Stalls.SingleOrDefault(x => x.Id == Stalls);
                if (a != null||b!=null||c!=null)
                {
                    Session["WareHouse"] = a;
                    Session["Store"] = b;
                    Session["Stalls"] = c;
                    return Json(new { code = 200,a=a.Name,b=b.Name,c=c.Name, Url = "/Home/Index" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Tài Khoản Hoặc Mật Khẩu không Đúng !!!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult UpLoadft()
        {
            try
            {
                var a = (WareHouse)Session["WareHouse"];
                var namewarehouse = a == null ? "Không Có" : a.Name;
                var b = (Store)Session["Store"];
                var namestore = b == null ? "Không Có" : b.Name;
                var c = (Stall)Session["Stalls"];
                var namestalls = c == null ? "Không Có" : c.Name;
                var d = (User)Session["user"];
                var iduser = d.Id;
                return Json(new { code = 200,namewarehouse= namewarehouse, namestore= namestore, namestalls = namestalls, iduser= iduser }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Goods(string id)
        {
            try
            {
                var c = (from b in db.Goods.Where(x => x.Id == id)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             unit = b.Unit.Name,
                             price = b.Price,
                             discount = b.Discount,
                             categoods = b.CateGood.Name
                         }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult UserTV(string id)
        {
            try
            {
                var c = (from b in db.Users.Where(x => x.Id == id)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }      
        [HttpGet]
        public JsonResult Customer(string id)
        {
            try
            {
                var c = (from b in db.Customers.Where(x => x.Id == id)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             point = b.Point == null ? 0 : b.Point
                         }).ToList();
                return Json(new { code = 200, c = c }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult UserSession()
        {
            try
            {
                var c = (User)Session["user"];
                return Json(new { code = 200, name = c.Name,id=c.Id}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddBill(string idcustomer,string sumprice)
            {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var stalls = (Stall)Session["Stalls"];
                var store = (Store)Session["Store"];
                var d = new Bill();
                if (DateTime.Now.Hour < 14 && DateTime.Now.Hour >= 6)
                {
                    d.IdNumShift = 1;
                }else if (DateTime.Now.Hour <= 22 && DateTime.Now.Hour >= 14)
                {
                    d.IdNumShift = 2;
                }
                else
                {
                    d.IdNumShift = 3;
                }
                d.Status = true;
                d.HangBill = true;
                d.IdStall = stalls.Id;
                d.IdStore = store.Id;
                d.IdUsers = session.Id;
                d.IdCustomer = idcustomer == ""?null:idcustomer;
                d.Point = float.Parse(sumprice) / 100000;
                d.TotalMoney = float.Parse(sumprice);
                d.CreateDate = DateTime.Now;
                db.Bills.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        } [HttpPost]
        public JsonResult Refunds(string refunds)
            {
            try
            {
                var idbill = db.Bills.OrderBy(x => x.Id).ToList().LastOrDefault();
                var c = db.Bills.Find(idbill.Id);
                c.Refunds = float.Parse(refunds);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddDetailBill(string idgoods,string amounts,string price,string discount,string totalmoney)
        {
            try
            {
                var store = (Store)Session["Store"];
                var e = db.DetailWareHouses.SingleOrDefault(x => x.IdStore == store.Id && x.IdGoods == idgoods);
                e.Inventory -= float.Parse(amounts);
                var idbill = db.Bills.OrderBy(x => x.Id);
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new DetailBill();
                d.IdGoods = idgoods;
                d.Bill = idbill.ToList().LastOrDefault();
                d.Amount = int.Parse(amounts);
                d.Price = float.Parse(price);
                d.Discount = float.Parse(discount);
                d.SumPrice = float.Parse(totalmoney);
                db.DetailBills.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult BILL()
        {
            try
            {
                 var idbill = db.Bills.OrderBy(x => x.Id);
                var idlast = idbill.ToList().LastOrDefault();
                var d = (from b in db.DetailBills.Where(x => x.IdBill == idlast.Id)
                         select new
                         {
                             namegoods = b.Good.Name,
                             amount = b.Amount,
                             discount = b.Discount,
                             price = b.Price,
                             totalmoney = b.SumPrice
                         }).ToList();
                var c = (from b in db.Bills.Where(x=>x.Id == idlast.Id &&x.Status==true)
                         select new
                         {
                             id=b.Id,
                             store = b.Store.Name,
                             address = b.Store.AddRess,
                             idbill = b.Id,
                             stalls = b.Stall.Name,
                             date = b.CreateDate.Value.Day + "/" + b.CreateDate.Value.Month + "/" + b.CreateDate.Value.Year,
                             time = b.CreateDate.Value.Hour + ":" + b.CreateDate.Value.Minute + ":" + b.CreateDate.Value.Second,
                             userTN = b.User.Name,
                             customer = b.Customer.Name + " - " + b.Customer.Id,
                             point = b.Point,
                             totalmoney = b.TotalMoney,
                             refunds = b.Refunds,
                         }).ToList();
             
                return Json(new { code = 200, c = c,d=d }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Shift()
        {
            try
            {
                var d = DateTime.Now;
                var ca = "1";
                if (d.Hour >= 6 && d.Hour < 14)
                {
                    ca = "1";
                }else if(d.Hour >= 14 &&d.Hour <= 22)
                {
                    ca = "2";
                }
                else
                {
                    ca = "3";
                }
                var e = (User)Session["user"];
                var nv = e.Name;
                var c = (from b in db.NumberShifts.Where(x => x.Id > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList();

                return Json(new { code = 200, c = c,ca=ca, nv= nv }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddReportShift(string tiendauca,string caban)
        {
            try
            {
                var d = new ReportFirstShift();
                var nv = (User)Session["user"];
                var stall = (Stall)Session["Stalls"];
                var store = (Store)Session["Store"];

                d.IdUser = nv.Id;
                d.IdStall = stall.Id;
                d.IdStore = store.Id;
                d.IdNumShift = int.Parse(caban);
                d.FirstShiftMoney = float.Parse(tiendauca);
                d.CreateDate = DateTime.Now;
                d.ModifyDate = DateTime.Now;
                db.ReportFirstShifts.Add(d);
                db.SaveChanges();
                return Json(new { code = 200,  }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PrintReportFirstShift()
        {
            try
            {
                var a = db.ReportFirstShifts.OrderBy(x => x.Id).ToList().LastOrDefault();
                var b = (from c in db.ReportFirstShifts.Where(x => x.Id == a.Id)
                         select new
                         {
                             store = c.Store.Name,
                             address = c.Store.AddRess,
                             stall = c.Stall.Name,
                             date = c.CreateDate.Value.Day + "/" + c.CreateDate.Value.Month + "/" + c.CreateDate.Value.Year,
                             shift = c.NumberShift.Name,
                             nv = c.User.Name,
                             moneyfirst = c.FirstShiftMoney,
                             id = c.Id
                         }).ToList();

                return Json(new { code = 200,b=b }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult First(int seri)
        {
            try
            {
                var b = (from c in db.ReportFirstShifts.Where(x => x.Id == seri)
                         select new
                         {
                             store = c.Store.Name,
                             address = c.Store.AddRess,
                             stall = c.Stall.Name,
                             date = c.CreateDate.Value.Day + "/" + c.CreateDate.Value.Month + "/" + c.CreateDate.Value.Year,
                             shift = c.NumberShift.Name,
                             nv = c.User.Name,
                             moneyfirst = c.FirstShiftMoney,
                             id = c.Id
                         }).ToList();

                return Json(new { code = 200, b = b }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ReportBill(int seri)
        {
            try
            {
                var a = db.ReportFirstShifts.Find(seri);
                var nv = (User)Session["user"];
                var stall = (Stall)Session["Stalls"];
                var store = (Store)Session["Store"];
                var b = (from c in db.Bills.Where(x => x.IdStall == stall.Id &&x.IdStore==store.Id&&x.IdUsers==nv.Id
                                                   &&x.CreateDate.Value.Day==a.CreateDate.Value.Day 
                                                   &&x.CreateDate.Value.Month==a.CreateDate.Value.Month 
                                                   &&x.CreateDate.Value.Year==a.CreateDate.Value.Year
                                                   &&x.IdNumShift == a.IdNumShift&&x.Status==true && x.HangBill == true)
                         select new
                         {
                             id= c.Id,
                             sumprice = c.TotalMoney
                         }).ToList();

                return Json(new { code = 200, b = b }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddReportEndShift(int seri,string tiencuoica, string tienthucban)
        {
            try
            {
                var d = new ReportlEndShift();
                var nv = (User)Session["user"];
                var stall = (Stall)Session["Stalls"];
                var store = (Store)Session["Store"];
                var a = db.ReportFirstShifts.SingleOrDefault(x => x.Id == seri);
                d.IdFirst = seri;
                d.IdUser = nv.Id;
                d.IdStall = stall.Id;
                d.IdStore = store.Id;
                d.IdNumShift = a.NumberShift.Id;
                d.FirstShiftMoney = a.FirstShiftMoney;
                d.EndShiftMoney1 = int.Parse(tiencuoica);
                d.RealMoney = int.Parse(tienthucban);
                d.CreateDate = DateTime.Now;
                d.ModifyDate = DateTime.Now;
                db.ReportlEndShifts.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PrintReportEndShift()
        {
            try
            {
                var a = db.ReportlEndShifts.OrderBy(x => x.Id).ToList().LastOrDefault();
                var b = (from c in db.ReportlEndShifts.Where(x => x.Id == a.Id)
                         select new
                         {
                             store = c.Store.Name,
                             address = c.Store.AddRess,
                             stall = c.Stall.Name,
                             date = c.CreateDate.Value.Day + "/" + c.CreateDate.Value.Month + "/" + c.CreateDate.Value.Year,
                             shift = c.NumberShift.Name,
                             nv = c.User.Name,
                             moneyfirst = c.FirstShiftMoney,
                             id = c.Id,
                             moneyend =  c.EndShiftMoney1,
                             realmoney = c.RealMoney
                         }).ToList();
                var d = (from c in db.Bills.Where(x => x.IdStall == a.Stall.Id && x.IdStore == a.Store.Id && x.IdUsers == a.User.Id
                                                 && x.CreateDate.Value.Day == a.CreateDate.Value.Day
                                                 && x.CreateDate.Value.Month == a.CreateDate.Value.Month
                                                 && x.CreateDate.Value.Year == a.CreateDate.Value.Year
                                                 && x.IdNumShift == a.IdNumShift &&x.Status == true&&x.HangBill==true)
                         select new
                         {
                             id = c.Id,
                             sumprice = c.TotalMoney
                         }).ToList();
                return Json(new { code = 200, b = b,d=d }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult HangBill(string deshangbill)
        {
            try
            {
                var a = db.Bills.OrderBy(x => x.Id).ToList().LastOrDefault();
                var b = db.Bills.Find(a.Id);
                b.HangBill = false;
                b.Description = deshangbill;
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteBill()
        {
            try
            {
                var a = db.Bills.OrderBy(x => x.Id).ToList().LastOrDefault();
                var b = db.Bills.SingleOrDefault(x=>x.Id==a.Id);
                var c = db.DetailBills.Where(x => x.IdBill == a.Id).ToList();
                for(int i = 0; i < c.Count(); i++)
                {
                    var d = db.DetailBills.Find(c[i].Id);
                    db.DetailBills.Remove(d);
                    db.SaveChanges();
                }
                db.Bills.Remove(b);
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult OpenImpresora()
        {
            try
            {
                Process.Start("plugin_impresora_termica_64_bits.exe");
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult AllHangBill(string seachhoadontreo)
        {
            try
            {
                var a = (from b in db.Bills.Where(x => x.HangBill == false)
                         select new
                         {
                             id = b.Id,
                             des = b.Description,
                         }).ToList().Where(x=>x.des.ToLower().Contains(seachhoadontreo)|| x.des.Contains(seachhoadontreo));
                return Json(new { code = 200,a=a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DetailHangBill(int id)
        {
            try
            {
                var a = (from b in db.DetailBills.Where(x => x.IdBill == id)
                         select new
                         {
                             id = b.Id,
                             idgoods = b.IdGoods
                         }).ToList();
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteHangBill(int id)
        {
            try
            {
                var c = db.DetailBills.Where(x => x.IdBill == id).ToList();
                for (int i = 0; i < c.Count(); i++)
                {
                    var d = db.DetailBills.Find(c[i].Id);
                    db.DetailBills.Remove(d);
                    db.SaveChanges();
                }
                var e = db.Bills.Find(id);
                db.Bills.Remove(e);
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}