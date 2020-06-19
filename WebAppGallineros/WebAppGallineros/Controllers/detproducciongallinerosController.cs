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
    public class detproducciongallinerosController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: detproducciongallineros
        public ActionResult Index()
        {
            var detproducciongallineros = db.detproducciongallineros.Include(d => d.gallineros).Include(d => d.produccion);
            return View(detproducciongallineros.ToList());
        }

        // GET: detproducciongallineros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detproducciongallineros detproducciongallineros = db.detproducciongallineros.Find(id);
            if (detproducciongallineros == null)
            {
                return HttpNotFound();
            }
            return View(detproducciongallineros);
        }

        // GET: detproducciongallineros/Create
        public ActionResult Create()
        {
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo");
            ViewBag.Produccion_Id = new SelectList(db.produccion, "id", "Folio");
            return View();
        }

        // POST: detproducciongallineros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Produccion_Id,Gallineros_Id,cantidadhuevos")] detproducciongallineros detproducciongallineros)
        {
            if (ModelState.IsValid)
            {
                db.detproducciongallineros.Add(detproducciongallineros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detproducciongallineros.Gallineros_Id);
            ViewBag.Produccion_Id = new SelectList(db.produccion, "id", "Folio", detproducciongallineros.Produccion_Id);
            return View(detproducciongallineros);
        }

        // GET: detproducciongallineros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detproducciongallineros detproducciongallineros = db.detproducciongallineros.Find(id);
            if (detproducciongallineros == null)
            {
                return HttpNotFound();
            }
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detproducciongallineros.Gallineros_Id);
            ViewBag.Produccion_Id = new SelectList(db.produccion, "id", "Folio", detproducciongallineros.Produccion_Id);
            return View(detproducciongallineros);
        }

        // POST: detproducciongallineros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Produccion_Id,Gallineros_Id,cantidadhuevos")] detproducciongallineros detproducciongallineros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detproducciongallineros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detproducciongallineros.Gallineros_Id);
            ViewBag.Produccion_Id = new SelectList(db.produccion, "id", "Folio", detproducciongallineros.Produccion_Id);
            return View(detproducciongallineros);
        }

        // GET: detproducciongallineros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detproducciongallineros detproducciongallineros = db.detproducciongallineros.Find(id);
            if (detproducciongallineros == null)
            {
                return HttpNotFound();
            }
            return View(detproducciongallineros);
        }

        // POST: detproducciongallineros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            detproducciongallineros detproducciongallineros = db.detproducciongallineros.Find(id);
            db.detproducciongallineros.Remove(detproducciongallineros);
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
