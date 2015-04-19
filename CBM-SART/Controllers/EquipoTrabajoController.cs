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

namespace CBM_SART.Controllers
{
    public class EquipoTrabajoController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /EquipoTrabajo/
        public async Task<ActionResult> Index()
        {
            return View(await db.iso_equipo_proteccion.ToListAsync());
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
            return View(iso_equipo_proteccion);
        }

        // GET: /EquipoTrabajo/Create
        public ActionResult Create()
        {
            return View();
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
                db.iso_equipo_proteccion.Add(iso_equipo_proteccion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iso_equipo_proteccion);
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
            return View(iso_equipo_proteccion);
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
                db.Entry(iso_equipo_proteccion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iso_equipo_proteccion);
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
            return View(iso_equipo_proteccion);
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
