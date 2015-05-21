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
using System.Data.Entity.SqlServer;

namespace CBM_SART.Controllers
{
    public class PlanController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Plan/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "ipl_nombre_plan", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_plan>();
            ViewBag.filter = filter;
            records.Content = db.iso_plan
                        .Where(x => filter == null ||
                                (x.ipl_nombre_plan.ToLower().Contains(filter.ToLower().Trim()))
                                || x.ipl_codigo_plan.ToLower().Contains(filter.ToLower().Trim())
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_plan
                         .Where(x => filter == null ||
                                (x.ipl_nombre_plan.ToLower().Contains(filter.ToLower().Trim()))
                                || x.ipl_codigo_plan.ToLower().Contains(filter.ToLower().Trim())).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
        }
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.iso_plan.ToListAsync());
        //}

        // GET: /Plan/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_plan iso_plan = await db.iso_plan.FindAsync(id);
            if (iso_plan == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", iso_plan);
        }

        // GET: /Plan/Create
        public ActionResult Create()
        {
            var iso_plan = new iso_plan();
            return PartialView("Create", iso_plan);
            //return View();
        }

        // POST: /Plan/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ipl_id_plan,ipl_tipo_plan,ipl_nombre_plan,ipl_descripcion_plan,ipl_fecha_creacion_plan,ipl_creador_plan,ipl_tolerancia,ipl_responsable,ipl_codigo_plan,ipl_id_plan_padre,ipl_tipo_proyecto,ipl_objetivo_estrategico,ipl_id_area")] iso_plan iso_plan)
        {
            if (ModelState.IsValid)
            {
                db.iso_plan.Add(iso_plan);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            //return View(iso_plan);
            return PartialView("Create", iso_plan);
        }

        // GET: /Plan/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_plan iso_plan = await db.iso_plan.FindAsync(id);
            if (iso_plan == null)
            {
                return HttpNotFound();
            }
            //return View(iso_plan);
            return PartialView("Edit", iso_plan);
        }

        // POST: /Plan/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ipl_id_plan,ipl_tipo_plan,ipl_nombre_plan,ipl_descripcion_plan,ipl_fecha_creacion_plan,ipl_creador_plan,ipl_tolerancia,ipl_responsable,ipl_codigo_plan,ipl_id_plan_padre,ipl_tipo_proyecto,ipl_objetivo_estrategico,ipl_id_area")] iso_plan iso_plan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_plan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //return View(iso_plan);
            return PartialView("Edit", iso_plan);
        }

        // GET: /Plan/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_plan iso_plan = await db.iso_plan.FindAsync(id);
            if (iso_plan == null)
            {
                return HttpNotFound();
            }
            //return View(iso_plan);
            return PartialView("Delete", iso_plan);
        }

        // POST: /Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_plan iso_plan = await db.iso_plan.FindAsync(id);
            db.iso_plan.Remove(iso_plan);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public ActionResult NuevaTarea(int id)
        {
            var nombreplan = db.iso_plan.Where(x => x.ipl_id_plan == id).ToArray();
            int nroTarea = int.Parse(db.iso_detalle_plan.Where(x => x.idp_id_plan == id).Select(x => x.idp_numero_plan).Max());

            var iso_detalle_plan = new iso_detalle_plan();
            ViewBag.nombreplan = nombreplan;
            ViewBag.nroTarea = nroTarea + 1;
            return PartialView("NuevaTarea", iso_detalle_plan);
        }
        public ActionResult GuardarTarea(int id, iso_detalle_plan iso_detalle_plan)
        {
            iso_detalle_plan.idp_id_plan = id;
            if (ModelState.IsValid)
            {
                db.iso_detalle_plan.Add(iso_detalle_plan);
                db.SaveChanges();
                return RedirectToAction("Tareas/" + id);
            }

            //return View(iso_plan);
            return RedirectToAction("Tareas/" + id);
        }

        public ActionResult EditarTarea(int? id)
        {
            
            //int nroTarea = int.Parse(db.iso_detalle_plan.Where(x => x.idp_id_plan == id).Select(x => x.idp_numero_plan).Max());

            //var iso_detalle_plan = new iso_detalle_plan();
            
            //ViewBag.nroTarea = nroTarea + 1;
            //return PartialView("EditarTarea", iso_detalle_plan);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == id).First();
            var idPlan = iso_detalle_plan.idp_id_plan;
            var nombreplan = db.iso_plan.Where(x => x.ipl_id_plan == idPlan).ToArray();
            ViewBag.nombreplan = nombreplan;
            if (iso_detalle_plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTarea = iso_detalle_plan.idp_id_detalle_plan;
            return PartialView("EditarTarea", iso_detalle_plan);
        }
        public ActionResult EditarTareaSI(int id, iso_detalle_plan iso_detalle_plan)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(iso_detalle_plan).State = EntityState.Modified;
            //    
            //    db.SaveChanges();
            //    return RedirectToAction("Tareas/" + id);
            //}

            ////return View(iso_plan);
            //return PartialView("GuardarTarea/" + id, iso_detalle_plan);
            //iso_detalle_plan iso_detalle_plan_ed = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == id).First();
            var idPlan = iso_detalle_plan.idp_id_plan;
            if (ModelState.IsValid) {
                iso_detalle_plan.idp_id_detalle_plan = id;
                db.Entry(iso_detalle_plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Tareas/" + idPlan);
            }
            return RedirectToAction("Tareas/" + idPlan);
        }

        public ActionResult EliminarTarea(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == id).First();
            if (iso_detalle_plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTarea = iso_detalle_plan.idp_id_detalle_plan;
            return PartialView("EliminarTarea", iso_detalle_plan);
        }
        public ActionResult EliminarTareaSI(int id)
        {
            iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == id).First();
            var idPlan = iso_detalle_plan.idp_id_plan;
            db.iso_detalle_plan.Remove(iso_detalle_plan);
            db.SaveChanges();
            return RedirectToAction("Tareas/" + idPlan);
        }

        public ActionResult Tareas(int id, string filter = null, int page = 1, int pageSize = 15, string sort = "idp_numero_plan", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_detalle_plan>();
            ViewBag.filter = filter;
            ViewBag.id = id;
            var nombreplan = db.iso_plan.Where(x => x.ipl_id_plan == id).ToArray();
            ViewBag.nombreplan = nombreplan;

            records.Content = db.iso_detalle_plan
                        .Where(x => x.idp_id_plan == id)
                        .OrderBy(x => x.idp_numero_plan)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
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
