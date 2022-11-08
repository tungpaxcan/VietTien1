using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class ReportSaleController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: ReportSale
        public ActionResult ReportSaleTime()
        {
            return View();
        }
        public ActionResult ReportSaleCate()
        {
            return View();
        }
        public ActionResult ReportSaleGroup()
        {
            return View();
        }
        public ActionResult ReportSaleGood()
        {
            return View();
        }
        public ActionResult ReportSaleUser()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ShowTime(int value,DateTime S,DateTime E)
        {
            try
            {
                var d = DateTime.Now;
                var day = d.Day;
                var month = d.Month;
                var year = d.Year;
                var a = (from b in db.Bills.Where(x=>x.CreateDate.Value.Day==day&& x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year)
                         select new
                         {
                             id = b.Id,
                             createdate = b.CreateDate.Value.Day+"/"+b.CreateDate.Value.Month+"/"+b.CreateDate.Value.Year,
                             sumprice = b.TotalMoney
                         }).ToList();
                var e = (from b in db.Bills.Where(x => (x.CreateDate.Value.Day <= day && x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year) &&
                                                       (x.CreateDate.Value.Day >= day - 6 && x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year))
                         select new
                         {
                             id = b.Id,
                             createdate = b.CreateDate.Value.Day + "/" + b.CreateDate.Value.Month + "/" + b.CreateDate.Value.Year,
                             sumprice = b.TotalMoney
                         }).ToList();
                var g = (from b in db.Bills.Where(x => (x.CreateDate.Value.Day <= day && x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year) &&
                                                      (x.CreateDate.Value.Day >= day - 29 && x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year))
                         select new
                         {
                             id = b.Id,
                             createdate = b.CreateDate.Value.Day + "/" + b.CreateDate.Value.Month + "/" + b.CreateDate.Value.Year,
                             sumprice = b.TotalMoney
                         }).ToList();
                var h = (from b in db.Bills.Where(x => (x.CreateDate.Value.Day <= E.Day && x.CreateDate.Value.Month <= E.Month && x.CreateDate.Value.Year <= E.Year) &&
                                                     (x.CreateDate.Value.Day >= S.Day && x.CreateDate.Value.Month >= S.Month  && x.CreateDate.Value.Year >= S.Year))
                         select new
                         {
                             id = b.Id,
                             createdate = b.CreateDate.Value.Day + "/" + b.CreateDate.Value.Month + "/" + b.CreateDate.Value.Year,
                             sumprice = b.TotalMoney
                         }).ToList();
                return Json(new { code = 200,a=a,e=e,g=g,h=h}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Cate(string seach)
        {
            try
            {
                var a = (from b in db.CateGoods.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList().Where(x=>x.name.ToLower().Contains(seach)||x.name.Contains(seach));
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ShowCate(string value)
        {
            try
            {

                var a = (from b in db.DetailBills.Where(x => x.Good.CateGood.Id==value)
                         select new
                         {
                             id = b.Bill.Id,
                             createdate = b.Bill.CreateDate.Value.Day + "/" + b.Bill.CreateDate.Value.Month + "/" + b.Bill.CreateDate.Value.Year,
                             sumprice = b.Bill.TotalMoney
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Group(string seach)
        {
            try
            {
                var a = (from b in db.GroupGoods.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList().Where(x => x.name.ToLower().Contains(seach) || x.name.Contains(seach));
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ShowGroup(string value)
        {
            try
            {

                var a = (from b in db.DetailBills.Where(x => x.Good.GroupGood.Id == value)
                         select new
                         {
                             id = b.Bill.Id,
                             createdate = b.Bill.CreateDate.Value.Day + "/" + b.Bill.CreateDate.Value.Month + "/" + b.Bill.CreateDate.Value.Year,
                             sumprice = b.Bill.TotalMoney
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Good(string seach)
        {
            try
            {
                var a = (from b in db.Goods.Where(x => x.IdGood.ToLower().Contains(seach) || x.IdGood.Contains(seach))
                         select new
                         {
                             id = b.Id,
                             idgood = b.IdGood,
                             name = b.Name
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ShowGood(string value)
        {
            try
            {

                var a = (from b in db.DetailBills.Where(x => x.Good.IdGood == value)
                         select new
                         {
                             id = b.Bill.Id,
                             createdate = b.Bill.CreateDate.Value.Day + "/" + b.Bill.CreateDate.Value.Month + "/" + b.Bill.CreateDate.Value.Year,
                             sumprice = b.Bill.TotalMoney
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Users(string seach)
        {
            try
            {
                var a = (from b in db.Users.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
                         }).ToList().Where(x => x.name.ToLower().Contains(seach) || x.name.Contains(seach));
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ShowUser(string value)
        {
            try
            {

                var a = (from b in db.DetailBills.Where(x => x.Bill.User.Id == value)
                         select new
                         {
                             id = b.Bill.Id,
                             createdate = b.Bill.CreateDate.Value.Day + "/" + b.Bill.CreateDate.Value.Month + "/" + b.Bill.CreateDate.Value.Year,
                             sumprice = b.Bill.TotalMoney
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {

                var a = (from b in db.DetailBills.Where(x => x.IdBill == id)
                         select new
                         {
                             id = b.Bill.Id,
                             idgood = b.Good.IdGood,
                             sumprice = b.SumPrice
                         }).ToList();
                return Json(new { code = 200, a = a, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}