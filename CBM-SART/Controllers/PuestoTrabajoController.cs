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
    public class PuestoTrabajoController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /PuestoTrabajo/
        //public async Task<ActionResult> Index()
        //{
        //    //return View(await db.iso_puesto_trabajo.ToListAsync());
        //    var iso_puesto_trabajo = await db.iso_puesto_trabajo.ToListAsync();
        //    return  PartialView("Index", iso_puesto_trabajo);
        //}

        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "ipt_nombre_puesto_t", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_puesto_trabajo>();
            ViewBag.filter = filter;
            records.Content = db.iso_puesto_trabajo
                        .Where(x => filter == null ||
                                (x.ipt_nombre_puesto_t.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_puesto_trabajo
                         .Where(x => filter == null ||
                                (x.ipt_nombre_puesto_t.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            return View(records);
            //return View(db.iso_empresa.ToList());
        }
        // GET: /PuestoTrabajo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_puesto_trabajo iso_puesto_trabajo = await db.iso_puesto_trabajo.FindAsync(id);
            if (iso_puesto_trabajo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", iso_puesto_trabajo);
        }

        // GET: /PuestoTrabajo/Create
        public ActionResult Create()
        {
            iso_puesto_trabajo iso_puesto_trabajo = new iso_puesto_trabajo();
            return PartialView("Create", iso_puesto_trabajo);
        }

        // POST: /PuestoTrabajo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ipt_id_puesto_t,ipt_nombre_puesto_t,ipt_descrip_puesto_t,ipt_nro_trbajadores")] iso_puesto_trabajo iso_puesto_trabajo, string path = "")
        {
            if (ModelState.IsValid)
            {
                db.iso_puesto_trabajo.Add(iso_puesto_trabajo);
                await db.SaveChangesAsync();
                if (path == "")
                {
                    return View("Index");
                }
                else
                {
                    return Redirect(path);
                }
            }

            return PartialView("Create", iso_puesto_trabajo);
        }

        // GET: /PuestoTrabajo/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_puesto_trabajo iso_puesto_trabajo = await db.iso_puesto_trabajo.FindAsync(id);
            if (iso_puesto_trabajo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", iso_puesto_trabajo);
        }

        // POST: /PuestoTrabajo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ipt_id_puesto_t,ipt_nombre_puesto_t,ipt_descrip_puesto_t,ipt_nro_trbajadores")] iso_puesto_trabajo iso_puesto_trabajo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_puesto_trabajo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("Edit", iso_puesto_trabajo);
        }

        // GET: /PuestoTrabajo/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_puesto_trabajo iso_puesto_trabajo = await db.iso_puesto_trabajo.FindAsync(id);
            if (iso_puesto_trabajo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", iso_puesto_trabajo);
        }

        // POST: /PuestoTrabajo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_puesto_trabajo iso_puesto_trabajo = await db.iso_puesto_trabajo.FindAsync(id);
            db.iso_puesto_trabajo.Remove(iso_puesto_trabajo);
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
