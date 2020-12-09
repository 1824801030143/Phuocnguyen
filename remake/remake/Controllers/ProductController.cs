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
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        [ChildActionOnly]
        public ActionResult ProductstylePartial()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IDCategory = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "ID", "CategoryName");
            ViewBag.IDProvider = new SelectList(db.Providers.OrderBy(n => n.Name), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,IDCategory,IDProvider,Poppular,Sale,New,Price,Amount")] Product pd, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                pd.Image = new byte[image.ContentLength];
                image.InputStream.Read(pd.Image, 0, image.ContentLength);
                string fileName = System.IO.Path.GetFileName(image.FileName);
                string urlImage = Server.MapPath("~/image/" + fileName);
                image.SaveAs(urlImage);

                pd.UrlImage = "image/" + fileName;
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(pd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pd);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.IDCategory = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "ID", "CategoryName");
            ViewBag.IDProvider = new SelectList(db.Providers.OrderBy(n => n.Name), "ID", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pd = db.Products.Find(id);
            if (pd == null)
            {
                return HttpNotFound();
            }
            return View(pd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pd = db.Products.Find(id);
            if (pd == null)
            {
                return HttpNotFound();
            }
            return View(pd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product pd = db.Products.Find(id);
            db.Products.Remove(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult DetailsProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pd = db.Products.Find(id);
            if(pd == null)
            {
                return HttpNotFound();
            }
            return View(pd);
        }
    }
}