using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppGallineros.Entities;

namespace WebAppGallineros.Controllers
{
    [Authorize]
    public class gallinerosController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: gallineros
        public ActionResult Index()
        {
            return View(db.gallineros.ToList());
        }

        // GET: gallineros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallineros gallineros = db.gallineros.Find(id);
            if (gallineros == null)
            {
                return HttpNotFound();
            }
            return View(gallineros);
        }

        // GET: gallineros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: gallineros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo")] gallineros gallineros)
        {
            if (ModelState.IsValid)
            {
                db.gallineros.Add(gallineros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gallineros);
        }

        // GET: gallineros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallineros gallineros = db.gallineros.Find(id);
            if (gallineros == null)
            {
                return HttpNotFound();
            }
            return View(gallineros);
        }

        // POST: gallineros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo")] gallineros gallineros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallineros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallineros);
        }

        // GET: gallineros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallineros gallineros = db.gallineros.Find(id);
            if (gallineros == null)
            {
                return HttpNotFound();
            }
            return View(gallineros);
        }

        // POST: gallineros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gallineros gallineros = db.gallineros.Find(id);
            db.gallineros.Remove(gallineros);
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
