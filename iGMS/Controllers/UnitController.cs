using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using iGMS.Models;
using OfficeOpenXml;

namespace iGMS.Controllers
{
    public class UnitController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: Unit
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
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }
        [HttpGet]
        public JsonResult allUnit()
        {
            try
            {
                var a = (from b in db.Units.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                var groupUnit = (from b in db.GroupUnits.Where(x => x.Id > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, a = a,groupUnit=groupUnit }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult unitToGroup(int idGroupUnit)
        {
            try
            {
                var a = (from b in db.Units.Where(x => x.Id.Length > 0&&x.IdGroupUnit== idGroupUnit)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, a = a }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach,int idGroupUnit)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Units.Where(x => x.Id.Length > 0 && x.IdGroupUnit >0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList().Where(x => x.name.ToLower().Contains(seach) || x.name.Contains(seach)
                                              || x.id.ToLower().Contains(seach) || x.id.Contains(seach));
                if (idGroupUnit != -1)
                {
                     a = (from b in db.Units.Where(x => x.Id.Length > 0 && x.IdGroupUnit == idGroupUnit)
                             select new
                             {
                                 id = b.Id,
                                 name = b.Name,
                             }).ToList().Where(x => x.name.ToLower().Contains(seach) || x.name.Contains(seach)
                                                  || x.id.ToLower().Contains(seach) || x.id.Contains(seach));
                }
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
        public JsonResult Add(string name,string id,int idGroupUnit,string des,bool status)
        {
            try
            {
                var unit = db.Units.Find(id);
                if (unit == null)
                {
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new Unit();
                    d.Name = name;
                    d.Id = id;
                    d.IdGroupUnit = idGroupUnit;
                    d.Description = des;
                    d.Status = status;
                    d.CreateDate = DateTime.Now;
                    d.ModifyDate = DateTime.Now;
                    d.CreateBy = nameAdmin;
                    d.ModifyBy = nameAdmin;
                    db.Units.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Tạo Đơn Vị Thành Công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Mã Đơn Vị Trùng (Đã Có Trong Hệ Thống)" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Tạo Đơn Vị Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string name, string id, int idGroupUnit, string des, bool status)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.Units.Find(id);
                d.Name = name;
                d.IdGroupUnit = idGroupUnit;
                d.Description = des;
                d.Status = status;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa Đơn Vị Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sửa Đơn Vị Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                Dele.DeleteUnits(id);
                return Json(new { code = 200, msg = "Xóa Đơn Vị Thành Công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Đơn Vị Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {

            var units = new List<Unit>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        ExcelWorksheet currentSheet = package.Workbook.Worksheets.First();
                        var workSheet = currentSheet;
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            try
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null)
                                {
                                    var id = workSheet.Cells[rowIterator, 1].Value == null ? null : workSheet.Cells[rowIterator, 1].Value.ToString();
                                    var name = workSheet.Cells[rowIterator, 2].Value == null ? null : workSheet.Cells[rowIterator, 2].Value.ToString();
                                    var idGroupUnit = workSheet.Cells[rowIterator, 3].Value == null ? 0 : int.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                    var des = workSheet.Cells[rowIterator, 4].Value == null ? null : workSheet.Cells[rowIterator, 4].Value.ToString();
                                    var checkUnit = db.Units.Find(id);
                                    var groupUnit = db.GroupUnits.Find(idGroupUnit);
                                    if (name == null)
                                    {
                                        MessageBox.Show("Chưa Nhập Tên Tại Dòng " + rowIterator);
                                        continue;
                                    }
                                    if (groupUnit == null)
                                    {
                                        MessageBox.Show("Nhập Mã Nhóm Đơn Vị Sai Hoặc Chưa Nhập Tại Dòng " + rowIterator);
                                        continue;
                                    }
                                    if (checkUnit == null)
                                    {
                                        var session = (User)Session["user"];
                                        var nameAdmin = session.Name;
                                        var d = new Unit();
                                        d.Name = name;
                                        d.Id = id;
                                        d.IdGroupUnit = idGroupUnit;
                                        d.Description = des;
                                        d.Status = true;
                                        d.CreateDate = DateTime.Now;
                                        d.ModifyDate = DateTime.Now;
                                        d.CreateBy = nameAdmin;
                                        d.ModifyBy = nameAdmin;
                                        db.Units.Add(d);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Trùng " + id + "(Đã Có " + id + " Trong Hệ Thống) Tại Dòng " + rowIterator);
                                    }
                                }
                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var error in ex.EntityValidationErrors)
                                {
                                    foreach (var validationError in error.ValidationErrors)
                                    {
                                        Console.WriteLine("Lỗi xác thực: {0}", validationError.ErrorMessage);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return View("Index");
        }
    }
}