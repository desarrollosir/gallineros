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
    public class gallinasController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: gallinas
        public ActionResult Index()
        {
            return View(db.gallinas.ToList());
        }

        // GET: gallinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallinas gallinas = db.gallinas.Find(id);
            if (gallinas == null)
            {
                return HttpNotFound();
            }
            return View(gallinas);
        }

        // GET: gallinas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: gallinas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] gallinas gallinas)
        {
            if (ModelState.IsValid)
            {
                db.gallinas.Add(gallinas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gallinas);
        }

        // GET: gallinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallinas gallinas = db.gallinas.Find(id);
            if (gallinas == null)
            {
                return HttpNotFound();
            }
            return View(gallinas);
        }

        // POST: gallinas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] gallinas gallinas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallinas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallinas);
        }

        // GET: gallinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallinas gallinas = db.gallinas.Find(id);
            if (gallinas == null)
            {
                return HttpNotFound();
            }
            return View(gallinas);
        }

        // POST: gallinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gallinas gallinas = db.gallinas.Find(id);
            db.gallinas.Remove(gallinas);
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
