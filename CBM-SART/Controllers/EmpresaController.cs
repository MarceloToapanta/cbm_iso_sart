using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CBM_SART.Models;

namespace CBM_SART.Controllers
{
    public class EmpresaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /Empresa/
        public ActionResult Index()
        {
            return View(db.iso_empresa.ToList());
        }

        // GET: /Empresa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            return View(iso_empresa);
        }

        // GET: /Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Empresa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.iso_empresa.Add(iso_empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iso_empresa);
        }

        // GET: /Empresa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            return View(iso_empresa);
        }

        // POST: /Empresa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="iem_cod_empresa,iem_nombre_empresa,iem_nemonico_empresa,iem_ruc_empresa,iem_direccion_empresa,iem_telefono_empresa,iem_rep_legal_empresa,iem_personeria_empresa,iem_icono_empresa,iem_vision_empresa,iem_mision_empresa,iem_politica_empresa,iem_objetivo_empresa,iem_valores_empresa,iem_icono_archivo,iem_politica_general,iem_politica_calidad,iem_estrategia_general,iem_razon_social,iem_numero_patronal,iem_actividad,iem_numero_trab_administrativos,iem_numero_trab_planta")] iso_empresa iso_empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iso_empresa);
        }

        // GET: /Empresa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            if (iso_empresa == null)
            {
                return HttpNotFound();
            }
            return View(iso_empresa);
        }

        // POST: /Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            iso_empresa iso_empresa = db.iso_empresa.Find(id);
            db.iso_empresa.Remove(iso_empresa);
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
