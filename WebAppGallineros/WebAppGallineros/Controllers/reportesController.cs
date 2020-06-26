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
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var reporte = db.reporte.Include(r => r.granjeros);
            var lista = reporte.Where(x => x.Folio.Contains(granjero.granjas.nomenclatura)).ToList();

            return View(lista);
        }

        public ActionResult NuevoReporte()
        {
            return View();
        }

        public ActionResult GuardarReporte(DetalleReporteVM[] detalleReporte)
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            int produccionxgranja = (db.reporte.Where(x => x.Folio.Contains(granjero.granjas.nomenclatura)).Count() + 1);

            reporte reporte = new reporte();
            reporte.Fecha = DateTime.Now;
            reporte.Folio = granjero.granjas.nomenclatura + "-RP-" + produccionxgranja;
            reporte.Granjeros_Id = granjero.id;

            db.reporte.Add(reporte);
            db.SaveChanges();

            AgregarDetalleReporte(detalleReporte, reporte.id);
            ActualizarEstadoGallinas(detalleReporte);

            return Json(new { respuesta = true }, JsonRequestBehavior.AllowGet);
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

        public void ActualizarEstadoGallinas(DetalleReporteVM[] detalleReporte) 
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            
            foreach (var item in detalleReporte)
            {
                detgallinasgallineros registroeditar = db.detgallinasgallineros.Where(x => x.Granjas_Id.Equals(granjero.Granjas_Id) && x.gallinas.nombre.Equals(item.gallina)).FirstOrDefault();
                registroeditar.StatusGallina_Id = db.statusgallina.Where(x => x.descripcion.Contains(item.statusgallina)).FirstOrDefault().id;
                db.SaveChanges();
            }
        }

        public ActionResult DetalleReporte(int reporteid)
        {
            List<detreportegallinas> listaReporte = db.detreportegallinas.Where(x => x.reporte_id == reporteid).ToList();
            reporte reporte = db.reporte.Where(x => x.id == reporteid).FirstOrDefault();
            ViewBag.Granjero = reporte.granjeros.Nombre + " " + reporte.granjeros.Apellidos;
            ViewBag.Folio = reporte.Folio;
            ViewBag.Fecha = reporte.Fecha;
            ViewBag.Granja = reporte.granjeros.granjas.descripcion;
            return View(listaReporte);
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
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var gallinasgranja = (from det in db.detgallinasgallineros
                                  where det.Granjas_Id == granjero.Granjas_Id && det.StatusGallina_Id == 1 && det.gallinas.nombre.Contains(prefix)
                                  select new
                                  {
                                      label = det.gallinas.nombre,
                                      val = det.gallinas.nombre
                                  }).ToList();

            return Json(gallinasgranja);
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
