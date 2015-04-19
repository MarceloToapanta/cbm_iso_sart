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
    public class ProfesiogramaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Profesiograma/
        public async Task<ActionResult> Index()
        {
            var iso_matriz_profesiograma = db.iso_matriz_profesiograma.Include(i => i.iso_cargo).Include(i => i.iso_puesto_trabajo);
            return View(await iso_matriz_profesiograma.ToListAsync());
        }

        // GET: /Profesiograma/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_profesiograma iso_matriz_profesiograma = await db.iso_matriz_profesiograma.FindAsync(id);
            if (iso_matriz_profesiograma == null)
            {
                return HttpNotFound();
            }
            return View(iso_matriz_profesiograma);
        }

        // GET: /Profesiograma/Create
        public ActionResult Create()
        {
            ViewBag.imp_id_cargo = new SelectList(db.iso_cargo, "icg_id_cargo", "icg_nombre");
            ViewBag.imp_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo, "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return View();
        }

        // POST: /Profesiograma/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="imp_id_matrizp,imp_id_cargo,imp_id_area,imp_id_puesto_trabajo,imp_formacion,imp_experiencia,imp_aptitudes,imp_actitudes,imp_tareas_funciones,imp_herramientas,imp_exigencias,imp_competencias,imp_capacitacion,imp_horario_trabajo,imp_fecha_creacion,imp_preocupacional,imp_per_reintegro,imp_reintegro,imp_especial,imp_salida,imp_absoluta,imp_relativa,imp_edad,imp_genero,imp_criterios_exclusion,imp_conocimientos_ad")] iso_matriz_profesiograma iso_matriz_profesiograma)
        {
            if (ModelState.IsValid)
            {
                db.iso_matriz_profesiograma.Add(iso_matriz_profesiograma);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.imp_id_cargo = new SelectList(db.iso_cargo, "icg_id_cargo", "icg_nombre", iso_matriz_profesiograma.imp_id_cargo);
            ViewBag.imp_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo, "ipt_id_puesto_t", "ipt_nombre_puesto_t", iso_matriz_profesiograma.imp_id_puesto_trabajo);
            return View(iso_matriz_profesiograma);
        }

        // GET: /Profesiograma/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_profesiograma iso_matriz_profesiograma = await db.iso_matriz_profesiograma.FindAsync(id);
            if (iso_matriz_profesiograma == null)
            {
                return HttpNotFound();
            }
            ViewBag.imp_id_cargo = new SelectList(db.iso_cargo, "icg_id_cargo", "icg_nombre", iso_matriz_profesiograma.imp_id_cargo);
            ViewBag.imp_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo, "ipt_id_puesto_t", "ipt_nombre_puesto_t", iso_matriz_profesiograma.imp_id_puesto_trabajo);
            return View(iso_matriz_profesiograma);
        }

        // POST: /Profesiograma/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="imp_id_matrizp,imp_id_cargo,imp_id_area,imp_id_puesto_trabajo,imp_formacion,imp_experiencia,imp_aptitudes,imp_actitudes,imp_tareas_funciones,imp_herramientas,imp_exigencias,imp_competencias,imp_capacitacion,imp_horario_trabajo,imp_fecha_creacion,imp_preocupacional,imp_per_reintegro,imp_reintegro,imp_especial,imp_salida,imp_absoluta,imp_relativa,imp_edad,imp_genero,imp_criterios_exclusion,imp_conocimientos_ad")] iso_matriz_profesiograma iso_matriz_profesiograma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_matriz_profesiograma).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.imp_id_cargo = new SelectList(db.iso_cargo, "icg_id_cargo", "icg_nombre", iso_matriz_profesiograma.imp_id_cargo);
            ViewBag.imp_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo, "ipt_id_puesto_t", "ipt_nombre_puesto_t", iso_matriz_profesiograma.imp_id_puesto_trabajo);
            return View(iso_matriz_profesiograma);
        }

        // GET: /Profesiograma/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_profesiograma iso_matriz_profesiograma = await db.iso_matriz_profesiograma.FindAsync(id);
            if (iso_matriz_profesiograma == null)
            {
                return HttpNotFound();
            }
            return View(iso_matriz_profesiograma);
        }

        // POST: /Profesiograma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_matriz_profesiograma iso_matriz_profesiograma = await db.iso_matriz_profesiograma.FindAsync(id);
            db.iso_matriz_profesiograma.Remove(iso_matriz_profesiograma);
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
