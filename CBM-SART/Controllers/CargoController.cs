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
    public class CargoController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Cargo/
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.iso_cargo.ToListAsync());
        //}
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "icg_nombre", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_cargo>();
            ViewBag.filter = filter;
            records.Content = db.iso_cargo
                        .Where(x => filter == null ||
                                (x.icg_nombre.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_cargo
                         .Where(x => filter == null ||
                                (x.icg_nombre.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            ViewBag.totalpuestos = db.iso_puesto_trabajo.Count();

            return View(records);

            //return View(db.iso_empresa.ToList());
        }

        // GET: /Cargo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.icg_area = new SelectList(db.iso_departamento, "ide_id_departamento", "ide_nombre_departamento");
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", iso_cargo);
        }

        // GET: /Cargo/Create
        public ActionResult Create()
        {
            var iso_cargo = new iso_cargo();
            ViewBag.icg_area = new SelectList(db.iso_departamento, "ide_id_departamento", "ide_nombre_departamento");
            return PartialView("Create", iso_cargo);
        }

        // POST: /Cargo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "icg_id_cargo,icg_nombre,icg_descripcion,icg_cod_clase_cargo,icg_num_trabajadores,icg_area,icg_jefe_inmediato,icg_edad,icg_genero,icg_instruccion,icg_experiencia,icg_conocimiento_adicional")] iso_cargo iso_cargo)
        {
            if (ModelState.IsValid)
            {
                db.iso_cargo.Add(iso_cargo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("Create", iso_cargo);
        }

        // GET: /Cargo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.icg_area = new SelectList(db.iso_departamento, "ide_id_departamento", "ide_nombre_departamento");
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", iso_cargo);
        }

        // POST: /Cargo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "icg_id_cargo,icg_nombre,icg_descripcion,icg_cod_clase_cargo,icg_num_trabajadores,icg_area,icg_jefe_inmediato,icg_edad,icg_genero,icg_instruccion,icg_experiencia,icg_conocimiento_adicional")] iso_cargo iso_cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_cargo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("Edit", iso_cargo);
        }

        // GET: /Cargo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.icg_area = new SelectList(db.iso_departamento, "ide_id_departamento", "ide_nombre_departamento");
            iso_cargo iso_cargo = await db.iso_cargo.FindAsync(id);
            if (iso_cargo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", iso_cargo);
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
        public ActionResult AsignarPuestoT(int id) {
            iso_cargo iso_cargo = db.iso_cargo.Find(id);
            //Asignados
            var puestosasignados = iso_cargo.iso_puesto_trabajo;
            ViewBag.puestos_asignados = puestosasignados;
            //Disponibles
            var puestosdisponibles = db.iso_puesto_trabajo.ToList();
            var totalpuestosdisponibles = puestosdisponibles.Except(puestosasignados);
            ViewBag.puestosdisponibles = totalpuestosdisponibles;
            return PartialView("AsignarPuestoT",iso_cargo);
        }
        public ActionResult PuestosAsignados(int id)
        {
            if (id != 0)
            {
                var iso_cargo = db.iso_cargo.Find(id);
                var puestos = iso_cargo.iso_puesto_trabajo.Select((x=>new{
                    x.ipt_id_puesto_t, x.ipt_nro_trbajadores, x.ipt_nombre_puesto_t}));
                return this.Json(puestos, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var puestos = db.iso_puesto_trabajo.Select((x => new {
                    x.ipt_id_puesto_t,
                    x.ipt_nro_trbajadores,
                    x.ipt_nombre_puesto_t
                }));
                return this.Json(puestos.ToList(), JsonRequestBehavior.AllowGet);
            }

            
        }


        public ActionResult PuestoTrabajoList(int id, string filter = null, int page = 1, int pageSize = 15, string sort = "icg_nombre", string sortdir = "ASC")
        {
            if (id <= 0)
            {
                var recordsvacio = new PagedList<iso_puesto_trabajo>();
                recordsvacio.Content = db.iso_puesto_trabajo
                        .OrderBy(x => x.ipt_nombre_puesto_t)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                ViewBag.filter = filter;
                recordsvacio.PageSize = pageSize;
                recordsvacio.TotalRecords = db.iso_puesto_trabajo.Count();
                ViewBag.total = recordsvacio.TotalRecords;
                return PartialView(recordsvacio);
            }


            var iso_cargo = db.iso_cargo.Find(id);
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_puesto_trabajo>();
            ViewBag.filter = filter;
            records.Content = iso_cargo.iso_puesto_trabajo
                        .Where(x => filter == null ||
                                (x.ipt_nombre_puesto_t.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            //// Count
            records.TotalRecords = iso_cargo.iso_puesto_trabajo
                         .Where(x => filter == null ||
                                (x.ipt_nombre_puesto_t.ToLower().Contains(filter.ToLower().Trim()))).Count();
            //records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            return PartialView(records);
        }
        public String AnadirPuestoT(int idcargo, int[] puestos)
        {
            //Bueca el cargo
            iso_cargo iso_cargo = db.iso_cargo.Find(idcargo);
            //Añade puestos
            for (int i = 0; i < puestos.Length; i++)
            {
                iso_puesto_trabajo ord = new iso_puesto_trabajo();
                ord  = db.iso_puesto_trabajo.Find(puestos[i]);
                if (ord != null)
                {
                    iso_cargo.iso_puesto_trabajo.Add(ord);
                }
                //str = str + puestos[i];
            }
            db.SaveChanges();
            return "";
        }
        public ActionResult QuitarPuestoT(int idcargo, int[] puestos)
        {
            //Bueca el cargo
            iso_cargo iso_cargo = db.iso_cargo.Find(idcargo);
            //Añade puestos
            for (int i = 0; i < puestos.Length; i++)
            {
                iso_puesto_trabajo ord = new iso_puesto_trabajo();
                ord = db.iso_puesto_trabajo.Find(puestos[i]);
                if (ord != null)
                {
                    iso_cargo.iso_puesto_trabajo.Remove(ord);
                }
                //str = str + puestos[i];
            }
            db.SaveChanges();
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
