using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;
using System.Data.Entity.ModelConfiguration;

namespace iGMS.Controllers
{
    public class RegisterController : BaseController
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListUser()
        {
            return View();
        }
        public ActionResult EditUser(string id)
        {
            if (id.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpGet]
        public JsonResult List(int pagenum,int page,string seach)
        {
            try
            {
                var pageSize = pagenum;
                var a = (from b in db.Users.Where(x => x.Id.Length>0)
                         select new
                         {
                             id = b.Id,
                             name = b.Name,
                             address = b.AddRess,
                             email = b.Email,
                             ManageMainCategories = b.RoleAdmin1.ManageMainCategories == true ? "Quản Lý Chính\n" : "",
                             PurchaseManager = b.RoleAdmin1.PurchaseManager == true ? "Quản Lý Mua Hàng\n" : "",
                             SalesManager = b.RoleAdmin1.SalesManager == true ? "Quản Lý Bán Hàng\n" : "",
                             WarehouseManagement = b.RoleAdmin1.WarehouseManagement == true ? "Quản Lý Kho Hàng\n" : "",
                             ManagePayments = b.RoleAdmin1.ManagePayments == true ? "Quản Lý Thanh Toán\n" : "",
                             AccountingTransfer = b.RoleAdmin1.AccountingTransfer == true ? "Quản Lý Kết Chuyên Kế Toán\n" : ""
                         }).ToList().Where(x=>x.id.ToLower().Contains(seach)||x.name.ToLower().Contains(seach));
                var pages = a.Count() % pageSize == 0 ? a.Count() / pageSize : a.Count() / pageSize + 1;
                var c = a.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var count = a.Count();
                return Json(new { code = 200,c = c,pages = pages,count =count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Register(string id,string user, string pass,string name,string province,string district,
                                    string town,string address,string email,DateTime birth,int phone,string des)
        {
            try
            {
                var idNV = "NV" + id;
                var ids = db.Users.Where(x => x.Id == idNV).ToList();
                var users = db.Users.Where(x => x.User1 == user).ToList();
                if (ids.Count == 0 && users.Count == 0)
                {
                    var idroleadmin = db.RoleAdmins.OrderBy(x => x.Id);
                    var idrole = db.Roles.OrderBy(x => x.Id);
                    var Address = province + " ," + district + " ," + town + " ," + address;
                    var session = (User)Session["user"];
                    var nameAdmin = session.Name;
                    var d = new User();
                    d.Id = idNV;
                    d.User1 = user;
                    d.Pass = Encode.ToMD5(pass);
                    d.Name = name;
                    d.AddRess = Address;
                    d.Email = email;
                    d.Birth = birth;
                    d.Phone = phone;
                    d.Description = des;
                    d.RoleAdmin1 = idroleadmin.ToList().LastOrDefault();
                    d.Role1 = idrole.ToList().LastOrDefault();
                    d.CreateBy = nameAdmin;
                    d.CreateDate = DateTime.Now;
                    d.ModifyBy = nameAdmin;
                    d.ModifyDate = DateTime.Now;
                    db.Users.Add(d);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "Trùng USER !!!" }, JsonRequestBehavior.AllowGet);
                }
               
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
                var d = db.Users.Find(id);
                var e = db.RoleAdmins.Find(d.RoleAdmin);
                var f = db.Roles.Find(d.Role);
                db.Users.Remove(d);
                db.SaveChanges();
                db.RoleAdmins.Remove(e);
                db.SaveChanges();
                db.Roles.Remove(f);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { code = 500,msg ="Xóa Thất Bại"  }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}