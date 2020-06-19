using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppGallineros.Entities;
using WebAppGallineros.ViewModels;

namespace WebAppGallineros.Controllers
{
    [Authorize]
    public class reportesController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: reportes
        public ActionResult Index()
        {
            var reporte = db.reporte.Include(r => r.granjeros);
            return View(reporte.ToList());
        }

        public ActionResult NuevoReporte() 
        {
            return View();
        }

        public ActionResult GuardarReporte(DetalleReporteVM[] detalleReporte)
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            int produccionxgranja = (db.produccion.Where(x => x.Folio.Contains(granjero.granjas.nomenclatura)).Count() + 1);

            reporte reporte = new reporte();
            reporte.Fecha = DateTime.Now;
            reporte.Folio = granjero.granjas.nomenclatura + "-RP-" + produccionxgranja;
            reporte.Granjeros_Id = granjero.id;

            db.reporte.Add(reporte);
            db.SaveChanges();

            AgregarDetalleReporte(detalleReporte, reporte.id);

            return View();
        }

        public void AgregarDetalleReporte(DetalleReporteVM[] detalleReporte, int reporteid) 
        {
            List<detreportegallinas> listaReporte = new List<detreportegallinas>();

            foreach (var item in detalleReporte)
            {
                detreportegallinas detalle = new detreportegallinas();
                detalle.reporte_id = reporteid;
                detalle.statusgallina_id = db.statusgallina.Where(x => x.descripcion.Equals(item.statusgallina)).FirstOrDefault().id;
                detalle.gallinas_id = db.gallinas.Where(x => x.nombre.Equals(item.gallina)).FirstOrDefault().id;


                listaReporte.Add(detalle);
            }

            db.detreportegallinas.AddRange(listaReporte);
            db.SaveChanges();
        }

        [HttpPost]        
        public JsonResult ListaStatusGallinas()
        {
            List<SelectListItem> liststatus = new List<SelectListItem>();

            foreach (var item in db.statusgallina.Where(x => x.id != 1).ToList())
            {
                liststatus.Add(new SelectListItem
                {
                    Value = item.descripcion,
                    Text = item.descripcion
                });
            }

            return Json(liststatus);
        }

        [HttpPost]
        public JsonResult AutoCompleteGallinas(string prefix)
        {
            var proveedores = (from proveedor in db.gallinas
                               where proveedor.nombre.Contains(prefix)
                               orderby proveedor.nombre ascending
                               select new
                               {
                                   label = proveedor.nombre,
                                   val = proveedor.nombre
                               }).ToList();

            return Json(proveedores);
        }

        // GET: reportes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reporte reporte = db.reporte.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // GET: reportes/Create
        public ActionResult Create()
        {
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre");
            return View();
        }

        // POST: reportes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Fecha,Folio,Granjeros_Id")] reporte reporte)
        {
            if (ModelState.IsValid)
            {
                db.reporte.Add(reporte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", reporte.Granjeros_Id);
            return View(reporte);
        }

        // GET: reportes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reporte reporte = db.reporte.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", reporte.Granjeros_Id);
            return View(reporte);
        }

        // POST: reportes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Fecha,Folio,Granjeros_Id")] reporte reporte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reporte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", reporte.Granjeros_Id);
            return View(reporte);
        }

        // GET: reportes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reporte reporte = db.reporte.Find(id);
            if (reporte == null)
            {
                return HttpNotFound();
            }
            return View(reporte);
        }

        // POST: reportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            reporte reporte = db.reporte.Find(id);
            db.reporte.Remove(reporte);
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
