using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using System.Data;
using System.Data.Entity;
using System.Web.Security;

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

        public ActionResult Review()
        {
            return View(db.OrderProducts.ToList());
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserWeb acc)
        {
            var check = db.UserWebs.Where(x => x.UserName == acc.UserName && x.Password == acc.Password).FirstOrDefault();
            if (check != null && check.TypeUser == 1)
            {
                var lstPer = db.UserPermissions.Where(n => n.IDTypeUser == check.TypeUser);
                string per = "";

                if (lstPer.Count() != 0)
                {
                    foreach (var item in lstPer)
                    {
                        per += item.Permission.ID + ",";
                    }
                    per = per.Substring(0, per.Length - 1);
                    PermissionDivide(check.UserName.ToString(), per);
                    Session["user"] = check.Name.ToString();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Error = "Tên tài khoản hoặc mật khẩu không đúng Hoặc bạn không có quyền truy cập vào trang này !!!";
            }
            return View();
        }

        public void PermissionDivide(string username, string permission)
        {
            FormsAuthentication.Initialize();

            var divide = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddHours(3), false, permission);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(divide));

            if (divide.IsPersistent)
                cookie.Expires = divide.Expiration;

            Response.Cookies.Add(cookie);
        }
    }
}