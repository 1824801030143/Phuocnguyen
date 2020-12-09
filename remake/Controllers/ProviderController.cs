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
    public class ProviderController : Controller
    {
        private readonly NuocHoaShopEntities db = new NuocHoaShopEntities();
        // GET: Provider
        public ActionResult Index()
        {
            return View(db.Providers.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IDCategory = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "ID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name ")] Provider pv)
        {
            if (ModelState.IsValid)
            {
                db.Providers.Add(pv);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pv );
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider pv = db.Providers.Find(id);
            if (pv == null)
            {
                return HttpNotFound();
            }
            return View(pv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Provider pv)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pv);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider pv = db.Providers.Find(id);
            if (pv == null)
            {
                return HttpNotFound();
            }
            return View(pv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Provider pv = db.Providers.Find(id);
            db.Providers.Remove(pv);
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