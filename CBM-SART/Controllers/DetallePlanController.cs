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
    public class DetallePlanController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /DetallePlan/
        public async Task<ActionResult> Index()
        {
            var iso_detalle_plan = db.iso_detalle_plan.Include(i => i.iso_plan);
            return View(await iso_detalle_plan.ToListAsync());
        }

        // GET: /DetallePlan/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_plan iso_detalle_plan = await db.iso_detalle_plan.FindAsync(id);
            if (iso_detalle_plan == null)
            {
                return HttpNotFound();
            }
            return View(iso_detalle_plan);
        }

        // GET: /DetallePlan/Create
        public ActionResult Create()
        {
            
            ViewBag.idp_id_plan = new SelectList(db.iso_plan, "ipl_id_plan", "ipl_nombre_plan");
            return View();
        }

        // POST: /DetallePlan/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="idp_id_detalle_plan,idp_id_plan,idp_numero_plan,idp_tarea,idp_observacion,idp_fecha_comienzo,idp_fecha_fin,idp_duracion,idp_costo,idp_porcentaje,idp_estado,idp_id_plan_padre,idp_descripcion,idp_fecha_creacion,idp_fecha_modificacion,idp_verificar_accion,idp_unidad,idp_cantidad,idp_tiempoded,idp_tiemponeto,idp_avancep,idp_objetivo_estrategico,idp_entregable,idp_id_departamento,idp_costo_ejecutado,idp_id_proceso,idp_peso")] iso_detalle_plan iso_detalle_plan)
        {
            if (ModelState.IsValid)
            {
                db.iso_detalle_plan.Add(iso_detalle_plan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idp_id_plan = new SelectList(db.iso_plan, "ipl_id_plan", "ipl_tipo_plan", iso_detalle_plan.idp_id_plan);
            return View(iso_detalle_plan);
        }

        // GET: /DetallePlan/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_plan iso_detalle_plan = await db.iso_detalle_plan.FindAsync(id);
            if (iso_detalle_plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.idp_id_plan = new SelectList(db.iso_plan, "ipl_id_plan", "ipl_tipo_plan", iso_detalle_plan.idp_id_plan);
            return View(iso_detalle_plan);
        }

        // POST: /DetallePlan/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="idp_id_detalle_plan,idp_id_plan,idp_numero_plan,idp_tarea,idp_observacion,idp_fecha_comienzo,idp_fecha_fin,idp_duracion,idp_costo,idp_porcentaje,idp_estado,idp_id_plan_padre,idp_descripcion,idp_fecha_creacion,idp_fecha_modificacion,idp_verificar_accion,idp_unidad,idp_cantidad,idp_tiempoded,idp_tiemponeto,idp_avancep,idp_objetivo_estrategico,idp_entregable,idp_id_departamento,idp_costo_ejecutado,idp_id_proceso,idp_peso")] iso_detalle_plan iso_detalle_plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_detalle_plan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idp_id_plan = new SelectList(db.iso_plan, "ipl_id_plan", "ipl_tipo_plan", iso_detalle_plan.idp_id_plan);
            return View(iso_detalle_plan);
        }

        // GET: /DetallePlan/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_plan iso_detalle_plan = await db.iso_detalle_plan.FindAsync(id);
            if (iso_detalle_plan == null)
            {
                return HttpNotFound();
            }
            return View(iso_detalle_plan);
        }

        // POST: /DetallePlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_detalle_plan iso_detalle_plan = await db.iso_detalle_plan.FindAsync(id);
            db.iso_detalle_plan.Remove(iso_detalle_plan);
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
