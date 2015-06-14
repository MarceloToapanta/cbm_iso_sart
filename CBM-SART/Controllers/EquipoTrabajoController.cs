using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CBM_SART.Models;
using System.Linq.Dynamic;
using System.IO;

namespace CBM_SART.Controllers
{
    public class EquipoTrabajoController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /EquipoTrabajo/
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.iso_equipo_proteccion.ToListAsync());
        //}
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "iep_nombre_equipo_p", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_equipo_proteccion>();
            ViewBag.filter = filter;
            records.Content = db.iso_equipo_proteccion
                        .Where(x => filter == null ||
                                (x.iep_nombre_equipo_p.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_equipo_proteccion
                         .Where(x => filter == null ||
                                (x.iep_nombre_equipo_p.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
        }

        // GET: /EquipoTrabajo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_equipo_proteccion iso_equipo_proteccion = await db.iso_equipo_proteccion.FindAsync(id);
            if (iso_equipo_proteccion == null)
            {
                return HttpNotFound();
            }
            //return View(iso_equipo_proteccion);
            return PartialView("Details", iso_equipo_proteccion);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        // GET: /EquipoTrabajo/Create
        public ActionResult Create()
        {
            iso_equipo_proteccion iso_equipo_proteccion = new iso_equipo_proteccion();
            //return View();
            return PartialView("Details", iso_equipo_proteccion);
        }

        // POST: /EquipoTrabajo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="iep_id_equipo_p,iep_nombre_equipo_p,iep_ubic_imagen_equipo_p,iep_imagen_equipo_p,iep_stock")] iso_equipo_proteccion iso_equipo_proteccion)
        {

            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_equipo_proteccion.iep_imagen_equipo_p = ConvertToBytes(file);
                }
                db.iso_equipo_proteccion.Add(iso_equipo_proteccion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //return View(iso_equipo_proteccion);
            return PartialView("Create", iso_equipo_proteccion);
        }

        // GET: /EquipoTrabajo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_equipo_proteccion iso_equipo_proteccion = await db.iso_equipo_proteccion.FindAsync(id);
            if (iso_equipo_proteccion == null)
            {
                return HttpNotFound();
            }
            //return View(iso_equipo_proteccion);
            return PartialView("Edit", iso_equipo_proteccion);
        }

        // POST: /EquipoTrabajo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="iep_id_equipo_p,iep_nombre_equipo_p,iep_ubic_imagen_equipo_p,iep_imagen_equipo_p,iep_stock")] iso_equipo_proteccion iso_equipo_proteccion)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_equipo_proteccion.iep_imagen_equipo_p = ConvertToBytes(file);
                }
                db.Entry(iso_equipo_proteccion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("Edit", iso_equipo_proteccion);
        }

        // GET: /EquipoTrabajo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_equipo_proteccion iso_equipo_proteccion = await db.iso_equipo_proteccion.FindAsync(id);
            if (iso_equipo_proteccion == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", iso_equipo_proteccion);
        }

        // POST: /EquipoTrabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_equipo_proteccion iso_equipo_proteccion = await db.iso_equipo_proteccion.FindAsync(id);
            db.iso_equipo_proteccion.Remove(iso_equipo_proteccion);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public FileContentResult GetImage(int ID)
        {
            iso_equipo_proteccion cat = db.iso_equipo_proteccion.FirstOrDefault(c => c.iep_id_equipo_p == ID);
            if (cat != null)
            {

                string type = string.Empty;
                if (!string.IsNullOrEmpty(cat.iep_ubic_imagen_equipo_p))
                {
                    type = cat.iep_ubic_imagen_equipo_p;
                }
                else
                {
                    type = "image/jpeg";
                }
                return File(cat.iep_imagen_equipo_p, type);
            }
            else
            {
                return null;
            }
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
