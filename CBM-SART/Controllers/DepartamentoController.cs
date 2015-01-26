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
    public class DepartamentoController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Departamento/
        public async Task<ActionResult> Index()
        {
            var iso_departamento = db.iso_departamento.Include(i => i.iso_empresa);
            return View(await iso_departamento.ToListAsync());
        }

        // GET: /Departamento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
            if (iso_departamento == null)
            {
                return HttpNotFound();
            }
            return View(iso_departamento);
        }

        // GET: /Departamento/Create
        public ActionResult Create()
        {
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa");
            return View();
        }

        // POST: /Departamento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ide_id_departamento,ide_nombre_departamento,ide_descripcion_departamento,ide_id_empresa,ide_id_departamento_padre,ide_deshabilitar,ide_cod_documento")] iso_departamento iso_departamento)
        {
            if (ModelState.IsValid)
            {
                db.iso_departamento.Add(iso_departamento);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
            return View(iso_departamento);
        }

        // GET: /Departamento/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
            if (iso_departamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
            return View(iso_departamento);
        }

        // POST: /Departamento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ide_id_departamento,ide_nombre_departamento,ide_descripcion_departamento,ide_id_empresa,ide_id_departamento_padre,ide_deshabilitar,ide_cod_documento")] iso_departamento iso_departamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_departamento).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
            return View(iso_departamento);
        }

        // GET: /Departamento/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
            if (iso_departamento == null)
            {
                return HttpNotFound();
            }
            return View(iso_departamento);
        }

        // POST: /Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
            db.iso_departamento.Remove(iso_departamento);
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
