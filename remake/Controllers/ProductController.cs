using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using System.Data;
using System.Data.Entity;
using PagedList;

namespace remake.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Product
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Product(int? idcate, int? idpro,int? page )
        {
            if(idcate == null || idpro == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var lstpd = db.Products.Where(n => n.IDCategory == idcate && n.IDProvider == idpro);
            if (lstpd.Count() == 0 )
            {
                return HttpNotFound();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int PageSize = 6;
            int PageNumber = (page ?? 1);
            ViewBag.idcate = idcate;
            ViewBag.idpro = idpro;
            return View(lstpd.OrderBy(n => n.IDCategory).ToPagedList(PageNumber,PageSize));
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

        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,IDCategory,IDProvider,Poppular,Sale,New,Price,Amount,DESCRIPTION")] Product pd, HttpPostedFileBase image)
        {
            ViewBag.IDCategory = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "ID", "CategoryName");
            ViewBag.IDProvider = new SelectList(db.Providers.OrderBy(n => n.Name), "ID", "Name");
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

        [Authorize(Roles = "1")]
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductName,IDCategory,IDProvider,Poppular,Sale,New,Price,Amount,DESCRIPTION")] Product pd, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
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
                if (image != null && image.ContentLength > 0)
                {
                    pd.Image = new byte[image.ContentLength];
                    image.InputStream.Read(pd.Image, 0, image.ContentLength);
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    string urlImage = Server.MapPath("~/image/" + fileName);
                    image.SaveAs(urlImage);

                    pd.UrlImage = "image/" + fileName;

                }
                db.Entry(pd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pd);
        }

        [Authorize(Roles = "1")]
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