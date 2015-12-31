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
using System.Linq.Dynamic;
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class MatrizMRLController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /MatrizMRL/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "imr_puesto_trabajo_mrl", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_matriz_mrl>();
            ViewBag.filter = filter;
            records.Content = db.iso_matriz_mrl
                        .Where(x => filter == null ||
                                (x.imr_puesto_trabajo_mrl.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_matriz_mrl
                         .Where(x => filter == null ||
                                (x.imr_puesto_trabajo_mrl.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);
            //return View(db.iso_empresa.ToList());
        }
        //Riesgos
        public ActionResult Riesgos(int? id, int tipoRiesgo = 1, string filter = null, int page = 1, int pageSize = 25, string sort = "idm_id_riesgo_mrl", string sortdir = "ASC")
        {
            //var iso_detalle_matriz_mrl = db.iso_detalle_matriz_mrl.Where(x => x.idm_id_matriz_mrl == id).ToList();
            //return View(iso_detalle_matriz_mrl);
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_detalle_matriz_mrl>();
            ViewBag.filter = filter;
            ViewBag.tipoRiesgo = tipoRiesgo;
            records.Content = db.iso_detalle_matriz_mrl
                        .Where(x => x.idm_id_tipo_riesgo_mrl == tipoRiesgo && x.idm_id_matriz_mrl == id)
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_detalle_matriz_mrl
                                .Where(x => x.idm_id_tipo_riesgo_mrl == tipoRiesgo && x.idm_id_matriz_mrl == id)
                                .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);
        }
        
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_mrl iso_matriz_mrl = await db.iso_matriz_mrl.FindAsync(id);
            if (iso_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            return View(iso_matriz_mrl);
        }

        // GET: /MatrizMRL/Create
        public ActionResult Create()
        {
            ViewBag.imr_localizacion_mrl = new SelectList(db.iso_departamento.OrderBy("ide_nombre_departamento asc"), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.imr_cargo_mrl = new SelectList(db.iso_cargo.OrderBy("icg_nombre asc"), "icg_id_cargo", "icg_nombre");
            return PartialView();
        }

        // POST: /MatrizMRL/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="imr_id_matriz_mrl,imr_localizacion_mrl,imr_proceso_mrl,imr_sub_proceso_mrl,imr_cargo_mrl,imr_puesto_trabajo_mrl,imr_nro_tabajadores_mrl,imr_jefe_area_mrl,imr_fecha_evaluacion_mrl,imr_descripcion_mrl,imr_id_puesto_t_mrl,imr_ubicacion")] iso_matriz_mrl iso_matriz_mrl)
        {
            if (ModelState.IsValid)
            {
                string imr_localizacion_mrl = db.iso_puesto_trabajo.Where(x => x.ipt_id_puesto_t == int.Parse(iso_matriz_mrl.imr_localizacion_mrl)).First().ToString();
                iso_matriz_mrl.imr_localizacion_mrl = "";
                iso_matriz_mrl.imr_cargo_mrl = "";
                iso_matriz_mrl.imr_puesto_trabajo_mrl = "";
                db.iso_matriz_mrl.Add(iso_matriz_mrl);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iso_matriz_mrl);
        }

        // GET: /MatrizMRL/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_mrl iso_matriz_mrl = await db.iso_matriz_mrl.FindAsync(id);
            if (iso_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            return View(iso_matriz_mrl);
        }

        // POST: /MatrizMRL/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="imr_id_matriz_mrl,imr_localizacion_mrl,imr_proceso_mrl,imr_sub_proceso_mrl,imr_cargo_mrl,imr_puesto_trabajo_mrl,imr_nro_tabajadores_mrl,imr_jefe_area_mrl,imr_fecha_evaluacion_mrl,imr_descripcion_mrl,imr_id_puesto_t_mrl,imr_ubicacion")] iso_matriz_mrl iso_matriz_mrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_matriz_mrl).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iso_matriz_mrl);
        }

        // GET: /MatrizMRL/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_matriz_mrl iso_matriz_mrl = await db.iso_matriz_mrl.FindAsync(id);
            if (iso_matriz_mrl == null)
            {
                return HttpNotFound();
            }
            return View(iso_matriz_mrl);
        }

        // POST: /MatrizMRL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_matriz_mrl iso_matriz_mrl = await db.iso_matriz_mrl.FindAsync(id);
            db.iso_matriz_mrl.Remove(iso_matriz_mrl);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditarRiesgo(int idMatriz, int idDetalle, int IdTipoDetalle)
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
            return PartialView(iso_detalle_matriz_mrl);
        }

        public ActionResult PuestosPorCargo(int id)
        {
            if (id != 0)
            {
                var iso_cargo = db.iso_cargo.Find(id);
                var puestos = iso_cargo.iso_puesto_trabajo.Select((x => new
                {
                    x.ipt_id_puesto_t,
                    x.ipt_nro_trbajadores,
                    x.ipt_nombre_puesto_t
                }));
                return this.Json(puestos, JsonRequestBehavior.AllowGet);
            }
            return null;
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
