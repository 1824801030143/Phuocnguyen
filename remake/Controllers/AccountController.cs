using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace remake.Controllers
{
    public class AccountController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserWeb acc)
        {
            var check = db.UserWebs.Where(x => x.UserName == acc.UserName && x.Password == acc.Password).FirstOrDefault();
            if (check != null)
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
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Tên tài khoản hoặc mật khẩu không đúng !!!";
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

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(UserWeb acc)
        {
            db.UserWebs.Add(acc);
            db.SaveChanges();
            ModelState.Clear();
            return View("Login");
        }

        
    }
}