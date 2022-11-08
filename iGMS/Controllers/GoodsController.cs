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
using System.Web.UI;
using System.Windows;
using iGMS.Models;
using OfficeOpenXml;

namespace iGMS.Controllers
{
    public class GoodsController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
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
                             idgood = b.IdGood,
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
        public JsonResult Add(string id, string idgood,string sku, string style,string warehouse,float qty, string color, string size, string name, string gender, string categoods
            , string groupgoods,float price, string season, string coo, string material, string company,float pricenew)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var ids = db.Goods.Where(x => x.Id == id).ToList();
                if (ids.Count == 0 )
                {
                    var e = new DetailWareHouse();
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new Good();
                    d.Discount = 0;
                    d.SKU = sku;
                    d.IdWareHouse = warehouse;
                    d.Id = id;
                    d.IdGood = idgood;
                    d.IdStyle = style;
                    d.IdColor = color;
                    d.IdSize = size;
                    d.Name = name;
                    d.IdGender = gender;
                    d.IdCate = categoods;
                    d.IdGroupGood = groupgoods;
                    d.Price = price;
                    d.IdSeason = season;
                    d.IdCoo = coo;
                    d.Material = material;
                    d.Company = company;
                    d.PriceNew = pricenew;
                    d.CreateBy = nameAdmin;
                    d.CreateDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    e.IdWareHouse = warehouse;
                    e.IdGoods = id;
                    e.Inventory = qty;
                    db.Goods.Add(d);
                    db.DetailWareHouses.Add(e);
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
        public JsonResult Edit(string id, string idgood,string sku, string style,string warehouse,float qty, string color, string size, string name, string gender, string categoods
            , string groupgoods, float price, string season, string coo, string material, string company, float pricenew)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                var session = (User)Session["user"];
                var nameAdmin = session.Name;
                var d = db.Goods.Find(id);
                var e = db.DetailWareHouses.SingleOrDefault(x => x.IdGoods == id && x.Inventory == qty);
                e.IdWareHouse =warehouse;
                d.IdGood = idgood;
                d.SKU = sku;
                d.IdStyle = style;
                d.IdWareHouse = warehouse;
                d.IdColor = color;
                d.IdSize = size;
                d.Name = name;
                d.IdGender = gender;
                d.IdCate = categoods;
                d.IdGroupGood = groupgoods;
                d.Price = price;
                d.IdSeason = season;
                d.IdCoo = coo;
                d.Material = material;
                d.Company = company;
                d.PriceNew = pricenew;
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
        public JsonResult GroupGoods()
        {
            try
            {
                var c = (from b in db.GroupGoods.Where(x => x.Id.Length > 0)
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
        public JsonResult Style()
        {
            try
            {
                var c = (from b in db.Styles.Where(x => x.Id.Length > 0)
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
        [HttpGet]
        public JsonResult Gender()
        {
            try
            {
                var c = (from b in db.Genders.Where(x => x.Id.Length > 0)
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
        public JsonResult Coo()
        {
            try
            {
                var c = (from b in db.Coos.Where(x => x.Id.Length > 0)
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
        public JsonResult WareHouse()
        {
            try
            {
                var c = (from b in db.WareHouses.Where(x => x.Id.Length > 0)
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
        public JsonResult QTY(string id,string warehouse)
        {
            try
            {
                var c = (from b in db.DetailWareHouses.Where(x => x.IdGoods == id && x.IdWareHouse == warehouse)
                         select new
                         {
                             qty = b.Inventory,
                         }).ToList();
                return Json(new { code = 200, c = c, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EPC(string id, string epc)
        {
            try
            {
                var a = new EPC();
                a.IdGoods = id;
                a.IdEPC = epc;
                a.Status = true;
                db.EPCs.Add(a);
                db.SaveChanges();
                return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
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
            var EPCList = new List<EPC>();
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
                            var goods = new Good();
                            var dego = new DetailSupplierGood();
                            var epc = new EPC();
                          
                            var id = workSheet.Cells[rowIterator, 1].Value.ToString();
                            var idepc = Encode.EPC(workSheet.Cells[rowIterator, 2].Value.ToString());
                            var supplier = workSheet.Cells[rowIterator, 5].Value.ToString();
                            var warehouse = workSheet.Cells[rowIterator, 4].Value.ToString();
                            var style = workSheet.Cells[rowIterator, 6].Value == null ? null  : workSheet.Cells[rowIterator, 6].Value.ToString();
                            var color = workSheet.Cells[rowIterator, 7].Value == null ? "" : workSheet.Cells[rowIterator, 7].Value.ToString();
                            var size = workSheet.Cells[rowIterator, 9].Value == null ? "" : workSheet.Cells[rowIterator, 9].Value.ToString();
                            var gender = workSheet.Cells[rowIterator, 10].Value == null ? "" : workSheet.Cells[rowIterator, 10].Value.ToString();
                            var groupgood = workSheet.Cells[rowIterator, 11].Value == null ? "" : workSheet.Cells[rowIterator, 11].Value.ToString();
                            var categood = workSheet.Cells[rowIterator, 16].Value == null ? "" : workSheet.Cells[rowIterator, 16].Value.ToString();
                            var season = workSheet.Cells[rowIterator, 23].Value == null ? "" : workSheet.Cells[rowIterator, 23].Value.ToString();
                            var coo = workSheet.Cells[rowIterator, 24].Value == null ? "" : workSheet.Cells[rowIterator, 24].Value.ToString();
                            var idss = db.Goods.Where(x => x.Id == id ).ToList();
                            var idssepc = db.EPCs.Where(x => x.IdGoods == id && x.IdEPC == idepc).ToList();
                            var su = suList.Where(x => x.IdSupplier == supplier &&x.IdGoods == id).ToList();
                            var Kho = db.WareHouses.Find(warehouse);
                            var NCC = db.Suppliers.Find(supplier);
                            var Styles = db.Styles.Find(style);
                            var Color = db.Colors.Find(color);
                            var Size = db.Sizes.Find(size);
                            var Gender = db.Genders.Find(gender);
                            var Groupgood = db.GroupGoods.Find(groupgood);
                            var Categood = db.CateGoods.Find(categood);
                            var Season = db.Seasons.Find(season);
                            var Coo = db.Coos.Find(coo);
                            var qty = db.DetailWareHouses.SingleOrDefault(x => x.IdWareHouse == warehouse && x.IdGoods == id);
                      
                            if (Kho == null)
                            {
                                var wa = new WareHouse();
                                wa.Id = warehouse;
                                wa.Name = warehouse;
                                db.WareHouses.Add(wa);
                                db.SaveChanges();
                            }
                            if(NCC == null)
                            {
                                var sup = new Supplier();
                                sup.Id = supplier;
                                sup.Name = supplier;
                                db.Suppliers.Add(sup);
                                db.SaveChanges();
                            } 
                            //if(Styles == null)
                            //{
                            //    MessageBox.Show("Chưa Có Phong Cách " + style+ " Trong Dữu Liệu Tại Dòng " + rowIterator);
                            //    return View("../Goods/Index");
                            //}
                            if(Color == null)
                            {
                                var col = new Color();
                                col.Id = color;
                                col.Name = color;
                                db.Colors.Add(col);
                                db.SaveChanges();
                            }
                            if(Gender == null)
                            {
                                var gen = new Gender();
                                gen.Id = gender;
                                gen.Name = gender;
                                db.Genders.Add(gen);
                                db.SaveChanges();
                            }
                            if(Groupgood == null)
                            {
                                var gro = new GroupGood();
                                gro.Id = groupgood;
                                gro.Name = groupgood;
                                db.GroupGoods.Add(gro);
                                db.SaveChanges();
                            }if(Categood == null)
                            {
                                var cat = new CateGood();
                                cat.Id = categood;
                                cat.Name = categood;
                                db.CateGoods.Add(cat);
                                db.SaveChanges();
                            }if(Season == null)
                            {
                                var sea = new Season();
                                sea.Id = season;
                                sea.Name = season;
                                db.Seasons.Add(sea);
                                db.SaveChanges();
                            }if(Coo == null)
                            {
                                var co = new Coo();
                                co.Id = coo;
                                co.Name = coo;
                                db.Coos.Add(co);
                                db.SaveChanges();
                            }
                            //if (Size == null)
                            //{
                            //    MessageBox.Show("Chưa Có Kích Thước " + size + "Trong Dữ Liệu Tại Dòng " + rowIterator);
                            //    return View("../Goods/Index");
                            //}
                            if (idss.Count==0)
                            {
                                goods.Discount = 0;
                                goods.Id = workSheet.Cells[rowIterator, 1].Value.ToString();
                                goods.IdGood = workSheet.Cells[rowIterator, 3].Value.ToString();
                                goods.IdWareHouse = workSheet.Cells[rowIterator, 4].Value.ToString();
                                goods.IdStyle = style;
                                goods.IdColor = color;
                                goods.IdSize = size;
                                goods.IdGender =gender;
                                goods.IdGroupGood = groupgood;
                                goods.Name = workSheet.Cells[rowIterator, 12].Value.ToString();
                                goods.IdCate = categood;
                                goods.Price = Convert.ToDouble(workSheet.Cells[rowIterator, 22].Value.ToString().Replace("/,/g", ""));
                                dego.PurchasePrice = Convert.ToDouble(workSheet.Cells[rowIterator, 22].Value.ToString().Replace("/,/g", ""));
                                goods.IdSeason = season;
                                goods.IdCoo = coo;
                                goods.Company = workSheet.Cells[rowIterator, 32].Value.ToString();
                                goods.PriceNew = Convert.ToDouble(workSheet.Cells[rowIterator, 34].Value.ToString().Replace("/,/g", ""));
                                dego.IdSupplier = workSheet.Cells[rowIterator, 5].Value.ToString();
                                dego.IdGoods = workSheet.Cells[rowIterator, 1].Value.ToString();
                                db.DetailSupplierGoods.Add(dego);
                                db.Goods.Add(goods);
                                db.SaveChanges();
                            }
                            if ( idssepc.Count == 0)
                            {
                                epc.IdGoods = workSheet.Cells[rowIterator, 1].Value.ToString();
                                epc.IdEPC = idepc;
                                epc.Status = true;
                                db.EPCs.Add(epc);
                                db.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("Trùng Epc " + idepc + " Tại Dòng " + rowIterator);
                            }
                            if (qty != null)
                            {
                                qty.Inventory += 1;
                                db.SaveChanges();
                            }
                            else
                            {
                                var dewa = new DetailWareHouse();
                                dewa.Inventory = 1;
                                dewa.Status = true;
                                dewa.StatusWait = true;
                                dewa.IdGoods = id;
                                dewa.IdWareHouse = warehouse;
                                db.DetailWareHouses.Add(dewa);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            return View("Index");
        }
    }
}