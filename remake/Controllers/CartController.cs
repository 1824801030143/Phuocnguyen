using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using remake.Models;

namespace remake.Controllers
{
    public class CartController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();

        public List<Cart> GetListCart()
        {
            List<Cart> lstcart = Session ["Cart"] as List<Cart>;
            if(lstcart == null)
            {
                lstcart = new List<Cart>();
                Session["Cart"] = lstcart;
            }
            return lstcart;
        }

        public ActionResult AddCart(int? idpd, string strURL)
        {
            Product pd = db.Products.SingleOrDefault(n => n.ID == idpd);

            if (pd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<Cart> lstcart = GetListCart();
            Cart check = lstcart.SingleOrDefault(n => n.IDProduct == idpd);

            if(check != null)
            {
                if(pd.Amount < check.Amount)
                {
                    return View("~/Home/Index");
                }
                check.Amount++;
                check.Total = check.Amount * check.Price;
                return Redirect(strURL);
            }
            Cart item = new Cart(idpd);
            lstcart.Add(item);
            return Redirect(strURL);
        }

        public double? TotalAmount()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;

            if(lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.Amount);
        }

        public double? TotalMoney()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                return 0;
            }
            return lstCart.Sum(n => n.Total);
        }

        public ActionResult CartPartial()
        {
            if(TotalAmount() == 0)
            {
                return PartialView();
            }
            ViewBag.TotalAmount = TotalAmount();
            return PartialView();
        }

        public ActionResult Index()
        {
            List<Cart> lstCart = GetListCart();
            ViewBag.TotalMoney = TotalMoney();
            return View(lstCart);
        }

        public ActionResult DeleteCart (int id)
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Product pd = db.Products.SingleOrDefault(n => n.ID == id);
            if(pd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<Cart> lstcart = GetListCart();

            Cart check = lstcart.SingleOrDefault(n => n.IDProduct == id);

            if(check == null)
            {
                return RedirectToAction("Index", "Home");
            }

            lstcart.Remove(check);

            return RedirectToAction("Index");
        }

        public ActionResult OrderProduct(UserWeb user)
        {
            if(Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            UserWeb us = new UserWeb();
            if(Session["user"] == null)
            {
                us = user;
                db.UserWebs.Add(us);
                db.SaveChanges();
            }
            else
            {
                UserWeb mem = Session["user"] as UserWeb;
                us.Name = mem.Name;
                us.Address = mem.Address;
                us.Email = mem.Email;
                us.Phone = mem.Phone;
                db.UserWebs.Add(us);
            }

            OrderProduct op = new OrderProduct();
            op.IDUser = us.ID;
            op.OrderDay = DateTime.Now;
            db.OrderProducts.Add(op);
            db.SaveChanges();

            List<Cart> lstcart = GetListCart();
            foreach(var item in lstcart)
            {
                OrderDetail od = new OrderDetail();
                od.ID = op.ID;
                od.IDProduct = item.IDProduct;
                od.ProductName = item.ProductName;
                od.Price = item.Price;
                od.Amount = item.Amount;
                db.OrderDetails.Add(od);
            }
            db.SaveChanges();
            Session["Cart"] = null;
            return RedirectToAction("Index");
        }
    }
}