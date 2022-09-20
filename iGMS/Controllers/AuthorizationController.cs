using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class AuthorizationController : BaseController
    {
        private VietTienEntities db = new VietTienEntities(); 
        // GET: Authorization
        [HttpGet]
        public JsonResult UserNV()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.RoleAdmin1.ManageMainCategories == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }
               
                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PurchaseManager()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.RoleAdmin1.PurchaseManager == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult WarehouseManagement()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.RoleAdmin1.WarehouseManagement == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult UserNV1()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.Role1.EditDiscountGoods == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        } 
        [HttpGet]
        public JsonResult UserNV2()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.Role1.EditDiscountBill == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }     
        [HttpGet]
        public JsonResult UserNV3()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.Role1.EditAmountGoods == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }    
        [HttpGet]
        public JsonResult UserNV4()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.Role1.IdentifyConsultants == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }  
        [HttpGet]
        public JsonResult UserNV5()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var User = (User)Session["user"];
                if (User.Role1.ConfirmCusInfor == false)
                {
                    return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(new { code = 300, }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}