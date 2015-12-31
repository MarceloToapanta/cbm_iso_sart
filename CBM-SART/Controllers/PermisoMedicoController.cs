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
    public class PermisoMedicoController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /PermisoMedico/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 20, string sort = "ipm_fecha_permiso_ini", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_permiso_medico>();
            ViewBag.filter = filter;
            records.Content = db.iso_permiso_medico
                        .Where(x => filter == null ||
                                (x.iso_consulta_medica.iso_historia_clinica.iso_personal.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_permiso_medico
                         .Where(x => filter == null ||
                                (x.iso_consulta_medica.iso_historia_clinica.iso_personal.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);
            //return View(db.iso_empresa.ToList());
        }
        //public async Task<ActionResult> Index()
        //{
        //    var iso_permiso_medico = db.iso_permiso_medico.Include(i => i.iso_consulta_medica);
        //    return View(await iso_permiso_medico.ToListAsync());
        //}

        // GET: /PermisoMedico/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_permiso_medico iso_permiso_medico = await db.iso_permiso_medico.FindAsync(id);
            if (iso_permiso_medico == null)
            {
                return HttpNotFound();
            }
            return View(iso_permiso_medico);
        }

        // GET: /PermisoMedico/Create
        public ActionResult Create()
        {
            ViewBag.ipm_id_consulta = new SelectList(db.iso_consulta_medica, "icm_id_consulta", "icm_motivo_consulta");
            var personal =
              db.iso_personal
                .OrderBy("ipe_apellido_paterno")
                .ToList()
                .Select(s => new
                {
                    id = s.ipe_id_personal,
                    nombre = string.Format("{0} {1}", s.ipe_nombre_personal, s.ipe_apellido_paterno)
                });
            ViewBag.ipm_id_personal = new SelectList(personal, "id", "nombre");
            return View();
        }

        // POST: /PermisoMedico/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ipm_id_permiso_med,ipm_id_historia,ipm_id_consulta,ipm_id_personal,ipm_id_tipo_permisom,ipm_fecha_permiso_ini,ipm_fecha_permiso_fin,ipm_dias_permiso")] iso_permiso_medico iso_permiso_medico)
        {
            if (ModelState.IsValid)
            {
                db.iso_permiso_medico.Add(iso_permiso_medico);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ipm_id_consulta = new SelectList(db.iso_consulta_medica, "icm_id_consulta", "icm_motivo_consulta", iso_permiso_medico.ipm_id_consulta);
            //return View(iso_permiso_medico);
            return PartialView("Create", iso_permiso_medico);
        }

        // GET: /PermisoMedico/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_permiso_medico iso_permiso_medico = await db.iso_permiso_medico.FindAsync(id);
            if (iso_permiso_medico == null)
            {
                return HttpNotFound();
            }
            ViewBag.ipm_id_consulta = new SelectList(db.iso_consulta_medica, "icm_id_consulta", "icm_motivo_consulta", iso_permiso_medico.ipm_id_consulta);
            return View(iso_permiso_medico);
        }

        // POST: /PermisoMedico/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ipm_id_permiso_med,ipm_id_historia,ipm_id_consulta,ipm_id_personal,ipm_id_tipo_permisom,ipm_fecha_permiso_ini,ipm_fecha_permiso_fin,ipm_dias_permiso")] iso_permiso_medico iso_permiso_medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_permiso_medico).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ipm_id_consulta = new SelectList(db.iso_consulta_medica, "icm_id_consulta", "icm_motivo_consulta", iso_permiso_medico.ipm_id_consulta);
            return View(iso_permiso_medico);
        }

        // GET: /PermisoMedico/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_permiso_medico iso_permiso_medico = await db.iso_permiso_medico.FindAsync(id);
            if (iso_permiso_medico == null)
            {
                return HttpNotFound();
            }
            return View(iso_permiso_medico);
        }

        // POST: /PermisoMedico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_permiso_medico iso_permiso_medico = await db.iso_permiso_medico.FindAsync(id);
            db.iso_permiso_medico.Remove(iso_permiso_medico);
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
