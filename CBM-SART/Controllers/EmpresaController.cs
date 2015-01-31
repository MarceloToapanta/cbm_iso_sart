using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CBM_SART.Models;
using System.Linq.Dynamic;

namespace CBM_SART.Controllers
{
    public class EmpresaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Empresa/
        [Authorize]
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "iem_cod_empresa", string sortdir = "ASC")
        {
            var records = new PagedList<iso_empresa>();
            ViewBag.filter = filter;
            records.Content = db.iso_empresa
                        .Where(x => filter == null ||
                                (x.iem_nombre_empresa.Contains(filter))
                                   || x.iem_nemonico_empresa.Contains(filter)
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_empresa
                         .Where(x => filter == null ||
                               (x.iem_nombre_empresa.Contains(filter)) || x.iem_nemonico_empresa.Contains(filter)).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);
            
            //return View(db.iso_empresa.ToList());
        }

        // GET: /Empresa/Details/5
        public ActionResult Details(int? id = 0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            //return View(iso_empresa);
            return PartialView("Details", iso_empresa);
        }

        // GET: /Empresa/Create
        public ActionResult Create()
        {
            var iso_empresa = new iso_empresa();
            return PartialView("Create", iso_empresa);
            //return View();
        }

        // POST: /Phone/Create
        [HttpPost]
        public JsonResult Create(iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.iso_empresa.Add(iso_empresa);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(iso_empresa, JsonRequestBehavior.AllowGet);
        }

        // POST: /Empresa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.iso_empresa.Add(iso_empresa);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(iso_empresa);
        //}

        // GET: /Empresa/Edit/5
        public ActionResult Edit(int? id = 0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            //return View(iso_empresa);
            return PartialView("Edit", iso_empresa);
        }

        // POST: /Empresa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(iso_empresa).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(iso_empresa);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_empresa).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("Edit", iso_empresa);
        }

        // GET: /Empresa/Delete/5
        public ActionResult Delete(int? id = 0)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            //return View(iso_empresa);
            return PartialView("Delete", iso_empresa);
        }

        // POST: /Empresa/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    iso_empresa iso_empresa = db.iso_empresa.Find(id);
        //    db.iso_empresa.Remove(iso_empresa);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var phone = db.iso_empresa.Find(id);
            db.iso_empresa.Remove(phone);
            db.SaveChanges();
            return Json(new { success = true });
        }

        /// <summary>
        public FileContentResult GetImage(int ID)
        {
            iso_empresa cat = db.iso_empresa.FirstOrDefault(c => c.iem_cod_empresa == ID);
            if (cat != null)
            {

                string type = string.Empty;
                if (!string.IsNullOrEmpty(cat.iem_icono_empresa))
                {
                    type = cat.iem_icono_empresa;
                }
                else
                {
                    type = "image/jpeg";
                }

                return File(cat.iem_icono_archivo, type);
            }
            else
            {
                return null;
            }
        }
        /// 
        
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
