using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using remake.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace remake.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Category
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {            
            return View();
        }

        public ActionResult Create([Bind(Include = "ID,CategoryName ")] Category ct)
        {           
            if (ModelState.IsValid)
            {
                db.Categories.Add(ct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ct);
        }

        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category ct = db.Categories.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Category ct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ct);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category ct = db.Categories.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category ct = db.Categories.Find(id);
            db.Categories.Remove(ct);
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
    }
}