using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class StallsController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: Stalls
        public ActionResult Index(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }
        public ActionResult Adds(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        } 
        public ActionResult Edits(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stall stall = db.Stalls.Find(id);
            if (stall == null)
            {
                return HttpNotFound();
            }
            return View(stall);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach , string id)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Stalls.Where(x => x.IdStore == id)
                         select new
                         {
                             id = b.Id,
                             name = b.Name
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
        public JsonResult Add(string id, string name,string idstore)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var ids = db.Stalls.Where(x => x.Id == id).ToList();
                if (ids.Count == 0)
                {
                    var d = new Stall();
                    d.Id = id;
                    d.Name = name;
                    d.IdStore = idstore;
                    db.Stalls.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Trùng Mã QB" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string id, string name, string idstore)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                    var d = db.Stalls.Find(id);
                    d.Id = id;
                    d.Name = name;
                    d.IdStore = idstore;
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
                var d = db.Stalls.Find(id);
                db.Stalls.Remove(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}