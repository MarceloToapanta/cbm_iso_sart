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
    public class AccidenteController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Accidente/
        public async Task<ActionResult> Index()
        {
            var iso_accidente = db.iso_accidente.Include(i => i.iso_personal);
            return View(await iso_accidente.ToListAsync());
        }

        // GET: /Accidente/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_accidente iso_accidente = await db.iso_accidente.FindAsync(id);
            if (iso_accidente == null)
            {
                return HttpNotFound();
            }
            return View(iso_accidente);
        }

        // GET: /Accidente/Create
        public ActionResult Create()
        {
            ViewBag.iac_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal");
            return View();
        }

        // POST: /Accidente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="iac_id_accidente,iac_id_personal,iac_cod_empresa,iac_dia_accidente,iac_fecha_accidente,iac_hora_accidente,iac_sitio_accidente,iac_descrip_accidente,iac_lesiones_accidente,iac_experiencia_trab,iac_entrenamiento_trab,iac_produjo_lesion,iac_falla_accidente,iac_nombres_testigos,iac_persona_atendio,iac_traslado_accidentado")] iso_accidente iso_accidente)
        {
            if (ModelState.IsValid)
            {
                db.iso_accidente.Add(iso_accidente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.iac_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_accidente.iac_id_personal);
            return View(iso_accidente);
        }

        // GET: /Accidente/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_accidente iso_accidente = await db.iso_accidente.FindAsync(id);
            if (iso_accidente == null)
            {
                return HttpNotFound();
            }
            ViewBag.iac_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_accidente.iac_id_personal);
            return View(iso_accidente);
        }

        // POST: /Accidente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="iac_id_accidente,iac_id_personal,iac_cod_empresa,iac_dia_accidente,iac_fecha_accidente,iac_hora_accidente,iac_sitio_accidente,iac_descrip_accidente,iac_lesiones_accidente,iac_experiencia_trab,iac_entrenamiento_trab,iac_produjo_lesion,iac_falla_accidente,iac_nombres_testigos,iac_persona_atendio,iac_traslado_accidentado")] iso_accidente iso_accidente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_accidente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.iac_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_accidente.iac_id_personal);
            return View(iso_accidente);
        }

        // GET: /Accidente/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_accidente iso_accidente = await db.iso_accidente.FindAsync(id);
            if (iso_accidente == null)
            {
                return HttpNotFound();
            }
            return View(iso_accidente);
        }

        // POST: /Accidente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_accidente iso_accidente = await db.iso_accidente.FindAsync(id);
            db.iso_accidente.Remove(iso_accidente);
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
