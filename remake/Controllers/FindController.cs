using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using PagedList;

namespace remake.Controllers
{
    public class FindController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();

        // GET: Find
        [HttpGet]
        public ActionResult FindResult(string keyword, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int PageSize = 6;
            int PageNumber = (page ?? 1);
            var lstpd = db.Products.Where(n => n.ProductName.Contains(keyword));
            ViewBag.Keyword = keyword;
            return View(lstpd.OrderBy( n => n.ProductName).ToPagedList(PageNumber,PageSize));
        }

        [HttpPost]
        public ActionResult FindResult(string keyword, int? page,Product pd)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int PageSize = 6;
            int PageNumber = (page ?? 1);
            var lstpd = db.Products.Where(n => n.ProductName.Contains(keyword));
            ViewBag.Keyword = keyword;
            return View(lstpd.OrderBy(n => n.ProductName).ToPagedList(PageNumber, PageSize));
        }
    }
}