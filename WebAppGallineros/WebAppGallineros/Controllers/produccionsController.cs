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
    public class produccionsController : Controller
    {
        private DB_A3F19C_gallinerosEntities db = new DB_A3F19C_gallinerosEntities();

        // GET: produccions
        public ActionResult Index()
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var granja = granjero.granjas.nomenclatura;
            var produccion = db.produccion.Include(p => p.granjeros);           
            
            return View(produccion.Where(x => x.Folio.Contains(granja)).ToList().OrderByDescending(x => x.id));
        }

        // GET: produccions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produccion produccion = db.produccion.Find(id);
            if (produccion == null)
            {
                return HttpNotFound();
            }
            return View(produccion);
        }

        public ActionResult DetalleProduccion(int produccionid)
        {
            List<detproducciongallineros> model = db.detproducciongallineros.Where(x => x.Produccion_Id == produccionid).ToList();
            var produccion = db.produccion.Where(x => x.id == produccionid).FirstOrDefault();

            ViewBag.Folio = produccion.Folio;
            ViewBag.Fecha = produccion.Fecha;
            ViewBag.Granjero = produccion.granjeros.Nombre + " " + produccion.granjeros.Apellidos;
            ViewBag.Granja = produccion.granjeros.granjas.descripcion;

            return View(model);        
        }

        public ActionResult NuevaProduccion()
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();

            //Validamos Si la Granja Tiene Gallineros

            var gallineros = db.detgallinasgallineros.Where(x => x.granjas.nomenclatura.Equals(granjero.granjas.nomenclatura)).FirstOrDefault();

            ViewBag.Granjero = granjero.Nombre + " " + granjero.Apellidos;
            ViewBag.Granja = granjero.granjas.descripcion;
            ViewBag.Folio = "SIN FOLIO GENERADO";
            ViewBag.Fecha = DateTime.Now;

            if (gallineros != null)
            {                
                ViewBag.Gallinero = gallineros.gallineros.codigo;
            }
            else
            {
                ViewBag.Gallinero = "NO HAY MAS GALLINEROS";
            }
            
            return View();
        }

        public ActionResult ContinuarProduccion(int _idproduccion)
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var produccion = db.produccion.Where(x => x.id == _idproduccion).FirstOrDefault();
            var ultimogallineroregistrado = db.detproducciongallineros.Where(x => x.Produccion_Id == _idproduccion).ToList().LastOrDefault();
            var ultimoidgallineroxgranja = db.detgallinasgallineros.Where(x => x.Granjas_Id == granjero.Granjas_Id).ToList().LastOrDefault();

            if (ultimogallineroregistrado.Gallineros_Id == ultimoidgallineroxgranja.Gallineros_Id) 
            {
                ViewBag.Gallinero = "NO HAY MAS GALLINEROS";
            }
            else
            {
                var siguientegallinero = db.gallineros.Where(x => x.id > ultimogallineroregistrado.Gallineros_Id).FirstOrDefault();
                ViewBag.Gallinero = siguientegallinero.codigo;
            }

            ViewBag.Granjero = granjero.Nombre + " " + granjero.Apellidos;
            ViewBag.Granja = granjero.granjas.descripcion;
            ViewBag.Folio = produccion.Folio;
            ViewBag.Fecha = produccion.Fecha;

            return View("NuevaProduccion");
        }

        public List<gallineros> GallinerosByGranja() 
        {
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var gallinerosgranja = db.detgallinasgallineros.Where(x => x.Granjas_Id == granjero.Granjas_Id).ToList();
            List<gallineros> listaGallineros = new List<gallineros>();

            foreach (var item in gallinerosgranja)
            {
                listaGallineros.Add(item.gallineros);
            }

            return listaGallineros;
        }

        [HttpPost]
        public ActionResult GuardarProduccion(ProduccionVM p)
        {
            string folio = "";
            string gallinerosiguiente = "";
            var granjero = db.granjeros.Where(x => x.AspNetUsers.Email.Equals(User.Identity.Name)).FirstOrDefault();
            var a = db.detgallinasgallineros.Where(x => x.Granjas_Id == granjero.Granjas_Id).GroupBy(x => x.gallineros).ToList().Select(x => x.Key);
            List<gallineros> listaGallineros = a.ToList();

            if (p.folio.Trim().Equals("SIN FOLIO GENERADO"))
            {
                produccion produccion = new produccion();                
                produccion.Granjeros_Id = granjero.id;
                produccion.Fecha = DateTime.Now;
                int produccionxgranja = (db.produccion.Where(x => x.Folio.Contains(granjero.granjas.nomenclatura)).Count() + 1);
                folio = granjero.granjas.nomenclatura + "-P-" + produccionxgranja;
                produccion.Folio = folio;

                db.produccion.Add(produccion);

                detproducciongallineros dtproduccion = new detproducciongallineros();
                dtproduccion.Gallineros_Id = listaGallineros.Where(x => x.codigo.Equals(p.gallinero)).FirstOrDefault().id;
                dtproduccion.Produccion_Id = produccion.id;
                dtproduccion.cantidadhuevos = p.qty;

                db.detproducciongallineros.Add(dtproduccion);
                db.SaveChanges();

                int gallineroactual = listaGallineros.Where(x => x.codigo.Equals(p.gallinero)).FirstOrDefault().id;

                var siguientegallinero = listaGallineros.Where(x => x.id > gallineroactual).FirstOrDefault();
                gallinerosiguiente = siguientegallinero.codigo;

                return Json(new { respuesta = true, folior = folio, gallinero = gallinerosiguiente }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int idproduccion = db.produccion.Where(x => x.Folio.Equals(p.folio)).FirstOrDefault().id;
                int idgallinero = listaGallineros.Where(x => x.codigo.Equals(p.gallinero)).FirstOrDefault().id;                
                int gallineroactual = listaGallineros.Where(x => x.codigo.Equals(p.gallinero)).FirstOrDefault().id;
                int gallinerofinal = listaGallineros.ToList().LastOrDefault().id;

                var siguientegallinero = listaGallineros.Where(x => x.id > gallineroactual).FirstOrDefault();

                if (siguientegallinero != null)
                {
                    gallinerosiguiente = siguientegallinero.codigo;
                    detproducciongallineros dtproduccion = new detproducciongallineros();
                    dtproduccion.Gallineros_Id = idgallinero;
                    dtproduccion.Produccion_Id = idproduccion;
                    dtproduccion.cantidadhuevos = p.qty;

                    db.detproducciongallineros.Add(dtproduccion);
                    db.SaveChanges();
                    
                    return Json(new { respuesta = true, folior = p.folio, gallinero = gallinerosiguiente }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if ((gallinerofinal - gallineroactual) == 0)
                    {                        
                        detproducciongallineros dtproduccion = new detproducciongallineros();
                        dtproduccion.Gallineros_Id = idgallinero;
                        dtproduccion.Produccion_Id = idproduccion;
                        dtproduccion.cantidadhuevos = p.qty;

                        db.detproducciongallineros.Add(dtproduccion);
                        db.SaveChanges();

                        gallinerosiguiente = "NO HAY MAS GALLINEROS";                        
                    }

                    return Json(new { respuesta = true, folior = p.folio, gallinero = gallinerosiguiente }, JsonRequestBehavior.AllowGet);
                }        
            }            
        }

        // GET: produccions/Create
        public ActionResult Create()
        {
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre");
            return View();
        }

        // POST: produccions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Fecha,Folio,Granjeros_Id")] produccion produccion)
        {
            if (ModelState.IsValid)
            {
                db.produccion.Add(produccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", produccion.Granjeros_Id);
            return View(produccion);
        }

        // GET: produccions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produccion produccion = db.produccion.Find(id);
            if (produccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", produccion.Granjeros_Id);
            return View(produccion);
        }

        // POST: produccions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Fecha,Folio,Granjeros_Id")] produccion produccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Granjeros_Id = new SelectList(db.granjeros, "id", "Nombre", produccion.Granjeros_Id);
            return View(produccion);
        }

        // GET: produccions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produccion produccion = db.produccion.Find(id);
            if (produccion == null)
            {
                return HttpNotFound();
            }
            return View(produccion);
        }

        // POST: produccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            produccion produccion = db.produccion.Find(id);
            db.produccion.Remove(produccion);
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
