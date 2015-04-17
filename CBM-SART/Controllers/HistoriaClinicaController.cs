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
    public class HistoriaClinicaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /HistoriaClinica/
        public async Task<ActionResult> Index()
        {
            var iso_historia_clinica = db.iso_historia_clinica.Include(i => i.iso_personal);
            return View(await iso_historia_clinica.ToListAsync());
        }

        // GET: /HistoriaClinica/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = await db.iso_historia_clinica.FindAsync(id);
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Create
        public ActionResult Create()
        {
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal");
            return View();
        }

        // POST: /HistoriaClinica/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ihc_id_historia,ihc_id_personal,ihc_id_empresa,ihc_lugar_historia_clinica,ihc_fecha_historia_clinica")] iso_historia_clinica iso_historia_clinica)
        {
            if (ModelState.IsValid)
            {
                db.iso_historia_clinica.Add(iso_historia_clinica);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = await db.iso_historia_clinica.FindAsync(id);
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // POST: /HistoriaClinica/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ihc_id_historia,ihc_id_personal,ihc_id_empresa,ihc_lugar_historia_clinica,ihc_fecha_historia_clinica")] iso_historia_clinica iso_historia_clinica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_historia_clinica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = await db.iso_historia_clinica.FindAsync(id);
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            return View(iso_historia_clinica);
        }

        // POST: /HistoriaClinica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_historia_clinica iso_historia_clinica = await db.iso_historia_clinica.FindAsync(id);
            db.iso_historia_clinica.Remove(iso_historia_clinica);
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
