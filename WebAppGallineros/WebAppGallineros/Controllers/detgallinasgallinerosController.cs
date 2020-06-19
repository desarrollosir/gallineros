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
    public class detgallinasgallinerosController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: detgallinasgallineros
        public ActionResult Index()
        {
            var detgallinasgallineros = db.detgallinasgallineros.Include(d => d.gallinas).Include(d => d.gallineros).Include(d => d.granjas).Include(d => d.statusgallina);
            return View(detgallinasgallineros.ToList());
        }

        // GET: detgallinasgallineros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detgallinasgallineros detgallinasgallineros = db.detgallinasgallineros.Find(id);
            if (detgallinasgallineros == null)
            {
                return HttpNotFound();
            }
            return View(detgallinasgallineros);
        }

        // GET: detgallinasgallineros/Create
        public ActionResult Create()
        {
            ViewBag.Gallinas_Id = new SelectList(db.gallinas, "id", "nombre");
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo");
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura");
            ViewBag.StatusGallina_Id = new SelectList(db.statusgallina, "id", "descripcion");
            return View();
        }

        // POST: detgallinasgallineros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Gallinas_Id,Gallineros_Id,Granjas_Id,StatusGallina_Id")] detgallinasgallineros detgallinasgallineros)
        {
            if (ModelState.IsValid)
            {
                db.detgallinasgallineros.Add(detgallinasgallineros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Gallinas_Id = new SelectList(db.gallinas, "id", "nombre", detgallinasgallineros.Gallinas_Id);
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detgallinasgallineros.Gallineros_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", detgallinasgallineros.Granjas_Id);
            ViewBag.StatusGallina_Id = new SelectList(db.statusgallina, "id", "descripcion", detgallinasgallineros.StatusGallina_Id);
            return View(detgallinasgallineros);
        }

        // GET: detgallinasgallineros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detgallinasgallineros detgallinasgallineros = db.detgallinasgallineros.Find(id);
            if (detgallinasgallineros == null)
            {
                return HttpNotFound();
            }
            ViewBag.Gallinas_Id = new SelectList(db.gallinas, "id", "nombre", detgallinasgallineros.Gallinas_Id);
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detgallinasgallineros.Gallineros_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", detgallinasgallineros.Granjas_Id);
            ViewBag.StatusGallina_Id = new SelectList(db.statusgallina, "id", "descripcion", detgallinasgallineros.StatusGallina_Id);
            return View(detgallinasgallineros);
        }

        // POST: detgallinasgallineros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Gallinas_Id,Gallineros_Id,Granjas_Id,StatusGallina_Id")] detgallinasgallineros detgallinasgallineros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detgallinasgallineros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gallinas_Id = new SelectList(db.gallinas, "id", "nombre", detgallinasgallineros.Gallinas_Id);
            ViewBag.Gallineros_Id = new SelectList(db.gallineros, "id", "codigo", detgallinasgallineros.Gallineros_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", detgallinasgallineros.Granjas_Id);
            ViewBag.StatusGallina_Id = new SelectList(db.statusgallina, "id", "descripcion", detgallinasgallineros.StatusGallina_Id);
            return View(detgallinasgallineros);
        }

        // GET: detgallinasgallineros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detgallinasgallineros detgallinasgallineros = db.detgallinasgallineros.Find(id);
            if (detgallinasgallineros == null)
            {
                return HttpNotFound();
            }
            return View(detgallinasgallineros);
        }

        // POST: detgallinasgallineros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            detgallinasgallineros detgallinasgallineros = db.detgallinasgallineros.Find(id);
            db.detgallinasgallineros.Remove(detgallinasgallineros);
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
