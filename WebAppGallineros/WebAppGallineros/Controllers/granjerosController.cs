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
    public class granjerosController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: granjeros
        public ActionResult Index()
        {
            var granjeros = db.granjeros.Include(g => g.AspNetUsers).Include(g => g.granjas);
            return View(granjeros.ToList());
        }

        // GET: granjeros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjeros granjeros = db.granjeros.Find(id);
            if (granjeros == null)
            {
                return HttpNotFound();
            }
            return View(granjeros);
        }

        // GET: granjeros/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUsers_Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura");
            return View();
        }

        // POST: granjeros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Nombre,Apellidos,Granjas_Id,AspNetUsers_Id")] granjeros granjeros)
        {
            if (ModelState.IsValid)
            {
                db.granjeros.Add(granjeros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUsers_Id = new SelectList(db.AspNetUsers, "Id", "Email", granjeros.AspNetUsers_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", granjeros.Granjas_Id);
            return View(granjeros);
        }

        // GET: granjeros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjeros granjeros = db.granjeros.Find(id);
            if (granjeros == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUsers_Id = new SelectList(db.AspNetUsers, "Id", "Email", granjeros.AspNetUsers_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", granjeros.Granjas_Id);
            return View(granjeros);
        }

        // POST: granjeros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nombre,Apellidos,Granjas_Id,AspNetUsers_Id")] granjeros granjeros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(granjeros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUsers_Id = new SelectList(db.AspNetUsers, "Id", "Email", granjeros.AspNetUsers_Id);
            ViewBag.Granjas_Id = new SelectList(db.granjas, "id", "nomenclatura", granjeros.Granjas_Id);
            return View(granjeros);
        }

        // GET: granjeros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            granjeros granjeros = db.granjeros.Find(id);
            if (granjeros == null)
            {
                return HttpNotFound();
            }
            return View(granjeros);
        }

        // POST: granjeros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            granjeros granjeros = db.granjeros.Find(id);
            db.granjeros.Remove(granjeros);
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
