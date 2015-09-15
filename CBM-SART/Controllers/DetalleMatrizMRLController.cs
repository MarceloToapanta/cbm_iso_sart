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
    public class DetalleMatrizMRLController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /DetalleMartrizMRL/
        public async Task<ActionResult> Index()
        {
            var iso_detalle_matriz_mrl = db.iso_detalle_matriz_mrl.Include(i => i.iso_matriz_mrl).Include(i => i.iso_riesgo_mrl);
            return View(await iso_detalle_matriz_mrl.ToListAsync());
        }

        // GET: /DetalleMartrizMRL/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_matriz_mrl iso_detalle_matriz_mrl = await db.iso_detalle_matriz_mrl.FindAsync(id);
            if (iso_detalle_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            return View(iso_detalle_matriz_mrl);
        }

        // GET: /DetalleMartrizMRL/Create
        public ActionResult Create()
        {
            ViewBag.idm_id_matriz_mrl = new SelectList(db.iso_matriz_mrl, "imr_id_matriz_mrl", "imr_localizacion_mrl");
            ViewBag.idm_id_riesgo_mrl = new SelectList(db.iso_riesgo_mrl, "irm_id_riesgo", "irm_nombre_riesgo");
            return View();
        }

        // POST: /DetalleMartrizMRL/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="idm_id_matriz_mrl,idm_id_riesgo_mrl,idm_id_tipo_riesgo_mrl,idm_probabilidad,idm_consecuencia,idm_exposicion,idm_resultado,idm_valoracion_gp,idm_descripcion")] iso_detalle_matriz_mrl iso_detalle_matriz_mrl)
        {
            if (ModelState.IsValid)
            {
                db.iso_detalle_matriz_mrl.Add(iso_detalle_matriz_mrl);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idm_id_matriz_mrl = new SelectList(db.iso_matriz_mrl, "imr_id_matriz_mrl", "imr_localizacion_mrl", iso_detalle_matriz_mrl.idm_id_matriz_mrl);
            ViewBag.idm_id_riesgo_mrl = new SelectList(db.iso_riesgo_mrl, "irm_id_riesgo", "irm_nombre_riesgo", iso_detalle_matriz_mrl.idm_id_riesgo_mrl);
            return View(iso_detalle_matriz_mrl);
        }

        // GET: /DetalleMartrizMRL/Edit/5
        public async Task<ActionResult> Edit(int idMatriz, int idDetalle, int IdTipoDetalle)
        {
            if (idMatriz == null || idDetalle == null || IdTipoDetalle == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_matriz_mrl iso_detalle_matriz_mrl = await db.iso_detalle_matriz_mrl.FindAsync(idMatriz, idDetalle, IdTipoDetalle);
            if (iso_detalle_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            ViewBag.idm_id_matriz_mrl = new SelectList(db.iso_matriz_mrl, "imr_id_matriz_mrl", "imr_localizacion_mrl", iso_detalle_matriz_mrl.idm_id_matriz_mrl);
            ViewBag.idm_id_riesgo_mrl = new SelectList(db.iso_riesgo_mrl, "irm_id_riesgo", "irm_nombre_riesgo", iso_detalle_matriz_mrl.idm_id_riesgo_mrl);
            return PartialView(iso_detalle_matriz_mrl);
        }

        // POST: /DetalleMartrizMRL/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="idm_id_matriz_mrl,idm_id_riesgo_mrl,idm_id_tipo_riesgo_mrl,idm_probabilidad,idm_consecuencia,idm_exposicion,idm_resultado,idm_valoracion_gp,idm_descripcion")] iso_detalle_matriz_mrl iso_detalle_matriz_mrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_detalle_matriz_mrl).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Redirect("/MatrizMRL/Riesgos/" + iso_detalle_matriz_mrl.idm_id_matriz_mrl);
            }
            ViewBag.idm_id_matriz_mrl = new SelectList(db.iso_matriz_mrl, "imr_id_matriz_mrl", "imr_localizacion_mrl", iso_detalle_matriz_mrl.idm_id_matriz_mrl);
            ViewBag.idm_id_riesgo_mrl = new SelectList(db.iso_riesgo_mrl, "irm_id_riesgo", "irm_nombre_riesgo", iso_detalle_matriz_mrl.idm_id_riesgo_mrl);
            return View(iso_detalle_matriz_mrl);
        }

        // GET: /DetalleMartrizMRL/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_matriz_mrl iso_detalle_matriz_mrl = await db.iso_detalle_matriz_mrl.FindAsync(id);
            if (iso_detalle_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            return View(iso_detalle_matriz_mrl);
        }

        // POST: /DetalleMartrizMRL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_detalle_matriz_mrl iso_detalle_matriz_mrl = await db.iso_detalle_matriz_mrl.FindAsync(id);
            db.iso_detalle_matriz_mrl.Remove(iso_detalle_matriz_mrl);
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
