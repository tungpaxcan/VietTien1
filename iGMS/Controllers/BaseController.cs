using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using iGMS.Models;

namespace iGMS.Controllers
{
    public class BaseController : Controller
    {
        private iPOSEntities db = new iPOSEntities();
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["user"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }
        }
        [HttpGet]
        public JsonResult SignOut()
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Session["user"]=null;
               return Json(new { code = 200, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Sai !!!" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}