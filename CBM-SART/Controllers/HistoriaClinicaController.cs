﻿using System;
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
using System.IO;

namespace CBM_SART.Controllers
{
    public class HistoriaClinicaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();

        // GET: /HistoriaClinica/
        //public async Task<ActionResult> Index()
        //{
        //    var iso_historia_clinica = db.iso_historia_clinica.Include(i => i.iso_personal);
        //    return View(await iso_historia_clinica.ToListAsync());
        //}

        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "iso_personal.ipe_nombre_personal", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_historia_clinica>();
            ViewBag.filter = filter;
            records.Content = db.iso_historia_clinica
                        .Where(x => filter == null ||
                                (x.iso_personal.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()) ||
                                x.iso_personal.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_historia_clinica
                         .Where(x => filter == null ||
                                (x.iso_personal.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))
                                ||
                                x.iso_personal.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim())).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
        }


        // GET: /HistoriaClinica/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = await db.iso_historia_clinica.FindAsync(id);
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Create
        public ActionResult Create()
        {
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_nombre_personal");
            return View();
        }

        // POST: /HistoriaClinica/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ihc_id_historia,ihc_id_personal,ihc_id_empresa,ihc_lugar_historia_clinica,ihc_fecha_historia_clinica")] iso_historia_clinica iso_historia_clinica)
        {
            if (ModelState.IsValid)
            {
                db.iso_historia_clinica.Add(iso_historia_clinica);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Edit/5
        public async Task<ActionResult> Edit(int? id, int idConsulta = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = db.iso_historia_clinica.Where(x => x.ihc_id_personal == id).First();
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            //Seleciona la Consulta Medica Preocupacional
            if (idConsulta == 0)
            {
                iso_consulta_medica iso_consulta_medica_pre = iso_historia_clinica.iso_consulta_medica.Where(x => x.icm_tipo_consulta == 1).First();
                if (iso_consulta_medica_pre != null) { idConsulta = iso_consulta_medica_pre.icm_id_consulta; }
                
            }
            ViewBag.idConsulta = idConsulta;
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // POST: /HistoriaClinica/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ihc_id_historia,ihc_id_personal,ihc_id_empresa,ihc_lugar_historia_clinica,ihc_fecha_historia_clinica")] iso_historia_clinica iso_historia_clinica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_historia_clinica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ihc_id_personal = new SelectList(db.iso_personal, "ipe_id_personal", "ipe_ced_ruc_personal", iso_historia_clinica.ihc_id_personal);
            return View(iso_historia_clinica);
        }

        // GET: /HistoriaClinica/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_historia_clinica iso_historia_clinica = db.iso_historia_clinica.Where(x => x.ihc_id_historia == id).First();
            if (iso_historia_clinica == null)
            {
                return HttpNotFound();
            }
            return PartialView(iso_historia_clinica);
        }

        // POST: /HistoriaClinica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_historia_clinica iso_historia_clinica = db.iso_historia_clinica.Where(x => x.ihc_id_historia == id).First();
            db.iso_historia_clinica.Remove(iso_historia_clinica);
            await db.SaveChangesAsync();
            //Elimina Consultas medicas
            var iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_historia_clinica == id).ToList();
            foreach(iso_consulta_medica consulta in iso_consulta_medica ){
                db.iso_consulta_medica.Remove(consulta);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Panel()
        {
            return View();
        }
        public ActionResult ListaPersonal(string filter = null, int page = 1, int pageSize = 18, string sort = "ipe_apellido_paterno", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_personal>();
            ViewBag.filter = filter;
            records.Content = db.iso_personal
                        .Where(x => filter == null ||
                                (x.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))
                                   || x.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim())
                              )
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_personal
                         .Where(x => filter == null ||
                               (x.ipe_nombre_personal.Contains(filter)) || x.ipe_apellido_paterno.Contains(filter)).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;


            //var Personal = db.iso_personal.ToList();
            return PartialView("ListaPersonal", records);
        }

        public ActionResult HistorialCM(int IdPersonal)
        {

            iso_personal iso_personal = db.iso_personal.Find(IdPersonal);
            iso_historia_clinica iso_historia_clinica = iso_personal.iso_historia_clinica.First();
            var iso_consulta_medica = iso_historia_clinica.iso_consulta_medica.ToList();

            //var Personal = db.iso_personal.ToList();
            return PartialView("HistorialCM", iso_consulta_medica);
        }

        public int InsertarHistoria(int IdPersonal)
        {
            //Si existe la historia
            int hc = db.iso_historia_clinica.Where(x => x.ihc_id_personal == IdPersonal).Count();
            if (hc > 0)
            {
                iso_historia_clinica iso_historia_clinica_registrada = db.iso_historia_clinica.Where(x => x.ihc_id_personal == IdPersonal).First();
                if (iso_historia_clinica_registrada != null)
                {
                    return iso_historia_clinica_registrada.ihc_id_historia;
                }
            }
            //Ingresa consulta Historia Clinica
            iso_historia_clinica iso_historia_clinica = new iso_historia_clinica();
            iso_personal iso_personal = db.iso_personal.Find(IdPersonal);

            iso_historia_clinica.ihc_id_personal = iso_personal.ipe_id_personal;
            iso_historia_clinica.ihc_id_empresa = iso_personal.iso_empresa.iem_cod_empresa;
            iso_historia_clinica.ihc_fecha_historia_clinica = DateTime.Now;
            iso_historia_clinica.ihc_lugar_historia_clinica = "CBM-SART";
            db.iso_historia_clinica.Add(iso_historia_clinica);
            db.SaveChanges();
            //Ingresa consulta medica Ocupacional
            iso_consulta_medica iso_consulta_medica = new iso_consulta_medica();
            iso_consulta_medica.icm_id_historia_clinica = iso_historia_clinica.ihc_id_historia;
            iso_consulta_medica.icm_id_personal = iso_personal.ipe_id_personal;
            iso_consulta_medica.icm_tipo_consulta = 1; //Preocupacional
            iso_consulta_medica.icm_fecha_consulta = DateTime.Now;
            db.iso_consulta_medica.Add(iso_consulta_medica);
            db.SaveChanges();

            return iso_historia_clinica.ihc_id_historia;
        }
        ///TABLAS CONSULTA MEDICA 
        ////////////////////////////Antecendete Personal///////////////////////////////////
        public ActionResult CmAntePersonal(int IdConsulta, string filter = null, int page = 1, int pageSize = 5, string sort = "iso_antecedente_personal.iap_nombre_antecedente_p", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_ante_personal_consulta_m>();
            ViewBag.filter = filter;
            records.Content = iso_consulta_medica.iso_ante_personal_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_antecedente_personal.iap_nombre_antecedente_p.ToLower().Contains(filter.ToLower().Trim()))
                              )
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records.TotalRecords = iso_consulta_medica.iso_ante_personal_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_antecedente_personal.iap_nombre_antecedente_p.ToLower().Contains(filter.ToLower().Trim()))).Count();
            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView(records);
        }
        public ActionResult CrearAntePersonal(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_personal_consulta_m.ToList();
            List<iso_antecedente_personal> list = new List<iso_antecedente_personal>();
            foreach (iso_ante_personal_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_personal);
            }
            var total = db.iso_antecedente_personal
                .OrderBy(x => x.iap_nombre_antecedente_p).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iap_id_antecedente_p", "iap_nombre_antecedente_p");
            iso_ante_personal_consulta_m iso_ante_personal_consulta_m = new iso_ante_personal_consulta_m();
            return PartialView("CMParametros/CrearAntePersonal", iso_ante_personal_consulta_m);
        }
        public string GuardarAntePersonal(int idConsulta, iso_ante_personal_consulta_m iso_ante_personal_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_ante_personal_consulta_m.ipc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_ante_personal_consulta_m.ipc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_ante_personal_consulta_m.ipc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_ante_personal_consulta_m.Add(iso_ante_personal_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarAntePersonal(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_personal_consulta_m.Where(x => x.ipc_id_antecedente_p != IdParametro).ToList();
            List<iso_antecedente_personal> list = new List<iso_antecedente_personal>();
            foreach (iso_ante_personal_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_personal);
            }
            var total = db.iso_antecedente_personal
                .OrderBy(x => x.iap_nombre_antecedente_p).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iap_id_antecedente_p", "iap_nombre_antecedente_p");
            iso_ante_personal_consulta_m iso_ante_personal_consulta_m = db.iso_ante_personal_consulta_m
                .Where(x => x.ipc_id_antecedente_p == IdParametro && x.ipc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarAntePersonal", iso_ante_personal_consulta_m);
        }
        public string ActualizarAntePersonal(iso_ante_personal_consulta_m iso_ante_personal_consulta_m)
        {
            db.Entry(iso_ante_personal_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Antecendete Familiar Morbilidad///////////////////////////////////
        public ActionResult CmAnteFamiliarMorb(int IdConsulta, string filter = null, int page = 1, int pageSize = 5, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_ante_familiar_consulta_m>();
            ViewBag.filter = filter;
            records.Content = iso_consulta_medica.iso_ante_familiar_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_antecedente_familiar.iaf_nombre_antecedente_f.ToLower().Contains(filter.ToLower().Trim())))
                        .Where(x => x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORBILIDAD")
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records.TotalRecords = iso_consulta_medica.iso_ante_familiar_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_antecedente_familiar.iaf_nombre_antecedente_f.ToLower().Contains(filter.ToLower().Trim())))
                         .Where(x => x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORBILIDAD")
                         .Count();
            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView(records);
        }
        public ActionResult CrearAnteFamiliarMorb(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_familiar_consulta_m.ToList();
            List<iso_antecedente_familiar> list = new List<iso_antecedente_familiar>();
            foreach (iso_ante_familiar_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_familiar);
            }
            var total = db.iso_antecedente_familiar
                .Where(x => x.iaf_tipo_antecedente_f == "MORBILIDAD")
                .OrderBy(x => x.iaf_nombre_antecedente_f).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iaf_id_antecedente_familiar", "iaf_nombre_antecedente_f");
            iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = new iso_ante_familiar_consulta_m();
            return PartialView("CMParametros/CrearAnteFamiliarMorb", iso_ante_familiar_consulta_m);
        }
        public string GuardarAnteFamiliarMorb(int idConsulta, iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_ante_familiar_consulta_m.ifc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_ante_familiar_consulta_m.ifc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_ante_familiar_consulta_m.ifc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_ante_familiar_consulta_m.Add(iso_ante_familiar_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarAnteFamiliarMorb(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_familiar_consulta_m.Where(x => x.ifc_id_antecedente_familiar != IdParametro).ToList();
            List<iso_antecedente_familiar> list = new List<iso_antecedente_familiar>();
            foreach (iso_ante_familiar_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_familiar);
            }
            var total = db.iso_antecedente_familiar
                .Where(x => x.iaf_tipo_antecedente_f == "MORBILIDAD")
                .OrderBy(x => x.iaf_nombre_antecedente_f).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iaf_id_antecedente_familiar", "iaf_nombre_antecedente_f");
            iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = db.iso_ante_familiar_consulta_m
                .Where(x => x.ifc_id_antecedente_familiar == IdParametro && x.ifc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarAnteFamiliarMorb", iso_ante_familiar_consulta_m);
        }
        public string ActualizarAnteFamiliarMorb(iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m)
        {
            db.Entry(iso_ante_familiar_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Fin Antecendete Familiar Morbilidad///////////////////////////////////
        ////////////////////////////Antecendete Familiar Mortalidad///////////////////////////////////
        public ActionResult CmAnteFamiliarMort(int IdConsulta, string filter = null, int page = 1, int pageSize = 5, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_ante_familiar_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_ante_familiar_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_antecedente_familiar.iaf_nombre_antecedente_f.ToLower().Contains(filter.ToLower().Trim())))
                        .Where(x => x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORTALIDAD")
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_ante_familiar_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_antecedente_familiar.iaf_nombre_antecedente_f.ToLower().Contains(filter.ToLower().Trim())))
                         .Where(x => x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORTALIDAD")
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmAnteFamiliarMort", records_mort);
        }
        public ActionResult CrearAnteFamiliarMort(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_familiar_consulta_m.ToList();
            List<iso_antecedente_familiar> list = new List<iso_antecedente_familiar>();
            foreach (iso_ante_familiar_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_familiar);
            }
            var total = db.iso_antecedente_familiar
                .Where(x => x.iaf_tipo_antecedente_f == "MORTALIDAD")
                .OrderBy(x => x.iaf_nombre_antecedente_f).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iaf_id_antecedente_familiar", "iaf_nombre_antecedente_f");
            iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = new iso_ante_familiar_consulta_m();
            return PartialView("CMParametros/CrearAnteFamiliarMort", iso_ante_familiar_consulta_m);
        }
        public string GuardarAnteFamiliarMort(int idConsulta, iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_ante_familiar_consulta_m.ifc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_ante_familiar_consulta_m.ifc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_ante_familiar_consulta_m.ifc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_ante_familiar_consulta_m.Add(iso_ante_familiar_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarAnteFamiliarMort(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_familiar_consulta_m.Where(x => x.ifc_id_antecedente_familiar != IdParametro).ToList();
            List<iso_antecedente_familiar> list = new List<iso_antecedente_familiar>();
            foreach (iso_ante_familiar_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_familiar);
            }
            var total = db.iso_antecedente_familiar
                .Where(x => x.iaf_tipo_antecedente_f == "MORTALIDAD")
                .OrderBy(x => x.iaf_nombre_antecedente_f).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iaf_id_antecedente_familiar", "iaf_nombre_antecedente_f");
            iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = db.iso_ante_familiar_consulta_m
                .Where(x => x.ifc_id_antecedente_familiar == IdParametro && x.ifc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarAnteFamiliarMort", iso_ante_familiar_consulta_m);
        }
        public string ActualizarAnteFamiliarMort(iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m)
        {
            db.Entry(iso_ante_familiar_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Fin Antecendete Familiar Mortalidad///////////////////////////////////
        ///Eliminar
        public string EliminarAntecente(int IdConsulta, int IdAntecente, string TipoAntecente)
        {
            string mensaje = "No se pudo eliminar";
            if (TipoAntecente == "AntePersonal")
            {
                iso_ante_personal_consulta_m iso_ante_personal_consulta_m = db.iso_ante_personal_consulta_m.Where(
                    x => x.ipc_id_consulta_medica == IdConsulta && x.ipc_id_antecedente_p == IdAntecente).First();
                db.iso_ante_personal_consulta_m.Remove(iso_ante_personal_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "AnteFamiliarMorb")
            {
                iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = db.iso_ante_familiar_consulta_m.Where(
                    x => x.ifc_id_consulta_medica == IdConsulta && x.ifc_id_antecedente_familiar == IdAntecente
                    && x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORBILIDAD").First();
                db.iso_ante_familiar_consulta_m.Remove(iso_ante_familiar_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "AnteFamiliarMort")
            {
                iso_ante_familiar_consulta_m iso_ante_familiar_consulta_m = db.iso_ante_familiar_consulta_m.Where(
                    x => x.ifc_id_consulta_medica == IdConsulta && x.ifc_id_antecedente_familiar == IdAntecente
                    && x.iso_antecedente_familiar.iaf_tipo_antecedente_f == "MORTALIDAD").First();
                db.iso_ante_familiar_consulta_m.Remove(iso_ante_familiar_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            return mensaje;
        }


        public FileContentResult GetImage(int ID)
        {
            iso_personal cat = db.iso_personal.FirstOrDefault(c => c.ipe_id_personal == ID);
            if (cat != null)
            {
                string type = "image/jpeg";
                return File(cat.ipe_foto, type);
            }
            else
            {
                return null;
            }
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
