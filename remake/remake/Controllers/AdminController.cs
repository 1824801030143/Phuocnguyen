using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using System.Data;
using System.Data.Entity;

namespace remake.Controllers
{
    public class AdminController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public Decimal TotalRevenue(int month, int year)
        {
            var lstOrder = db.OrderProducts.Where(n => n.OrderDay.Value.Month == month && n.OrderDay.Value.Year == year);
            decimal Total = 0;
            foreach(var item in lstOrder)
            {
                Total += decimal.Parse(item.OrderDetails.Sum(n => n.Amount * n.Price).Value.ToString());
            }
            return Total;
        }
       
    }
}