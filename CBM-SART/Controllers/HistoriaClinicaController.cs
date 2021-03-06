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
using System.Globalization;
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class HistoriaClinicaController : BaseController
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
                int consulta_medica_pre = iso_historia_clinica.iso_consulta_medica.Where(x => x.icm_tipo_consulta == 1).Count();
                if (consulta_medica_pre == 0)
                {
                    iso_consulta_medica new_consulta_medica_pre = new iso_consulta_medica();
                    new_consulta_medica_pre.icm_id_historia_clinica = iso_historia_clinica.ihc_id_historia;
                    new_consulta_medica_pre.icm_id_personal = iso_historia_clinica.ihc_id_personal;
                    new_consulta_medica_pre.icm_tipo_consulta = 1; //Preocupacional
                    new_consulta_medica_pre.icm_fecha_consulta = DateTime.Now;
                    db.iso_consulta_medica.Add(new_consulta_medica_pre);
                    db.SaveChanges();
                }
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

        // GET: /HistoriaClinica/EditConsulta/5
        public async Task<ActionResult> EditConsulta(int? id, int idConsulta = 0)
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
            foreach (iso_consulta_medica consulta in iso_consulta_medica)
            {
                db.iso_consulta_medica.Remove(consulta);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Panel()
        {
            if (TipoUsuario() == "Medico" || TipoUsuario() == "Administrador"){
                ViewBag.TipoUsuario = "Medico";
            }
            return View();
        }
        public ActionResult IngresarCM(int id)
        {
            iso_historia_clinica iso_historia_clinica = db.iso_historia_clinica.Where(x => x.ihc_id_historia == id).First();
            ViewBag.IdHC = id;
            iso_consulta_medica iso_consulta_medica = new iso_consulta_medica();
            iso_consulta_medica.icm_id_historia_clinica = iso_historia_clinica.ihc_id_historia;
            iso_consulta_medica.icm_id_personal = iso_historia_clinica.ihc_id_personal;
            //Id 1 pertenece a consulta medica PREOCUPACIONAL
            ViewBag.TipoConsutla = db.iso_tipo_consulta_m.Where(x => x.itc_id_tipo_consulta_m != 1).ToArray();
            return View(iso_consulta_medica);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GuardarCM(iso_consulta_medica iso_consulta_medica)
        {
            if (ModelState.Count() > 1)
            {
                int cm_count = db.iso_consulta_medica.Where(x => x.icm_fecha_consulta == iso_consulta_medica.icm_fecha_consulta
                    && x.icm_tipo_consulta == iso_consulta_medica.icm_tipo_consulta).Count();
                if (cm_count == 0)
                {
                    db.iso_consulta_medica.Add(iso_consulta_medica);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "HistoriaClinica", new { id = iso_consulta_medica.icm_id_personal, idConsulta = iso_consulta_medica.icm_id_consulta });
                }
                else
                {
                    iso_consulta_medica cm = db.iso_consulta_medica.Where(x => x.icm_fecha_consulta == iso_consulta_medica.icm_fecha_consulta
                    && x.icm_tipo_consulta == iso_consulta_medica.icm_tipo_consulta).First();
                    return RedirectToAction("Edit", "HistoriaClinica", new { id = cm.icm_id_personal, idConsulta = cm.icm_id_consulta });
                }
            }
            return View("IngresarCM/" + iso_consulta_medica.icm_id_historia_clinica);
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
                               (x.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim())) || x.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim())).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;


            //var Personal = db.iso_personal.ToList();
            return PartialView("ListaPersonal", records);
        }

        public ActionResult ListaMedicos(string filter = null, int page = 1, int pageSize = 18, string sort = "ius_nombre_usuario", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_usuario>();
            ViewBag.filter = filter;
            records.Content = db.iso_usuario
                        .Where(x => filter == null ||
                                (x.ius_nombre_usuario.ToLower().Contains(filter.ToLower().Trim())))
                        .Where(x => x.ius_tipo_usuario == "Medico")
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_usuario
                         .Where(x => filter == null ||
                               (x.ius_nombre_usuario.ToLower().Contains(filter.ToLower().Trim()))).Where(x => x.ius_tipo_usuario == "Medico").Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;


            //var Personal = db.iso_personal.ToList();
            return PartialView("ListaMedicos", records);
        }

        public ActionResult HistorialCM(int IdPersonal, string filter = null, int page = 1, int pageSize = 10, string sort = "icm_fecha_consulta", string sortdir = "ASC")
        {
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_consulta_medica>();
            ViewBag.idPersonal = IdPersonal;
            ViewBag.filter = filter;
            records.Content = db.iso_consulta_medica
                        .Where(x => filter == null ||
                                (x.iso_tipo_consulta_m.itc_nom_tipo_consulta_m.ToLower().Contains(filter.ToLower().Trim())
                                )
                              )
                        .Where(x => x.icm_id_personal == IdPersonal)
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_consulta_medica
                         .Where(x => filter == null ||
                               (x.iso_tipo_consulta_m.itc_nom_tipo_consulta_m.ToLower().Contains(filter.ToLower().Trim())))
                          .Where(x => x.icm_id_personal == IdPersonal).Count();
            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;
            return PartialView("HistorialCM", records);
        }
        public ActionResult EliminarCM(int IdConsulta, int IdPersonal, int IdHistoria)
        {
            int cm = db.iso_consulta_medica
                .Where(x => x.icm_id_consulta == IdConsulta)
                .Where(x => x.icm_id_personal == IdPersonal)
                .Where(x => x.icm_id_historia_clinica == IdHistoria)
                .Count();
            if (cm > 0)
            {
                iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica
                    .Where(x => x.icm_id_consulta == IdConsulta)
                    .Where(x => x.icm_id_personal == IdPersonal)
                    .Where(x => x.icm_id_historia_clinica == IdHistoria)
                    .First();
                db.iso_consulta_medica.Remove(iso_consulta_medica);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "HistoriaClinica", new { id = IdPersonal });
        }
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;
            return this.Json(new { success = true });
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
        public ActionResult CmAntePersonal(int IdConsulta, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_personal.iap_nombre_antecedente_p", string sortdir = "ASC")
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
        public ActionResult CmAnteFamiliarMorb(int IdConsulta, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
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
        public ActionResult CmAnteFamiliarMort(int IdConsulta, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
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
        ////////////////////////////Antecendete Vacunas///////////////////////////////////
        public ActionResult CmAnteVacuna(int IdConsulta, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_vacuna_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_vacuna_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_antecedente_vacuna.iav_nombre_vacuna.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_vacuna_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_antecedente_vacuna.iav_nombre_vacuna.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmAnteVacuna", records_mort);
        }
        public ActionResult CrearAnteVacuna(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_vacuna_consulta_m.ToList();
            List<iso_antecedente_vacuna> list = new List<iso_antecedente_vacuna>();
            foreach (iso_vacuna_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_vacuna);
            }
            var total = db.iso_antecedente_vacuna
                .OrderBy(x => x.iav_nombre_vacuna).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iav_id_vacuna", "iav_nombre_vacuna");
            iso_vacuna_consulta_m iso_vacuna_consulta_m = new iso_vacuna_consulta_m();
            return PartialView("CMParametros/CrearAnteVacuna", iso_vacuna_consulta_m);
        }
        public string GuardarAnteVacuna(int idConsulta, iso_vacuna_consulta_m iso_vacuna_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_vacuna_consulta_m.ivm_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_vacuna_consulta_m.ivm_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_vacuna_consulta_m.ivm_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_vacuna_consulta_m.Add(iso_vacuna_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarAnteVacuna(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_vacuna_consulta_m.Where(x => x.ivm_id_vacuna != IdParametro).ToList();
            List<iso_antecedente_vacuna> list = new List<iso_antecedente_vacuna>();
            foreach (iso_vacuna_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_vacuna);
            }
            var total = db.iso_antecedente_vacuna
                .OrderBy(x => x.iav_nombre_vacuna).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iav_id_vacuna", "iav_nombre_vacuna");
            iso_vacuna_consulta_m iso_vacuna_consulta_m = db.iso_vacuna_consulta_m
                .Where(x => x.ivm_id_vacuna == IdParametro && x.ivm_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarAnteVacuna", iso_vacuna_consulta_m);
        }
        public string ActualizarAnteVacuna(iso_vacuna_consulta_m iso_vacuna_consulta_m)
        {
            db.Entry(iso_vacuna_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Fin Antecendete Vacunas///////////////////////////////////
        ////////////////////////////Antecendete Mujer///////////////////////////////////
        public ActionResult CmAnteMujer(int IdConsulta, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_ante_mujer_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_ante_mujer_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_antecedente_mujer.iam_nombre_antecedente_m.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_ante_mujer_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_antecedente_mujer.iam_nombre_antecedente_m.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmAnteMujer", records_mort);
        }
        public ActionResult CrearAnteMujer(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_mujer_consulta_m.ToList();
            List<iso_antecedente_mujer> list = new List<iso_antecedente_mujer>();
            foreach (iso_ante_mujer_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_mujer);
            }
            var total = db.iso_antecedente_mujer
                .OrderBy(x => x.iam_nombre_antecedente_m).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iam_id_antecedente_mujer", "iam_nombre_antecedente_m");
            iso_ante_mujer_consulta_m iso_ante_mujer_consulta_m = new iso_ante_mujer_consulta_m();
            return PartialView("CMParametros/CrearAnteMujer", iso_ante_mujer_consulta_m);
        }
        public string GuardarAnteMujer(int idConsulta, iso_ante_mujer_consulta_m iso_ante_mujer_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_ante_mujer_consulta_m.imc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_ante_mujer_consulta_m.imc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_ante_mujer_consulta_m.imc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_ante_mujer_consulta_m.Add(iso_ante_mujer_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarAnteMujer(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_ante_mujer_consulta_m.Where(x => x.imc_id_antecedente_mujer != IdParametro).ToList();
            List<iso_antecedente_mujer> list = new List<iso_antecedente_mujer>();
            foreach (iso_ante_mujer_consulta_m var in asignadosm)
            {
                list.Add(var.iso_antecedente_mujer);
            }
            var total = db.iso_antecedente_mujer
                .OrderBy(x => x.iam_nombre_antecedente_m).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iam_id_antecedente_mujer", "iam_nombre_antecedente_m");
            iso_ante_mujer_consulta_m iso_ante_mujer_consulta_m = db.iso_ante_mujer_consulta_m
                .Where(x => x.imc_id_antecedente_mujer == IdParametro && x.imc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarAnteMujer", iso_ante_mujer_consulta_m);
        }
        public string ActualizarAnteMujer(iso_ante_mujer_consulta_m iso_ante_mujer_consulta_m)
        {
            db.Entry(iso_ante_mujer_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Fin Antecendete Mujer///////////////////////////////////
        ////////////////*****************Signos y Sintomas*******************/////////////////////////////
        public ActionResult CmSigno(int IdConsulta, string filter = null, int page = 1, int pageSize = 10, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_sintoma_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_sintoma_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_sintoma.ist_nombre_sintoma.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_sintoma_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_sintoma.ist_nombre_sintoma.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmSigno", records_mort);
        }
        public ActionResult CrearSigno(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_sintoma_consulta_m.ToList();
            List<iso_sintoma> list = new List<iso_sintoma>();
            foreach (iso_sintoma_consulta_m var in asignadosm)
            {
                list.Add(var.iso_sintoma);
            }
            var total = db.iso_sintoma
                .OrderBy(x => x.ist_nombre_sintoma).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ist_id_sintoma", "ist_nombre_sintoma");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_sintoma.ToList(), "its_id_tipo_sintoma", "its_nombre_tipo_sintoma");
            iso_sintoma_consulta_m iso_sintoma_consulta_m = new iso_sintoma_consulta_m();
            return PartialView("CMParametros/CrearSigno", iso_sintoma_consulta_m);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ObetenerSignoPorTipo(int idConsulta, int idTipo)
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_sintoma_consulta_m.ToList();
            List<iso_sintoma> list = new List<iso_sintoma>();
            foreach (iso_sintoma_consulta_m var in asignadosm)
            {
                list.Add(var.iso_sintoma);
            }
            var total = db.iso_sintoma
                .Where(x => x.ist_id_tipo_sintoma == idTipo)
                .OrderBy(x => x.ist_nombre_sintoma).ToList();
            var totalresult = total.Except(list);
            SelectList SelectListParametro = new SelectList(totalresult, "ist_id_sintoma", "ist_nombre_sintoma");
            return Json(SelectListParametro, JsonRequestBehavior.AllowGet);
        }
        public string GuardarSigno(int idConsulta, iso_sintoma_consulta_m iso_sintoma_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_sintoma_consulta_m.isc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_sintoma_consulta_m.isc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_sintoma_consulta_m.isc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_sintoma_consulta_m.Add(iso_sintoma_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarSigno(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_sintoma_consulta_m.Where(x => x.isc_id_sintoma != IdParametro).ToList();
            List<iso_sintoma> list = new List<iso_sintoma>();
            foreach (iso_sintoma_consulta_m var in asignadosm)
            {
                list.Add(var.iso_sintoma);
            }
            var total = db.iso_sintoma
                .OrderBy(x => x.ist_nombre_sintoma).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ist_id_sintoma", "ist_nombre_sintoma");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_sintoma.ToList(), "its_id_tipo_sintoma", "its_nombre_tipo_sintoma");
            iso_sintoma_consulta_m iso_sintoma_consulta_m = db.iso_sintoma_consulta_m
                .Where(x => x.isc_id_sintoma == IdParametro && x.isc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarSigno", iso_sintoma_consulta_m);
        }
        public string ActualizarSigno(iso_sintoma_consulta_m iso_sintoma_consulta_m)
        {
            db.Entry(iso_sintoma_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Signos y Sintomas///////////////////////////////////
        ////////////////*****************Sindromas"Medidas Antropometricas"*******************/////////////////////////////
        public ActionResult CmSindrome(int IdConsulta, string filter = null, int page = 1, int pageSize = 10, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_sindrome_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_sindrome_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_sindrome.iso_nombre_sindrome.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_sindrome_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_sindrome.iso_nombre_sindrome.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmSindrome", records_mort);
        }
        public ActionResult CrearSindrome(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_sindrome_consulta_m.ToList();
            List<iso_sindrome> list = new List<iso_sindrome>();
            foreach (iso_sindrome_consulta_m var in asignadosm)
            {
                list.Add(var.iso_sindrome);
            }
            var total = db.iso_sindrome
                .OrderBy(x => x.iso_nombre_sindrome).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iso_id_sindrome", "iso_nombre_sindrome");
            iso_sindrome_consulta_m iso_sindrome_consulta_m = new iso_sindrome_consulta_m();
            return PartialView("CMParametros/CrearSindrome", iso_sindrome_consulta_m);
        }
        public string GuardarSindrome(int idConsulta, iso_sindrome_consulta_m iso_sindrome_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_sindrome_consulta_m.isd_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_sindrome_consulta_m.isd_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_sindrome_consulta_m.isd_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_sindrome_consulta_m.Add(iso_sindrome_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarSindrome(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_sindrome_consulta_m.Where(x => x.isd_id_sindrome != IdParametro).ToList();
            List<iso_sindrome> list = new List<iso_sindrome>();
            foreach (iso_sindrome_consulta_m var in asignadosm)
            {
                list.Add(var.iso_sindrome);
            }
            var total = db.iso_sindrome
                .OrderBy(x => x.iso_nombre_sindrome).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iso_id_sindrome", "iso_nombre_sindrome");
            iso_sindrome_consulta_m iso_sindrome_consulta_m = db.iso_sindrome_consulta_m
                .Where(x => x.isd_id_sindrome == IdParametro && x.isd_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarSindrome", iso_sindrome_consulta_m);
        }
        public string ActualizarSindrome(iso_sindrome_consulta_m iso_sindrome_consulta_m)
        {
            db.Entry(iso_sindrome_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Sindromas"Medidas Antropometricas"///////////////////////////////////
        ////////////////*****************Habitos"*******************/////////////////////////////
        public ActionResult CmHabito(int IdConsulta, string filter = null, int page = 1, int pageSize = 10, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_habito_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_habito_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_habitos.iht_nombre_habito.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_habito_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_habitos.iht_nombre_habito.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmHabito", records_mort);
        }
        public ActionResult CrearHabito(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_habito_consulta_m.ToList();
            List<iso_habitos> list = new List<iso_habitos>();
            foreach (iso_habito_consulta_m var in asignadosm)
            {
                list.Add(var.iso_habitos);
            }
            var total = db.iso_habitos
                .OrderBy(x => x.iht_nombre_habito).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iht_id_habito", "iht_nombre_habito");
            iso_habito_consulta_m iso_habito_consulta_m = new iso_habito_consulta_m();
            return PartialView("CMParametros/CrearHabito", iso_habito_consulta_m);
        }
        public string GuardarHabito(int idConsulta, iso_habito_consulta_m iso_habito_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_habito_consulta_m.ihc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_habito_consulta_m.ihc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_habito_consulta_m.ihc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_habito_consulta_m.Add(iso_habito_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarHabito(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_habito_consulta_m.Where(x => x.ihc_id_habito != IdParametro).ToList();
            List<iso_habitos> list = new List<iso_habitos>();
            foreach (iso_habito_consulta_m var in asignadosm)
            {
                list.Add(var.iso_habitos);
            }
            var total = db.iso_habitos
                .OrderBy(x => x.iht_nombre_habito).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "iht_id_habito", "iht_nombre_habito");
            iso_habito_consulta_m iso_habito_consulta_m = db.iso_habito_consulta_m
                .Where(x => x.ihc_id_habito == IdParametro && x.ihc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarHabito", iso_habito_consulta_m);
        }
        public string ActualizarHabito(iso_habito_consulta_m iso_habito_consulta_m)
        {
            db.Entry(iso_habito_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Habitos"///////////////////////////////////
        ///////////////////////////Examen Fisico/////////////////////////////
        public ActionResult CmEFParametro(int IdConsulta, int IdTipoParametro, string filter = null, int page = 1, int pageSize = 3, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_exam_fp_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_exam_fp_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_exam_fisico_parametro.ief_nombre_examen.ToLower().Contains(filter.ToLower().Trim())))
                        .Where(x => x.iec_id_tipo_exam_fisico == IdTipoParametro)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_exam_fp_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_exam_fisico_parametro.ief_nombre_examen.ToLower().Contains(filter.ToLower().Trim())))
                        .Where(x => x.iec_id_tipo_exam_fisico == IdTipoParametro)
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            ViewBag.IdTipoParametro = IdTipoParametro;
            return PartialView("CmEFParametro", records_mort);
        }
        public ActionResult CrearEFParametro(int idConsulta, int IdTipoParametro)
        {
            ViewBag.idConsulta = idConsulta;
            ViewBag.IdTipoParametro = IdTipoParametro;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_exam_fp_consulta_m.ToList();
            List<iso_exam_fisico_parametro> list = new List<iso_exam_fisico_parametro>();
            foreach (iso_exam_fp_consulta_m var in asignadosm)
            {
                list.Add(var.iso_exam_fisico_parametro);
            }
            var total = db.iso_exam_fisico_parametro.Where(x => x.ief_tipo_examen == IdTipoParametro)
                .OrderBy(x => x.ief_nombre_examen).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ief_id_examen_fis", "ief_nombre_examen");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_exam_fisico, "ite_id_tipo_ef", "ite_nombre_tipo_ef");
            iso_exam_fp_consulta_m iso_exam_fp_consulta_m = new iso_exam_fp_consulta_m();
            iso_exam_fp_consulta_m.iec_id_tipo_exam_fisico = IdTipoParametro;
            return PartialView("CMParametros/CrearEFParametro", iso_exam_fp_consulta_m);
        }
        public string GuardarEFParametro(int idConsulta, int IdTipoParametro, iso_exam_fp_consulta_m iso_exam_fp_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            iso_exam_fp_consulta_m.iec_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_exam_fp_consulta_m.iec_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_exam_fp_consulta_m.iec_id_personal = iso_consulta_medica.icm_id_personal;
            iso_exam_fp_consulta_m.iec_id_tipo_exam_fisico = IdTipoParametro;
            db.iso_exam_fp_consulta_m.Add(iso_exam_fp_consulta_m);
            return db.SaveChanges().ToString();
        }
        public ActionResult ModificarEFParametro(int idConsulta, int IdParametro, int idTipoParametro)
        {
            ViewBag.idConsulta = idConsulta;
            ViewBag.idTipoParametro = idTipoParametro;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_exam_fp_consulta_m.Where(x => x.iec_id_exam_fisico != IdParametro).ToList();
            List<iso_exam_fisico_parametro> list = new List<iso_exam_fisico_parametro>();
            foreach (iso_exam_fp_consulta_m var in asignadosm)
            {
                list.Add(var.iso_exam_fisico_parametro);
            }
            var total = db.iso_exam_fisico_parametro.Where(x => x.ief_tipo_examen == idTipoParametro)
                .OrderBy(x => x.ief_nombre_examen).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ief_id_examen_fis", "ief_nombre_examen");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_exam_fisico, "ite_id_tipo_ef", "ite_nombre_tipo_ef");
            iso_exam_fp_consulta_m iso_exam_fp_consulta_m = db.iso_exam_fp_consulta_m
                .Where(x => x.iec_id_exam_fisico == IdParametro && x.iec_id_consulta_medica == idConsulta && x.iec_id_tipo_exam_fisico == idTipoParametro).First();
            return PartialView("CMParametros/ModificarEFParametro", iso_exam_fp_consulta_m);
        }
        public string ActualizarEFParametro(iso_exam_fp_consulta_m iso_exam_fp_consulta_m)
        {
            db.Entry(iso_exam_fp_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        public string EliminarEFParametro(int IdConsulta, int IdAntecente, int IdTipoParametro)
        {
            string mensaje = "No se pudo eliminar";
            iso_exam_fp_consulta_m iso_exam_fp_consulta_m = db.iso_exam_fp_consulta_m.Where(
                x => x.iec_id_consulta_medica == IdConsulta && x.iec_id_exam_fisico == IdAntecente
                && x.iec_id_tipo_exam_fisico == IdTipoParametro).First();
            db.iso_exam_fp_consulta_m.Remove(iso_exam_fp_consulta_m);
            if (db.SaveChanges() == 1)
            {
                mensaje = "Registro eliminado";
            }
            return mensaje;
        }
        ////////////////*****************Examen Fisico"*******************/////////////////////////////
        ////////////////*****************Examen Laboratorio"*******************/////////////////////////////
        public ActionResult CmExamenLab(int IdConsulta, string filter = null, int page = 1, int pageSize = 10, string sort = "iso_antecedente_familiar.iaf_nombre_antecedente_f", string sortdir = "ASC")
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == IdConsulta).First();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records_mort = new PagedList<iso_pedido_exam_consulta_m>();
            ViewBag.filter = filter;
            records_mort.Content = iso_consulta_medica.iso_pedido_exam_consulta_m
                        .Where(x => filter == null ||
                                (x.iso_pedido_examen.ipe_nombre_pexamen.ToLower().Contains(filter.ToLower().Trim())))
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records_mort.TotalRecords = iso_consulta_medica.iso_pedido_exam_consulta_m
                         .Where(x => filter == null ||
                                (x.iso_pedido_examen.ipe_nombre_pexamen.ToLower().Contains(filter.ToLower().Trim())))
                         .Count()
                                ;
            records_mort.CurrentPage = page;
            records_mort.PageSize = pageSize;
            ViewBag.total = records_mort.TotalRecords;
            ViewBag.idConsulta = IdConsulta;
            return PartialView("CmExamenLab", records_mort);
        }
        public ActionResult CrearExamenLab(int idConsulta)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_pedido_exam_consulta_m.ToList();
            List<iso_pedido_examen> list = new List<iso_pedido_examen>();
            foreach (iso_pedido_exam_consulta_m var in asignadosm)
            {
                list.Add(var.iso_pedido_examen);
            }
            var total = db.iso_pedido_examen
                .OrderBy(x => x.ipe_nombre_pexamen).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ipe_id_pexamen", "ipe_nombre_pexamen");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_pedido_exam, "itp_id_tpedido_exam", "itp_nombre_tpedido");
            iso_pedido_exam_consulta_m iso_pedido_exam_consulta_m = new iso_pedido_exam_consulta_m();
            return PartialView("CMParametros/CrearExamenLab", iso_pedido_exam_consulta_m);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ObetenerExamenLabPorTipo(int idConsulta, int idTipo)
        {
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_pedido_exam_consulta_m.ToList();
            List<iso_pedido_examen> list = new List<iso_pedido_examen>();
            foreach (iso_pedido_exam_consulta_m var in asignadosm)
            {
                list.Add(var.iso_pedido_examen);
            }
            var total = db.iso_pedido_examen
                .Where(x => x.ipe_id_tipo_pexamen == idTipo)
                .OrderBy(x => x.ipe_nombre_pexamen).ToList();
            var totalresult = total.Except(list);
            SelectList SelectListParametro = new SelectList(totalresult, "ipe_id_pexamen", "ipe_nombre_pexamen");
            return Json(SelectListParametro, JsonRequestBehavior.AllowGet);
        }
        public string GuardarExamenLab(int idConsulta, iso_pedido_exam_consulta_m iso_pedido_exam_consulta_m)
        {
            //Obtiene Consulta
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();

            ///////imagen/////////
            HttpPostedFileBase file = Request.Files["file1"];
            if (Request.Files.Count > 0 && file.FileName != "")
            {
                iso_pedido_exam_consulta_m.ipc_archivo = ConvertToBytes(file);
            }

            iso_pedido_exam_consulta_m.ipc_id_consulta_medica = iso_consulta_medica.icm_id_consulta;
            iso_pedido_exam_consulta_m.ipc_id_historia_clinica = iso_consulta_medica.icm_id_historia_clinica;
            iso_pedido_exam_consulta_m.ipc_id_personal = iso_consulta_medica.icm_id_personal;
            db.iso_pedido_exam_consulta_m.Add(iso_pedido_exam_consulta_m);
            return db.SaveChanges().ToString();
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        public ActionResult ModificarExamenLab(int idConsulta, int IdParametro)
        {
            ViewBag.idConsulta = idConsulta;
            iso_consulta_medica iso_consulta_medica = db.iso_consulta_medica.Where(x => x.icm_id_consulta == idConsulta).First();
            var asignadosm = iso_consulta_medica.iso_pedido_exam_consulta_m.Where(x => x.ipc_id_pexamen != IdParametro).ToList();
            List<iso_pedido_examen> list = new List<iso_pedido_examen>();
            foreach (iso_pedido_exam_consulta_m var in asignadosm)
            {
                list.Add(var.iso_pedido_examen);
            }
            var total = db.iso_pedido_examen
                .OrderBy(x => x.ipe_nombre_pexamen).ToList();
            var totalresult = total.Except(list);
            ViewBag.SelectListParametro = new SelectList(totalresult, "ipe_id_pexamen", "ipe_nombre_pexamen");
            ViewBag.SelectListTipoParametro = new SelectList(db.iso_tipo_pedido_exam, "itp_id_tpedido_exam", "itp_nombre_tpedido");
            iso_pedido_exam_consulta_m iso_pedido_exam_consulta_m = db.iso_pedido_exam_consulta_m
                .Where(x => x.ipc_id_pexamen == IdParametro && x.ipc_id_consulta_medica == idConsulta).First();
            return PartialView("CMParametros/ModificarExamenLab", iso_pedido_exam_consulta_m);
        }
        public string ActualizarExamenLab(iso_pedido_exam_consulta_m iso_pedido_exam_consulta_m)
        {
            HttpPostedFileBase file = Request.Files["file2"];
            if (Request.Files.Count > 0 && file.FileName != "")
            {
                iso_pedido_exam_consulta_m.ipc_archivo = ConvertToBytes(file);
            }
            db.Entry(iso_pedido_exam_consulta_m).State = EntityState.Modified;
            return db.SaveChanges().ToString();
        }
        ////////////////////////////Examen Laboratorio///////////////////////////////////
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
            else if (TipoAntecente == "AnteVacuna")
            {
                iso_vacuna_consulta_m iso_vacuna_consulta_m = db.iso_vacuna_consulta_m.Where(
                    x => x.ivm_id_consulta_medica == IdConsulta && x.ivm_id_vacuna == IdAntecente).First();
                db.iso_vacuna_consulta_m.Remove(iso_vacuna_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "AnteMujer")
            {
                iso_ante_mujer_consulta_m iso_ante_mujer_consulta_m = db.iso_ante_mujer_consulta_m.Where(
                    x => x.imc_id_consulta_medica == IdConsulta && x.imc_id_antecedente_mujer == IdAntecente).First();
                db.iso_ante_mujer_consulta_m.Remove(iso_ante_mujer_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "Signo")
            {
                iso_sintoma_consulta_m iso_sintoma_consulta_m = db.iso_sintoma_consulta_m.Where(
                    x => x.isc_id_consulta_medica == IdConsulta && x.isc_id_sintoma == IdAntecente).First();
                db.iso_sintoma_consulta_m.Remove(iso_sintoma_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "Sindrome")
            {
                iso_sindrome_consulta_m iso_sindrome_consulta_m = db.iso_sindrome_consulta_m.Where(
                    x => x.isd_id_consulta_medica == IdConsulta && x.isd_id_sindrome == IdAntecente).First();
                db.iso_sindrome_consulta_m.Remove(iso_sindrome_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "Habito")
            {
                iso_habito_consulta_m iso_habito_consulta_m = db.iso_habito_consulta_m.Where(
                    x => x.ihc_id_consulta_medica == IdConsulta && x.ihc_id_habito == IdAntecente).First();
                db.iso_habito_consulta_m.Remove(iso_habito_consulta_m);
                db.SaveChanges();
                mensaje = "Registro eliminado";
            }
            else if (TipoAntecente == "ExamenLab")
            {
                iso_pedido_exam_consulta_m iso_pedido_exam_consulta_m = db.iso_pedido_exam_consulta_m.Where(
                    x => x.ipc_id_consulta_medica == IdConsulta && x.ipc_id_pexamen == IdAntecente).First();
                db.iso_pedido_exam_consulta_m.Remove(iso_pedido_exam_consulta_m);
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
        public FileContentResult GetDoc(int ID)
        {
            iso_pedido_exam_consulta_m cat = db.iso_pedido_exam_consulta_m.FirstOrDefault(c => c.ipc_id_pexamen == ID);
            if (cat.ipc_archivo != null)
            {
                string type = string.Empty;
                type = "application/unknown";
                return File(cat.ipc_archivo, type, cat.ipc_ubicacion_archivo.Replace("[Base de Datos] ", ""));
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
