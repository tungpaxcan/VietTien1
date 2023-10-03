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
    public class ColorController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: Color
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
            Color color = db.Colors.Find(id);
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Colors.Where(x => x.Id.Length > 0 && x.Name != null)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             color = b.Color1
                         }).ToList().Where(x => x.name.ToLower().Contains(seach)||x.name.Contains(seach)
                                              ||x.id.ToLower().Contains(seach)||x.id.Contains(seach));
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
        public JsonResult Add(string name, string id,string color,string des,bool status)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = new Color();
                d.Id = id;
                d.Name = name;
                d.Color1 = color;
                d.Description = des;
                d.Status = status;
                d.CreateDate = DateTime.Now;
                d.CreateBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
                d.ModifyBy = nameAdmin;
                db.Colors.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm Màu Sắc Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Thêm Màu Sắc Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string id, string name, string color, string des, bool status)
        {
            try
            {
                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.Colors.Find(id);
                d.Name = name;
                d.Color1 = color;
                d.Description = des;
                d.Status = status;
                d.ModifyBy = nameAdmin;
                d.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa Màu Sắc Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sửa Màu Sắc Thất Bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var d = db.Colors.Find(id);
                Dele.DeleteColors(id);
                return Json(new { code = 200, msg = "Xóa Màu Sắc Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Màu Sắc Thành Công" }, JsonRequestBehavior.AllowGet);
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
                                    var color = workSheet.Cells[rowIterator, 3].Value == null ? null : workSheet.Cells[rowIterator, 3].Value.ToString();
                                    var des = workSheet.Cells[rowIterator, 4].Value == null ? null : workSheet.Cells[rowIterator, 4].Value.ToString();
                                    var checkColor = db.Colors.Find(id);
                                    if (name == null)
                                    {
                                        MessageBox.Show("Chưa Nhập Tên Tại Dòng " + rowIterator);
                                        continue;
                                    }
                                    if (checkColor == null)
                                    {
                                        var session = (User)Session["user"];
                                        var nameAdmin = session.Name;
                                        var d = new Color();
                                        d.Name = name;
                                        d.Id = id;
                                        d.Color1 = color;
                                        d.Description = des;
                                        d.Status = true;
                                        d.CreateDate = DateTime.Now;
                                        d.ModifyDate = DateTime.Now;
                                        d.CreateBy = nameAdmin;
                                        d.ModifyBy = nameAdmin;
                                        db.Colors.Add(d);
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