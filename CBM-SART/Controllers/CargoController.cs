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
    public class CargoController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Cargo/
        public async Task<ActionResult> Index()
        {
            return View(await db.iso_cargo.ToListAsync());
        }

        // GET: /Cargo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return View(iso_cargo);
        }

        // GET: /Cargo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Cargo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="icg_id_cargo,icg_nombre,icg_descripcion,icg_cod_clase_cargo,icg_num_trabajadores,icg_area,icg_jefe_inmediato,icg_edad,icg_genero,icg_instruccion,icg_experiencia,icg_conocimiento_adicional")] iso_cargo iso_cargo)
        {
            if (ModelState.IsValid)
            {
                db.iso_cargo.Add(iso_cargo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iso_cargo);
        }

        // GET: /Cargo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return View(iso_cargo);
        }

        // POST: /Cargo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="icg_id_cargo,icg_nombre,icg_descripcion,icg_cod_clase_cargo,icg_num_trabajadores,icg_area,icg_jefe_inmediato,icg_edad,icg_genero,icg_instruccion,icg_experiencia,icg_conocimiento_adicional")] iso_cargo iso_cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_cargo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iso_cargo);
        }

        // GET: /Cargo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return View(iso_cargo);
        }

        // POST: /Cargo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            db.iso_cargo.Remove(iso_cargo);
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
