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
                list.Add(var.iso_sintoma );
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
