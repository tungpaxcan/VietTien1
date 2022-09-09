using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class SupplierController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Adds()
        {
            return View();
        }
        public ActionResult Edits(string id)
        {
            if (id.Length <= 0)
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
        public ActionResult Details(string id)
        {
            if (id.Length <= 0)
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
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Suppliers.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             address= b.AddRess
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
        public JsonResult Add(string id,string name,string nametransaction,string address,string represent,string position,
            int taxcode,string phone,string fax,string email,string website,int stk,string bank,int groupgoods,string des)
        {
            try
            {
                var idNCC = "NCC" + id;
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new Supplier();
                var ids = db.Suppliers.Where(x => x.Id == idNCC).ToList();
                if (ids.Count == 0)
                {
                    d.Id = idNCC;
                    d.Name = name;
                    d.NameTransaction = nametransaction;
                    d.AddRess = address;
                    d.Represent = represent;
                    d.Position = position;
                    d.TaxCode = taxcode;
                    d.Phone = phone;
                    d.Fax = fax;
                    d.Email = email;
                    d.Website = website;
                    d.STK = stk;
                    d.Bank = bank;
                    d.IdGroupGoods = groupgoods;
                    d.Description = des;
                    d.CreateBy = nameAdmin;
                    d.CreateDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    db.Suppliers.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Mã NCC Trùng !!!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string id, string name, string nametransaction, string address, string represent, string position,
    int taxcode, string phone, string fax, string email, string website, int stk, string bank, int groupgoods, string des)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.Suppliers.Find(id);
                d.Name = name;
                d.NameTransaction = nametransaction;
                d.AddRess = address;
                d.Represent = represent;
                d.Position = position;
                d.TaxCode = taxcode;
                d.Phone = phone;
                d.Fax = fax;
                d.Email = email;
                d.Website = website;
                d.STK = stk;
                d.Bank = bank;
                d.IdGroupGoods = groupgoods;
                d.Description = des;
                d.ModifyBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
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
                var d = db.Suppliers.Find(id);
                db.Suppliers.Remove(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GroupGoods(int idindustry)
        {
            try
            {
                var c = (from b in db.GroupGoods.Where(x => x.IdIndustry==idindustry)
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
    }
}