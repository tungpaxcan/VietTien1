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
    public class NatureController : BaseController
    {
        // GET: Nature
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
            Nature nature = db.Natures.Find(id);
            if (nature == null)
            {
                return HttpNotFound();
            }
            return View(nature);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Natures.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList().Where(x => x.name.ToLower().Contains(seach) || x.name.Contains(seach)
                                              || x.id.ToLower().Contains(seach) || x.id.Contains(seach));
                var pages = a.Count() % pageSize == 0 ? a.Count() / pageSize : a.Count() / pageSize + 1;
                var c = a.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var count = a.Count();
                return Json(new { code = 200, c = c, pages = pages, count = count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiển Thị Thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Nature()
        {
            try
            {
                var a = (from b in db.Natures.Where(x => x.Id.Length > 0&& x.Status ==true)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                         }).ToList();
                return Json(new { code = 200, a = a,}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiển Thị Thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Add(string name, string id, string des, bool status)
        {
            try
            {
                var nature = db.Natures.Find(id);
                if (nature == null)
                {
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new Nature();
                    d.Name = name;
                    d.Id = id;
                    d.Description = des;
                    d.Status = status;
                    d.CreateDate = DateTime.Now;
                    d.ModifyDate = DateTime.Now;
                    d.CreateBy = nameAdmin;
                    d.ModifyBy = nameAdmin;
                    db.Natures.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Tạo Tính Chất Thành Công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Mã Tính Chất Trùng (Đã Có Trong Hệ Thống)" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Tạo Tính Chất Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string name, string id, string des, bool status)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.Natures.Find(id);
                d.Name = name;
                d.Description = des;
                d.Status = status;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa Tính Chất Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sửa Tính Chất Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                Dele.DeleteNatures(id);
                return Json(new { code = 200, msg = "Xóa Tính Chất Thành Công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Tính Chất Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
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
                                    var des = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString();
                                    var checkNature = db.Natures.Find(id);

                                    if (name == null)
                                    {
                                        MessageBox.Show("Chưa Nhập Tên Tại Dòng " + rowIterator);
                                        continue;
                                    }
                                    if (checkNature == null)
                                    {
                                        var session = (User)Session["user"];
                                        var nameAdmin = session.Name;
                                        var d = new Nature();
                                        d.Name = name;
                                        d.Id = id;
                                        d.Description = des;
                                        d.Status = true;
                                        d.CreateDate = DateTime.Now;
                                        d.ModifyDate = DateTime.Now;
                                        d.CreateBy = nameAdmin;
                                        d.ModifyBy = nameAdmin;
                                        db.Natures.Add(d);
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