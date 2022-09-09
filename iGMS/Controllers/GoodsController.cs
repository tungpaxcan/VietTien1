using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;
using OfficeOpenXml;

namespace iGMS.Controllers
{
    public class GoodsController : BaseController
    {
        private iGMSEntities db = new iGMSEntities();
        // GET: Goods
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
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }
        public ActionResult Details(string id)
        {
            if (id.Length <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }
        [HttpGet]
        public JsonResult List(int pagenum, int page, string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Goods.Where(x => x.Id.Length > 0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             unit = b.Unit.Name,
                             price = b.Price,
                             pricetax = b.PriceTax
                         }).ToList().Where(x =>x.id.ToLower().Contains(seach)||x.name.ToLower().Contains(seach));
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
        public JsonResult Add(string id,string categoods,string name,float price,float pricetax,
            float internalprice,float gtgtinternaltax,float discount,float internaldiscount,
            DateTime expiry,DateTime warrantyperiod,float minimuminventory,float maximuminventory,string des,string unit,
            string material, string season, string color, string size)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ids = db.Goods.Where(x => x.Id == id).ToList();
                if (ids.Count == 0 )
                {

                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new Good();
                    d.Id = id;
                    d.IdCate = categoods;
                    d.Name = name;
                    d.Price = price;
                    d.PriceTax = pricetax;
                    d.InternalPrice = internalprice;
                    d.GTGTInternalTax = gtgtinternaltax;
                    d.Discount = discount;
                    d.InternalDiscount = internaldiscount;
                    d.Expiry = expiry;
                    d.WarrantyPeriod = warrantyperiod;
                    d.MinimumInventory = minimuminventory;
                    d.MaximumInventory = maximuminventory;
                    d.Description = des;
                    d.IdUnit = unit;
                    d.IdMaterial = material;
                    d.IdSeason = season;
                    d.IdColor = color;
                    d.IdSize = size;
                    d.CreateBy = nameAdmin;
                    d.CreateDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    db.Goods.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Trùng Mã HH !!!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Edit(string id, string categoods, string name, float price, float pricetax,
    float internalprice, float gtgtinternaltax, float discount, float internaldiscount,
    DateTime expiry, DateTime warrantyperiod, float minimuminventory, float maximuminventory, string des, string unit,
    string material, string season, string color, string size)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = db.Goods.Find(id);
                    d.IdCate = categoods;
                    d.Name = name;
                    d.Price = price;
                    d.PriceTax = pricetax;
                    d.InternalPrice = internalprice;
                    d.GTGTInternalTax = gtgtinternaltax;
                    d.Discount = discount;
                    d.InternalDiscount = internaldiscount;
                    d.Expiry = expiry;
                    d.WarrantyPeriod = warrantyperiod;
                    d.MinimumInventory = minimuminventory;
                    d.MaximumInventory = maximuminventory;
                    d.Description = des;
                    d.IdUnit = unit;
                    d.IdMaterial = material;
                    d.IdSeason = season;
                    d.IdColor = color;
                    d.IdSize = size;
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
                db.Configuration.ProxyCreationEnabled = false;
                var e = db.DetailBills.Where(x => x.IdGoods == id).ToList();
                e.RemoveAll(x => x.IdGoods == id);
                db.SaveChanges();
                var d = db.Goods.Find(id);
                db.Goods.Remove(d);
                db.SaveChanges();
       
                return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Xóa Thất Bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CateGoods()
        {
            try
            {
                var c = (from b in db.CateGoods.Where(x => x.Id.Length > 0)
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
        public JsonResult Supplier()
        {
            try
            {
                var c = (from b in db.Suppliers.Where(x => x.Id.Length > 0)
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
        public JsonResult SupplierEdit(string idgoods)
        {
            try
            {
                var c = (from b in db.DetailSupplierGoods.Where(x => x.IdGoods == idgoods)
                         select new
                         {
                             id = b.IdSupplier,
                             name = b.Supplier.Name
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Unit()
        {
            try
            {
                var c = (from b in db.Units.Where(x => x.Id.Length > 0)
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
        public JsonResult Material()
        {
            try
            {
                var c = (from b in db.Materials.Where(x => x.Id.Length > 0)
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
        public JsonResult Season()
        {
            try
            {
                var c = (from b in db.Seasons.Where(x => x.Id.Length > 0)
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
        public JsonResult Color()
        {
            try
            {
                var c = (from b in db.Colors.Where(x => x.Id.Length > 0)
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
        public JsonResult Size()
        {
            try
            {
                var c = (from b in db.Sizes.Where(x => x.Id.Length > 0)
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
        public ActionResult Upload(FormCollection formCollection)
        {

                    var usersList = new List<Good>();
            var suList = new List<DetailSupplierGood>();
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
                        for (int rowIterator = 3; rowIterator <= noOfRow; rowIterator++)
                        {
                            var goods = new Good();
                            var dego = new DetailSupplierGood();
                            dego.IdSupplier = workSheet.Cells[rowIterator, 2].Value.ToString();
                            dego.IdGoods = workSheet.Cells[rowIterator, 1].Value.ToString();
                            goods.Id = workSheet.Cells[rowIterator, 1].Value.ToString();
                            goods.IdCate = workSheet.Cells[rowIterator, 3].Value.ToString();
                            goods.IdUnit = workSheet.Cells[rowIterator, 4].Value.ToString();
                            goods.IdMaterial = workSheet.Cells[rowIterator, 5].Value.ToString();
                            goods.IdSeason = workSheet.Cells[rowIterator, 6].Value.ToString();
                            goods.IdColor = workSheet.Cells[rowIterator, 7].Value.ToString();
                            goods.IdSize = workSheet.Cells[rowIterator, 8].Value.ToString();
                            goods.Name = workSheet.Cells[rowIterator, 9].Value.ToString();
                            goods.Price = Convert.ToDouble(workSheet.Cells[rowIterator, 10].Value);
                            goods.PriceTax = Convert.ToDouble(workSheet.Cells[rowIterator, 11].Value);       
                            goods.Discount = Convert.ToDouble(workSheet.Cells[rowIterator, 12].Value);
                            goods.Expiry = Convert.ToDateTime(workSheet.Cells[rowIterator, 13].Value);
                            goods.WarrantyPeriod = Convert.ToDateTime(workSheet.Cells[rowIterator, 14].Value);
                            goods.InternalPrice = 0;
                            goods.GTGTInternalTax = 0;
                            goods.InternalDiscount = 0;
                            goods.Inventory = 0;
                            goods.MinimumInventory = 0;
                            goods.MaximumInventory = 0;
                            goods.Description = "";
                            goods.CreateDate = DateTime.Now;
                            goods.CreateBy = "";
                            goods.Status = true;
                           
                            usersList.Add(goods);
                            suList.Add(dego);
                        }
                    }
                }
            }
            using (iGMSEntities db = new iGMSEntities())
            {
                foreach (var item in usersList)
                {
                    db.Goods.Add(item);
                
                }
                foreach (var item in suList)
                {
                    db.DetailSupplierGoods.Add(item);

                }
                db.SaveChanges();
            }
            return View("Index");
        }
    }
}