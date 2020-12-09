using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using remake.Models;


namespace remake.Controllers
{
    public class HomeController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Home
        public ActionResult Index()
        {
            var lstp = db.Products;
            ViewBag.Listp = lstp;
            return View();
        }

        public ActionResult Preview()
        {
            return View();
        }

        public ActionResult MenuPartial()
        {
            var lstSP = db.Products;
            return PartialView(lstSP);
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }

}