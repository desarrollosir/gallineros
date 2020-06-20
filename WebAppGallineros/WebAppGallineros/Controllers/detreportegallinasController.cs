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
    public class detreportegallinasController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: detreportegallinas
        public ActionResult Index()
        {
            var detreportegallinas = db.detreportegallinas.Include(d => d.gallinas).Include(d => d.reporte).Include(d => d.statusgallina);
            return View(detreportegallinas.ToList());
        }

        // GET: detreportegallinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detreportegallinas detreportegallinas = db.detreportegallinas.Find(id);
            if (detreportegallinas == null)
            {
                return HttpNotFound();
            }
            return View(detreportegallinas);
        }

        // GET: detreportegallinas/Create
        public ActionResult Create()
        {
            ViewBag.gallinas_id = new SelectList(db.gallinas, "id", "nombre");
            ViewBag.reporte_id = new SelectList(db.reporte, "id", "Folio");
            ViewBag.statusgallina_id = new SelectList(db.statusgallina, "id", "descripcion");
            return View();
        }

        // POST: detreportegallinas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,gallinas_id,statusgallina_id,reporte_id")] detreportegallinas detreportegallinas)
        {
            if (ModelState.IsValid)
            {
                db.detreportegallinas.Add(detreportegallinas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.gallinas_id = new SelectList(db.gallinas, "id", "nombre", detreportegallinas.gallinas_id);
            ViewBag.reporte_id = new SelectList(db.reporte, "id", "Folio", detreportegallinas.reporte_id);
            ViewBag.statusgallina_id = new SelectList(db.statusgallina, "id", "descripcion", detreportegallinas.statusgallina_id);
            return View(detreportegallinas);
        }

        // GET: detreportegallinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detreportegallinas detreportegallinas = db.detreportegallinas.Find(id);
            if (detreportegallinas == null)
            {
                return HttpNotFound();
            }
            ViewBag.gallinas_id = new SelectList(db.gallinas, "id", "nombre", detreportegallinas.gallinas_id);
            ViewBag.reporte_id = new SelectList(db.reporte, "id", "Folio", detreportegallinas.reporte_id);
            ViewBag.statusgallina_id = new SelectList(db.statusgallina, "id", "descripcion", detreportegallinas.statusgallina_id);
            return View(detreportegallinas);
        }

        // POST: detreportegallinas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,gallinas_id,statusgallina_id,reporte_id")] detreportegallinas detreportegallinas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detreportegallinas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gallinas_id = new SelectList(db.gallinas, "id", "nombre", detreportegallinas.gallinas_id);
            ViewBag.reporte_id = new SelectList(db.reporte, "id", "Folio", detreportegallinas.reporte_id);
            ViewBag.statusgallina_id = new SelectList(db.statusgallina, "id", "descripcion", detreportegallinas.statusgallina_id);
            return View(detreportegallinas);
        }

        // GET: detreportegallinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            detreportegallinas detreportegallinas = db.detreportegallinas.Find(id);
            if (detreportegallinas == null)
            {
                return HttpNotFound();
            }
            return View(detreportegallinas);
        }

        // POST: detreportegallinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            detreportegallinas detreportegallinas = db.detreportegallinas.Find(id);
            db.detreportegallinas.Remove(detreportegallinas);
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
