using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class CustomerController : BaseController
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        } public ActionResult Indexs()
        {
            return View();
        }
        public ActionResult Adds()
        {
            return View();
        }  public ActionResult Addss()
        {
            return View();
        }
        public ActionResult Edits(string id)
        {
            if (id.Length <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        } 
        public ActionResult Editss(string id)
        {
            if (id.Length <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        public ActionResult Details(string id)
        {
            if (id.Length <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }  
        public ActionResult Detailss(string id)
        {
            if (id.Length <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Customers.Where(x => x.Id.Contains("DN"))
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             address = b.AddRess,
                             taxcode = b.TaxCode,
                             phone = b.Phone,
                             fax = b.Fax
                         }).ToList().Where(x => x.id.ToLower().Contains(seach) || x.name.ToLower().Contains(seach));
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
        [HttpGet]
        public JsonResult Lists(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Customers.Where(x => x.Id.Contains("CN"))
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             address = b.AddRess,
                             taxcode = b.TaxCode,
                             phone = b.Phone,
                             fax = b.Fax
                         }).ToList().Where(x => x.id.ToLower().Contains(seach) || x.name.ToLower().Contains(seach));
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
        public JsonResult Add(string id,string name,string address,string nametransaction,int taxcode, string fax, string phone,string email,
            string represent,string position,string website,int stk,string bank,string groupgoods,float debtfrom,float debtto)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ids = db.Customers.Where(x => x.Id == id).ToList();
                if (ids.Count == 0)
                {
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new Customer();
                    d.Id = id;
                    d.Name = name;
                    d.AddRess = address;
                    d.NameTransaction = nametransaction;
                    d.TaxCode = taxcode;
                    d.Fax = fax;
                    d.Phone = phone;
                    d.Email = email;
                    d.Represent = represent;
                    d.Position = position;
                    d.Website = website;
                    d.STK = stk;
                    d.Bank = bank;
                    d.IdGroupGoods = groupgoods;
                    d.DebtFrom = debtfrom;
                    d.DebtTo = debtto;
                    d.CreateBy = nameAdmin;
                    d.CreateDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    db.Customers.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Trùng Mã KH !!!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string id, string name, string address, string nametransaction, int taxcode, string fax, string phone, string email,
     string represent, string position, string website, int stk, string bank, string groupgoods, float debtfrom, float debtto)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = db.Customers.Find(id);
                    d.Name = name;
                    d.AddRess = address;
                    d.NameTransaction = nametransaction;
                    d.TaxCode = taxcode;
                    d.Fax = fax;
                    d.Phone = phone;
                    d.Email = email;
                    d.Represent = represent;
                    d.Position = position;
                    d.Website = website;
                    d.STK = stk;
                    d.Bank = bank;
                    d.IdGroupGoods = groupgoods;
                    d.DebtFrom = debtfrom;
                    d.DebtTo = debtto;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { code = 200}, JsonRequestBehavior.AllowGet);
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
                var d = db.Customers.Find(id);
                db.Customers.Remove(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Industry()
        {
            try
            {
                var c = (from b in db.Industries.Where(x => x.Id > 0)
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
        public JsonResult GroupGoods(int idindustry)
        {
            try
            {
                var c = (from b in db.GroupGoods.Where(x => x.IdIndustry == idindustry)
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