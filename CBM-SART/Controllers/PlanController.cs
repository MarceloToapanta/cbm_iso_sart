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
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class PlanController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Plan/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "ipl_fecha_creacion_plan", string sortdir = "DESC")
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
            //ViewBag.Ususarios = new SelectList(db., "iem_cod_empresa", "iem_nombre_empresa");
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
                //add first Task
                iso_detalle_plan iso_detalle_plan = new iso_detalle_plan();
                iso_detalle_plan.idp_id_plan = db.iso_plan.Select(x => x.ipl_id_plan).Max();
                iso_detalle_plan.idp_numero_plan = 0;
                iso_detalle_plan.idp_tarea = iso_plan.ipl_nombre_plan;
                iso_detalle_plan.idp_fecha_comienzo = iso_plan.ipl_fecha_creacion_plan;
                iso_detalle_plan.idp_fecha_fin = iso_plan.ipl_fecha_creacion_plan;
                iso_detalle_plan.idp_cantidad = "0";
                db.iso_detalle_plan.Add(iso_detalle_plan);
                db.SaveChanges();

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
                //Edit First task
                iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan
                    .Where(x => x.idp_id_plan == iso_plan.ipl_id_plan)
                    .Where(x => x.idp_numero_plan == 0).First();
                iso_detalle_plan.idp_tarea = iso_plan.ipl_nombre_plan;
                db.Entry(iso_detalle_plan).State = EntityState.Modified;
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
        public ActionResult NuevaTarea(int id, int? Nro)
        {
            var nombreplan = db.iso_plan.Where(x => x.ipl_id_plan == id).ToArray();
            int nroTarea = db.iso_detalle_plan.Where(x => x.idp_id_plan == id).Select(x => x.idp_numero_plan).Max();

            var iso_detalle_plan = new iso_detalle_plan();
            ViewBag.nombreplan = nombreplan;
            if (Nro > 0)
            {
                ViewBag.nroTarea = Nro;
            }
            else
            {
                ViewBag.nroTarea = nroTarea + 1;
            }
            
            return PartialView("NuevaTarea", iso_detalle_plan);
        }
        public ActionResult GuardarTarea(int id, iso_detalle_plan iso_detalle_plan)
        {

            iso_detalle_plan.idp_id_plan = id;
            if (ModelState.IsValid)
            {
                //Mueve uno mas arriba si es tarea insertada
                int Nro = iso_detalle_plan.idp_numero_plan;
                PonerTarea(Nro, id);

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
            //importante hacer esto antes de eliminar tarea
            QuitarTarea(id);
            db.iso_detalle_plan.Remove(iso_detalle_plan);
            db.SaveChanges();
            
            return RedirectToAction("Tareas/" + idPlan);
        }
        public void QuitarTarea(int idTareaEliminada){
            iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == idTareaEliminada).First();
            int Max = db.iso_detalle_plan.Where(x => x.idp_id_plan == iso_detalle_plan.idp_id_plan).Select(x => x.idp_numero_plan).Max();
            int NroTarea = iso_detalle_plan.idp_numero_plan;
            if (NroTarea < Max){
                //Selecionar las tareas mayores a NroTarea
                var TareasQuery = from d in db.iso_detalle_plan
                                       orderby d.idp_numero_plan
                                       where (d.idp_id_plan == iso_detalle_plan.idp_id_plan) 
                                       select d;
                //Actualizara en -1 cada tarea
                foreach (iso_detalle_plan us in TareasQuery)
                {
                    int NroT = us.idp_numero_plan;
                    if (NroT > NroTarea) { 
                        us.idp_numero_plan = us.idp_numero_plan - 1 ;
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }
        public void PonerTarea(int NroTarea, int IdPlan)
        {
            //iso_detalle_plan iso_detalle_plan = db.iso_detalle_plan.Where(x => x.idp_id_detalle_plan == idTareaEliminada).First();
            int Max = db.iso_detalle_plan.Where(x => x.idp_id_plan == IdPlan).Select(x => x.idp_numero_plan).Max();
            //int NroTarea = int.Parse(iso_detalle_plan.idp_numero_plan);
            if (NroTarea <= Max)
            {
                //Selecionar las tareas mayores a NroTarea
                var TareasQuery = from d in db.iso_detalle_plan
                                  orderby d.idp_numero_plan
                                  where (d.idp_id_plan == IdPlan)
                                  select d;
                //Actualizara en +1 cada tarea
                foreach (iso_detalle_plan us in TareasQuery)
                {
                    int NroT = us.idp_numero_plan;
                    if (NroT >= NroTarea)
                    {
                        us.idp_numero_plan = us.idp_numero_plan + 1;
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }
        public ActionResult Tareas(int id, string filter = null, int page = 1, int pageSize = 100, string sort = "idp_numero_plan", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_detalle_plan>();
            ViewBag.filter = filter;
            ViewBag.id = id;
            //Ajuste Fechas
            AjustarFechas(id);
            //
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
                         .Where(x => x.idp_id_plan == id).Count() -1;

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            //Count Tareas
            int t1 = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_estado == "Completo").Where(x => x.idp_numero_plan > 0)
                         .Count();
            ViewBag.t1 = t1;
            int t2 = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_estado == "Pendiente").Where(x => x.idp_numero_plan > 0)
                         .Count();
            ViewBag.t2 = t2;
            int t3 = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_estado == "Atrasado").Where(x => x.idp_numero_plan > 0)
                         .Count();
            ViewBag.t3 = t3;
            int t4 = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_estado == null || (x.idp_estado == "")).Where(x => x.idp_numero_plan > 0)
                         .Count();
            ViewBag.t4 = t4;
            //FechaTarea 0
            DateTime? fechaini = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_numero_plan == 0)
                         .Select(x => x.idp_fecha_comienzo).First();
            ViewBag.fechaini = fechaini;
            DateTime? fechafin = db.iso_detalle_plan
                         .Where(x => x.idp_id_plan == id).Where(x => x.idp_numero_plan == 0)
                         .Select(x => x.idp_fecha_fin).First();
            ViewBag.fechafin = fechafin;
            return View(records);

            //return View(db.iso_empresa.ToList());
        }
        public void AjustarFechas(int IdPlan)
        {
            //Ajustar Fecha Proyecto
            //1.-Selecionar Tarea 0
            iso_detalle_plan TareaCero = db.iso_detalle_plan
                .Where(x => x.idp_id_plan == IdPlan)
                .Where(x => x.idp_numero_plan == 0).First();
            //2.-Buscar Fecha Minima y Maxima
            DateTime? MinFecha = db.iso_detalle_plan
                .Where(x => x.idp_id_plan == IdPlan)
                .Where(x => x.idp_numero_plan > 0)
                .Select(x => x.idp_fecha_comienzo).Min();
            DateTime? MaxFecha = db.iso_detalle_plan
                .Where(x => x.idp_id_plan == IdPlan)
                .Where(x => x.idp_numero_plan > 0)
                .Select(x => x.idp_fecha_fin).Max();
            //3.-Acutaliar Fechas encontradas a Tarea 0
            if (MinFecha != null){
                TareaCero.idp_fecha_comienzo = MinFecha;
            }
            if (MaxFecha != null)
            {
                TareaCero.idp_fecha_fin = MaxFecha;
            }
            
            //Ajustar Cantidad
            //1.- Encontrar Suma de las Cantidades
            var Suma = db.iso_detalle_plan
                .Where(x => x.idp_id_plan == IdPlan)
                .Where(x => x.idp_numero_plan > 0);
            int SumaTotal = 0;
            foreach (iso_detalle_plan us in Suma)
            {
                SumaTotal =SumaTotal + int.Parse(us.idp_cantidad);
            }
            //2.- Encontrar el numero de Tareas
            int Total = db.iso_detalle_plan
                .Where(x => x.idp_id_plan == IdPlan)
                .Where(x => x.idp_numero_plan > 0).Count();
            //3.- Dividir la suma para el numero de tareas

            var Cantidad = 0;
            if (Total > 0) {
                Cantidad = SumaTotal / Total;
            }
            TareaCero.idp_cantidad = Cantidad.ToString();
            //Actualizar Estado
            //1.- Analizar Cantidad Total
            if (Cantidad == 100)
            {
                TareaCero.idp_estado = "Completo";
            }
            if (Cantidad <= 100)
            {
                if (MaxFecha >= @DateTime.Now)
                {
                    TareaCero.idp_estado = "Pendiente";
                }
                else
                {
                    TareaCero.idp_estado = "Atrasado";
                }
                
            }
            db.Entry(TareaCero).State = EntityState.Modified;
            db.SaveChanges();

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
