using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CBM_SART.Models;
using System.Linq.Dynamic;
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class DepartamentoController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Departamento/
        //public async Task<ActionResult> Index()
        //{
        //    var iso_departamento = db.iso_departamento.Include(i => i.iso_empresa);
        //    return View(await iso_departamento.ToListAsync());
        //}
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 20, string sort = "ide_id_departamento", string sortdir = "ASC")
        {
            int ide = IdEmpresa();
            ViewBag.nombre_empresa = NombreEmpresa();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_departamento>();
            ViewBag.filter = filter;
            records.Content = db.iso_departamento
                        .Where(x => filter == null ||
                                (x.ide_nombre_departamento.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .Where(x => x.ide_id_empresa == ide)
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_departamento
                         .Where(x => filter == null ||
                                (x.ide_nombre_departamento.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
        }

        // GET: /Departamento/Details/5
        public async Task<ActionResult> Details(int? id=0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa");
            iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
            if (iso_departamento == null)
            {
                return HttpNotFound();
            }
            //return View(iso_departamento);
            return PartialView("Details", iso_departamento);
        }

        // GET: /Departamento/Create
        public ActionResult Create()
        {
            
            var iso_departamento = new iso_departamento();
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
            return PartialView("Create", iso_departamento);
            //return View();

        }

        // POST: /Departamento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include="ide_id_departamento,ide_nombre_departamento,ide_descripcion_departamento,ide_id_empresa,ide_id_departamento_padre,ide_deshabilitar,ide_cod_documento")] iso_departamento iso_departamento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.iso_departamento.Add(iso_departamento);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
        //    return View(iso_departamento);
        //}
        public ActionResult Create(iso_departamento iso_departamento)
        //public ActionResult Create([Bind(Include = "iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.iso_departamento.Add(iso_departamento);
                db.SaveChanges();
                return RedirectToAction("Index");
                //return RedirectToAction("Index");
            }
            //return Json(iso_departamento, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }

        // GET: /Departamento/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
        //    if (iso_departamento == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
        //    return View(iso_departamento);
        //}
        public ActionResult Edit(int? id = 0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            
            iso_departamento iso_departamento = db.iso_departamento.Find(id);
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
            if (iso_departamento == null)
            {
             
            }
            //return View(iso_empresa);
            return PartialView("Edit", iso_departamento);
        }

        // POST: /Departamento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include="ide_id_departamento,ide_nombre_departamento,ide_descripcion_departamento,ide_id_empresa,ide_id_departamento_padre,ide_deshabilitar,ide_cod_documento")] iso_departamento iso_departamento)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(iso_departamento).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa", iso_departamento.ide_id_empresa);
        //    return View(iso_departamento);
        //}
        public ActionResult Edit(iso_departamento iso_departamento)
        //public ActionResult Edit([Bind(Include = "iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_departamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("Edit", iso_departamento);
        }

        // GET: /Departamento/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
        //    if (iso_departamento == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(iso_departamento);
        //}
        public ActionResult Delete(int? id = 0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ViewBag.ide_id_empresa = new SelectList(db.iso_empresa, "iem_cod_empresa", "iem_nombre_empresa");
            iso_departamento iso_departamento = db.iso_departamento.Find(id);
            if (iso_departamento == null)
            {
                return HttpNotFound();
            }
            //return View(iso_empresa);
            return PartialView("Delete", iso_departamento);
        }

        // POST: /Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    iso_departamento iso_departamento = await db.iso_departamento.FindAsync(id);
        //    db.iso_departamento.Remove(iso_departamento);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        public ActionResult DeleteConfirmed(int id)
        {
            
            var phone = db.iso_departamento.Find(id);
            db.iso_departamento.Remove(phone);
            db.SaveChanges();
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
