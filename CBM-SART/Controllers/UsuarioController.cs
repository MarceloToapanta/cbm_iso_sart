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
    
    public class UsuarioController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        //Login
        public ActionResult Ingresar(string url, iso_usuario iso_usuario)
        {
            if (Session["Usuario"] == null)
            {
                ViewBag.url = url;
                return PartialView(iso_usuario);
            }
            else
            {
                Session["AvisoUser"] = "Usted ya inició sesion en el sistema";
                return Redirect("/");
            }
            
        }
        [HttpPost]
        public ActionResult UsuarioActual(string url, iso_usuario iso_usuario)
        {
            int iso_usuario_count = db.iso_usuario.Where(x => x.ius_login == iso_usuario.ius_login && x.ius_password == iso_usuario.ius_password).Count();
            if (iso_usuario_count == 1)
            {
                iso_usuario iso_usuario_actual = db.iso_usuario.Where(x => x.ius_login == iso_usuario.ius_login && x.ius_password == iso_usuario.ius_password).First();
                Session["Usuario"] = iso_usuario_actual;
                if (url == "" || url == null)
                {
                    url = "/";
                }
                return Redirect(url);
                // return Json(new { ok = true, msj = "Usuario encontrado" });
            }
            else
            {
                if (iso_usuario.ius_login == "" || iso_usuario.ius_login == null)
                {
                    ModelState.AddModelError("", "Ingresar Usuario");
                    Session["ErroUser"] = "Ingresar Usuario";
                    return RedirectToAction("Ingresar", iso_usuario);
                }
                else
                {
                    //return Json(new { ok = false, msj = "El Usuario y Clave que has indicado no pertenece a ninguna cuenta.             \n Por favor, asegúrate de escribirlos correctamente" });
                    ModelState.AddModelError("", "El Usuario y Clave que has indicado no pertenece a ninguna cuenta.             \n Por favor, asegúrate de escribirlos correctamente");
                    Session["ErroUser"] = "El Usuario y Clave que has indicado no pertenece a ninguna cuenta.             \n Por favor, asegúrate de escribirlos correctamente";
                    return RedirectToAction("Ingresar", iso_usuario);
                }

                //int iso_usuario_e = db.iso_usuario.Where(x => x.ius_login == iso_usuario.ius_login).Count();
                //if (iso_usuario_e == 0)
                //{
                //    ModelState.AddModelError("", "Usuario no encontrado");
                //    return Json(new { ok = false, msj = "Usuario no encontrado" });
                //}
                //else
                //{
                //    ModelState.AddModelError("", "Clave no válida");
                //    return Json(new { ok = false, msj = "Clave no válida"});
                //}
            }
        }
        //Logout
        public ActionResult Salir()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index","Home");
        }
        public string ExisteUsuario()
        {
            if (Session["Usuario"] == null)
            {
                return "false";
            }
            return "true";
        }
        public string loginExiste(string ius_login)
        {
            if (db.iso_usuario.Where(x => x.ius_login == ius_login).Count() > 0)
            {
                var user = db.iso_usuario.Where(x => x.ius_login == ius_login).First();
                return "true";
            }
            else
            {
                return "false";
            }
            
        }
        [UserFilter]
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 20, string sort = "ius_nombre_usuario", string sortdir = "ASC")
        {

            if (String.IsNullOrEmpty(filter)) { filter = null; }
            ViewBag.nombre_empresa = NombreEmpresa();
            var records = new PagedList<iso_usuario>();
            ViewBag.filter = filter;
            records.Content = db.iso_usuario
                        .Where(x => filter == null ||
                                (x.ius_nombre_usuario.ToLower().Contains(filter.ToLower().Trim())))
                        .OrderBy(sort + " " + sortdir)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            // Count
            records.TotalRecords = db.iso_usuario
                         .Where(x => filter == null ||
                                (x.ius_nombre_usuario.ToLower().Contains(filter.ToLower().Trim()))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;
            ViewBag.total = records.TotalRecords;

            return View(records);
        }

        // GET: /Usuario/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_usuario iso_usuario = await db.iso_usuario.FindAsync(id);
            if (iso_usuario == null)
            {
                return HttpNotFound();
            }
            //return View(iso_usuario);
            return PartialView("Details", iso_usuario);
        }

        // GET: /Usuario/Create
        public ActionResult Create()
        {

            var iso_usuario = new iso_usuario();
            return PartialView("Create", iso_usuario);
            //return View();
        }

        // POST: /Usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ius_id_usuario,ius_nombre_usuario,ius_desc_usuario,ius_login,ius_password,ius_permisos,ius_id_departamento,ius_iniciales,ius_email,ius_firma_digital,ius_deshabilitado,ius_firma_archivo,ius_representante_direccion,ius_miembro_comite,ius_foto_ruta,ius_foto_archivo,ius_tipo_usuario,ius_carga_documento")] iso_usuario iso_usuario)
        {
            if (ModelState.IsValid)
            {

                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_usuario.ius_foto_archivo = ConvertToBytes(file);
                }
                db.iso_usuario.Add(iso_usuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return PartialView("Create", iso_usuario);
            //return View(iso_usuario);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        // GET: /Usuario/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_usuario iso_usuario = await db.iso_usuario.FindAsync(id);
            if (iso_usuario == null)
            {
                return HttpNotFound();
            }
            //return View(iso_usuario);
            return PartialView("Edit", iso_usuario);
        }

        // POST: /Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ius_id_usuario,ius_nombre_usuario,ius_desc_usuario,ius_login,ius_password,ius_permisos,ius_id_departamento,ius_iniciales,ius_email,ius_firma_digital,ius_deshabilitado,ius_firma_archivo,ius_representante_direccion,ius_miembro_comite,ius_foto_ruta,ius_foto_archivo,ius_tipo_usuario,ius_carga_documento")] iso_usuario iso_usuario)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file1"];
                if (file.FileName != "")
                {
                    iso_usuario.ius_foto_archivo = ConvertToBytes(file);
                }
                db.Entry(iso_usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (controllerName != "Usuario")
                {
                    return Redirect("/" + controllerName + "/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
                
            }
            return PartialView("Edit", iso_usuario);
            //return View(iso_usuario);
        }

        // GET: /Usuario/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            iso_usuario iso_usuario = await db.iso_usuario.FindAsync(id);
            if (iso_usuario == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", iso_usuario);
        }

        // POST: /Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            iso_usuario iso_usuario = await db.iso_usuario.FindAsync(id);
            db.iso_usuario.Remove(iso_usuario);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public FileContentResult GetImage(int ID)
        {
            iso_usuario cat = db.iso_usuario.FirstOrDefault(c => c.ius_id_usuario == ID);
            if (cat != null)
            {
                string type = "image/jpeg";
                return File(cat.ius_foto_archivo, type);
            }
            else
            {
                return null;
            }
        }

        public string Getlogin(int ID)
        {
            iso_usuario cat = db.iso_usuario.FirstOrDefault(c => c.ius_id_usuario == ID);
            if (cat != null)
            {
                return cat.ius_login;
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
