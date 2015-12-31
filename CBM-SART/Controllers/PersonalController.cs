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
using CBM_SART.ActionFilter;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class PersonalController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        // GET: /Personal/
        //public async Task<ActionResult> Index()
        //{
        //    var iso_personal = db.iso_personal.Include(i => i.iso_cargo).Include(i => i.iso_departamento).Include(i => i.iso_empresa).Include(i => i.iso_puesto_trabajo).Include(i => i.iso_puesto_trabajo1);
        //    return View(await iso_personal.ToListAsync());
        //}

        public ActionResult Index(string filter = null, int page = 1, int pageSize = 15, string sort = "ipe_apellido_paterno", string sortdir = "ASC")
        {
            int ide = IdEmpresa();
            ViewBag.nombre_empresa = NombreEmpresa();
            if (String.IsNullOrEmpty(filter)) { filter = null; }
            var records = new PagedList<iso_personal>();
            ViewBag.filter = filter;
            records.Content = db.iso_personal
                        .Where(x => filter == null ||
                                (x.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))
                                || x.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim())
                              )
                        .Where(x => x.ipe_id_empresa == ide)
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.iso_personal
                         .Where(x => filter == null ||
                                (x.ipe_nombre_personal.ToLower().Contains(filter.ToLower().Trim()))
                                || x.ipe_apellido_paterno.ToLower().Contains(filter.ToLower().Trim()))
                         .Where(x => x.ipe_id_empresa == ide).Count();
                        
            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);

            //return View(db.iso_empresa.ToList());
        }

        // GET: /Personal/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_personal iso_personal = await db.iso_personal.FindAsync(id);
            if (iso_personal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa", IdEmpresa());
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return PartialView("Details", iso_personal);
        }

        // GET: /Personal/Create
        public ActionResult Create()
        {
            int ide = IdEmpresa();
            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.Where(x => x.ide_id_empresa == ide).OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.Where(x => x.iem_cod_empresa == ide).OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa");
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            var iso_personal = new iso_personal();
            return PartialView("Create", iso_personal);
            //return View();
        }

        // POST: /Personal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ipe_id_personal,ipe_ced_ruc_personal,ipe_nombre_personal,ipe_fecha_nacimiento,ipe_dir_domicilio,ipe_telf_1,ipe_telf_2,ipe_celular,ipe_fecha_ingreso,ipe_id_cargo,ipe_id_empresa,ipe_id_departamento,ipe_curriculum,ipe_empresa_anterior,ipe_descripcion_estudios,ipe_descripcion_experiencia,ipe_archivo_curriculum,ipe_estado_personal,ipe_id_jefe,ipe_nro_afiliacion,ipe_genero,ipe_profesion,ipe_trab_habitual,ipe_horario_reg_ini,ipe_horario_reg_fin,ipe_salario_diario,ipe_salario_mensual,ipe_tiempo_servicio,ipe_estado_civil,ipe_lugar_nacimiento,ipe_email,ipe_nivel_educativo,ipe_apellido_paterno,ipe_edad,ipe_tipo_sangre,ipe_factor_rh,ipe_tiempo_per_a,ipe_tiempo_per_m,ipe_descrip_cargo,ipe_id_puesto_trabajo,ipe_cambio_area,ipe_id_centro_costo,ipe_apellido_materno,ipe_etnia,ipe_cedula_militar,ipe_ocupacion_anteriro,ipe_familiares_empresa,ipe_tiene_experiencia,ipe_nacionalidad,ipe_vulnerabilidad,ipe_codigo_personal,ipe_foto")] iso_personal iso_personal)
        {
            if (ModelState.IsValid)
            {
                ///////imagen/////////
                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_personal.ipe_foto = ConvertToBytes(file);
                }
                ///////Curriculum/////////
                HttpPostedFileBase filec = Request.Files["filedoc"];
                if (filec.FileName != "")
                {
                    iso_personal.ipe_archivo_curriculum = ConvertToBytes(filec);
                }
                db.iso_personal.Add(iso_personal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa");
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return PartialView("Create", iso_personal);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        // GET: /Personal/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            int ide = IdEmpresa();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_personal iso_personal = await db.iso_personal.FindAsync(id);
            if (iso_personal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.Where(x => x.ide_id_empresa == ide).OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.Where(x => x.iem_cod_empresa == ide).OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa");
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return PartialView("Edit", iso_personal);
        }

        // POST: /Personal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ipe_id_personal,ipe_ced_ruc_personal,ipe_nombre_personal,ipe_fecha_nacimiento,ipe_dir_domicilio,ipe_telf_1,ipe_telf_2,ipe_celular,ipe_fecha_ingreso,ipe_id_cargo,ipe_id_empresa,ipe_id_departamento,ipe_curriculum,ipe_empresa_anterior,ipe_descripcion_estudios,ipe_descripcion_experiencia,ipe_archivo_curriculum,ipe_estado_personal,ipe_id_jefe,ipe_nro_afiliacion,ipe_genero,ipe_profesion,ipe_trab_habitual,ipe_horario_reg_ini,ipe_horario_reg_fin,ipe_salario_diario,ipe_salario_mensual,ipe_tiempo_servicio,ipe_estado_civil,ipe_lugar_nacimiento,ipe_email,ipe_nivel_educativo,ipe_apellido_paterno,ipe_edad,ipe_tipo_sangre,ipe_factor_rh,ipe_tiempo_per_a,ipe_tiempo_per_m,ipe_descrip_cargo,ipe_id_puesto_trabajo,ipe_cambio_area,ipe_id_centro_costo,ipe_apellido_materno,ipe_etnia,ipe_cedula_militar,ipe_ocupacion_anteriro,ipe_familiares_empresa,ipe_tiene_experiencia,ipe_nacionalidad,ipe_vulnerabilidad,ipe_codigo_personal,ipe_foto")] iso_personal iso_personal)
        {
            if (ModelState.IsValid)
            {
                ///////imagen/////////
                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_personal.ipe_foto = ConvertToBytes(file);
                }
                ///////Curriculum/////////
                HttpPostedFileBase filec = Request.Files["filedoc"];
                if (filec.FileName != "")
                {
                    iso_personal.ipe_archivo_curriculum = ConvertToBytes(filec);
                }
                db.Entry(iso_personal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa");
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return PartialView("Edit", iso_personal);
        }

        // GET: /Personal/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_personal iso_personal = await db.iso_personal.FindAsync(id);
            if (iso_personal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ipe_id_cargo = new SelectList(db.iso_cargo.OrderBy(x => x.icg_nombre), "icg_id_cargo", "icg_nombre");
            ViewBag.ipe_id_departamento = new SelectList(db.iso_departamento.OrderBy(x => x.ide_nombre_departamento), "ide_id_departamento", "ide_nombre_departamento");
            ViewBag.ipe_id_empresa = new SelectList(db.iso_empresa.OrderBy(x => x.iem_nombre_empresa), "iem_cod_empresa", "iem_nombre_empresa");
            ViewBag.ipe_id_puesto_trabajo = new SelectList(db.iso_puesto_trabajo.OrderBy(x => x.ipt_nombre_puesto_t), "ipt_id_puesto_t", "ipt_nombre_puesto_t");
            return PartialView("Delete", iso_personal);
        }

        // POST: /Personal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_personal iso_personal = await db.iso_personal.FindAsync(id);
            db.iso_personal.Remove(iso_personal);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
        //Curriculum del Personal
        public FileContentResult GetDoc(int ID)
        {
            iso_personal cat = db.iso_personal.FirstOrDefault(c => c.ipe_id_personal == ID);
            if (cat != null)
            {
                string type = string.Empty;
                type = "application/unknown";
                return File(cat.ipe_archivo_curriculum, type);
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
