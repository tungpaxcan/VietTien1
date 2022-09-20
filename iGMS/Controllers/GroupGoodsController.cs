using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class GroupGoodsController : BaseController
    {
        private VietTienEntities db = new VietTienEntities();
        // GET: GroupGoods
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Adds()
        {
            return View();
        }
        public ActionResult Edits(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupGood groupGood = db.GroupGoods.Find(id);
            if (groupGood == null)
            {
                return HttpNotFound();
            }
            return View(groupGood);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.GroupGoods.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             nameindustry = b.Industry.Name,
                             name = b.Name,
                             des = b.Description
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
        public JsonResult Add(int idindustry,string name, string des,string id)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new GroupGood();
                d.Id = id;
                d.Name = name;
                d.IdIndustry = idindustry;
                d.Description = des;
                db.GroupGoods.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult Edit(string id,int idindustry, string name, string des)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.GroupGoods.Find(id);
                d.Name = name;
                d.Description = des;
                d.IdIndustry = idindustry;
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
                var d = db.GroupGoods.Find(id);
                db.GroupGoods.Remove(d);
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
    }
}