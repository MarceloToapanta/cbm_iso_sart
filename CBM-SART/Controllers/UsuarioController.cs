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

namespace CBM_SART.Controllers
{
    public class UsuarioController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        //Login
        public ActionResult Ingresar(string url)
        {
            ViewBag.url = url;
            return View();
        }
        [HttpPost]
        public ActionResult Ingresar(iso_usuario iso_usuario, string url)
        {
            int iso_usuario_count = db.iso_usuario.Where(x => x.ius_login == iso_usuario.ius_login && x.ius_password == iso_usuario.ius_password).Count();
            if (iso_usuario_count == 1)
            {
                iso_usuario iso_usuario_actual = db.iso_usuario.Where(x => x.ius_login == iso_usuario.ius_login && x.ius_password == iso_usuario.ius_password).First();
                Session["Usuario"] = iso_usuario_actual;
                return RedirectToAction(url);
            }
            else
            {
                return View();
            }
        }
        //Logout
        public ActionResult Salir()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index","Home");
        }
        // GET: /Usuario/
        public async Task<ActionResult> Index()
        {
            return View(await db.iso_usuario.ToListAsync());
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
            return View(iso_usuario);
        }

        // GET: /Usuario/Create
        public ActionResult Create()
        {
            return View();
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
                db.iso_usuario.Add(iso_usuario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iso_usuario);
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
            return View(iso_usuario);
        }

        // POST: /Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ius_id_usuario,ius_nombre_usuario,ius_desc_usuario,ius_login,ius_password,ius_permisos,ius_id_departamento,ius_iniciales,ius_email,ius_firma_digital,ius_deshabilitado,ius_firma_archivo,ius_representante_direccion,ius_miembro_comite,ius_foto_ruta,ius_foto_archivo,ius_tipo_usuario,ius_carga_documento")] iso_usuario iso_usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iso_usuario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iso_usuario);
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
            return View(iso_usuario);
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
