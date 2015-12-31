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
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class IncidenteController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Incidente/
        public async Task<ActionResult> Index()
        {
            var iso_incidente_trabajo = db.iso_incidente_trabajo.Include(i => i.iso_personal);
            return View(await iso_incidente_trabajo.ToListAsync());
        }

        // GET: /Incidente/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_incidente_trabajo iso_incidente_trabajo = await db.iso_incidente_trabajo.FindAsync(id);
            if (iso_incidente_trabajo == null)
            {
                return HttpNotFound();
            }
            return View(iso_incidente_trabajo);
        }

        // GET: /Incidente/Create
        public ActionResult Create()
        {
            ViewBag.iit_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal");
            return View();
        }

        // POST: /Incidente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="iit_id_personal,iit_id_incidente,iit_fecha_incidente,iit_descripcion_incid,iit_tratamiento_rec,iit_tiempo_reposo,iit_lugar,iit_hora")] iso_incidente_trabajo iso_incidente_trabajo)
        {
            if (ModelState.IsValid)
            {
                db.iso_incidente_trabajo.Add(iso_incidente_trabajo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.iit_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_incidente_trabajo.iit_id_personal);
            return View(iso_incidente_trabajo);
        }

        // GET: /Incidente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_incidente_trabajo iso_incidente_trabajo = await db.iso_incidente_trabajo.FindAsync(id);
            if (iso_incidente_trabajo == null)
            {
                return HttpNotFound();
            }
            ViewBag.iit_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_incidente_trabajo.iit_id_personal);
            return View(iso_incidente_trabajo);
        }

        // POST: /Incidente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="iit_id_personal,iit_id_incidente,iit_fecha_incidente,iit_descripcion_incid,iit_tratamiento_rec,iit_tiempo_reposo,iit_lugar,iit_hora")] iso_incidente_trabajo iso_incidente_trabajo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_incidente_trabajo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.iit_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_incidente_trabajo.iit_id_personal);
            return View(iso_incidente_trabajo);
        }

        // GET: /Incidente/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_incidente_trabajo iso_incidente_trabajo = await db.iso_incidente_trabajo.FindAsync(id);
            if (iso_incidente_trabajo == null)
            {
                return HttpNotFound();
            }
            return View(iso_incidente_trabajo);
        }

        // POST: /Incidente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_incidente_trabajo iso_incidente_trabajo = await db.iso_incidente_trabajo.FindAsync(id);
            db.iso_incidente_trabajo.Remove(iso_incidente_trabajo);
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
