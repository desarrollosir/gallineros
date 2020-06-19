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
    public class granjasController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: granjas
        public ActionResult Index()
        {
            return View(db.granjas.ToList());
        }

        // GET: granjas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjas granjas = db.granjas.Find(id);
            if (granjas == null)
            {
                return HttpNotFound();
            }
            return View(granjas);
        }

        // GET: granjas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: granjas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nomenclatura,descripcion")] granjas granjas)
        {
            if (ModelState.IsValid)
            {
                db.granjas.Add(granjas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(granjas);
        }

        // GET: granjas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjas granjas = db.granjas.Find(id);
            if (granjas == null)
            {
                return HttpNotFound();
            }
            return View(granjas);
        }

        // POST: granjas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nomenclatura,descripcion")] granjas granjas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(granjas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(granjas);
        }

        // GET: granjas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjas granjas = db.granjas.Find(id);
            if (granjas == null)
            {
                return HttpNotFound();
            }
            return View(granjas);
        }

        // POST: granjas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            granjas granjas = db.granjas.Find(id);
            db.granjas.Remove(granjas);
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
