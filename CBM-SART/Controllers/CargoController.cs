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

namespace CBM_SART.Controllers
{
    public class CargoController : Controller
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


        public ActionResult PuestoTrabajoList(int? idCargo)
        {
            ////var iso_puesto_trabajo = db.iso_puesto_trabajo.ToList();
            //if (idCargo == null) {
            //    var iso_puesto_trabajo = db.iso_puesto_trabajo.ToList();
            //    return PartialView(iso_puesto_trabajo);
            //}
            //else{
                //iso_puesto_trabajo iso_puesto_trabajo = db.iso_puesto_trabajo.Where(x => x.iso_cargo.Any(y => y.icg_id_cargo == idCargo)).ToList();
            var result = (from m in db.iso_puesto_trabajo
                          from b in m.iso_cargo
                          where b.icg_id_cargo == idCargo
                          select m);
            return PartialView(result);
            //}
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
