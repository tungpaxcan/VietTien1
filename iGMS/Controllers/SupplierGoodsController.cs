using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;
namespace iGMS.Controllers
{
    public class SupplierGoodsController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: SupplierGoods
        public ActionResult Index(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        public ActionResult Edits(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailSupplierGood detailSupplierGood = db.DetailSupplierGoods.Find(id);
            if (detailSupplierGood == null)
            {
                return HttpNotFound();
            }
            return View(detailSupplierGood);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach,string idsupplier)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.DetailSupplierGoods.Where(x => x.IdSupplier == idsupplier)
                         select new
                         {
                             id = b.Good.Id,
                             name = b.Good.Name,
                             unit = b.Good.Unit.Name,
                             purchaseprice = b.PurchasePrice,
                             purchasetax = b.PurchaseTax,
                             price = b.Good.Price,
                             pricetax = b.Good.PriceTax
                         }).ToList().Where(x => x.name.ToLower().Contains(seach));
                var pages = a.Count() % pageSize == 0 ? a.Count() / pageSize : a.Count() / pageSize + 1;
                var c = a.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var count = a.Count();
                return Json(new { code = 200, c = c, pages = pages, count = count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(string id, string supplier)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                    var d = new DetailSupplierGood();
                    d.IdGoods = id;
                    d.IdSupplier= supplier;
                    db.DetailSupplierGoods.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var a =(from i in db.DetailSupplierGoods.Where(x => x.IdGoods == id)
                        select new {id=i.Id})
                        .ToList();
                for(int  i = 0; i < a.Count(); i++)
                {
                    var b = a[i];
                    var e = db.DetailSupplierGoods.SingleOrDefault(x => x.Id==b.id);
                    db.DetailSupplierGoods.Remove(e);
                    db.SaveChanges();
                }
             

                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}