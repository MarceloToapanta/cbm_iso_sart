using CBM_SART.ActionFilter;
using CBM_SART.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class PermisoVigilaciaController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        //
        // GET: /PermisoVigilacia/
        public ActionResult Index(int? IdUsuario)
        {
            ViewBag.nombre_empresa = NombreEmpresa();
            var userlist = new SelectList(db.iso_usuario.Where(x => x.ius_deshabilitado == "N").ToList(), "ius_id_usuario", "ius_nombre_usuario", IdUsuario);
            ViewData["Usuarios"] = userlist;
            ViewBag.Modulos = db.iso_modulo;
            if (IdUsuario != null)
            {
                ViewBag.Usuario = db.iso_usuario.Find(IdUsuario);
            }
            return View();
        }
        public ActionResult Actualizar(int? IdUsuario, int[] modulo)
        {
            iso_usuario u = db.iso_usuario.Find(IdUsuario);
            //Elimina los registros anteriores
            if (u != null) {
                u.iso_modulo.Clear();
                db.SaveChanges();
                //Ingresa nuevos valores
                foreach (var nm in modulo.ToList())
                {
                    iso_modulo n = db.iso_modulo.Find(nm);
                    u.iso_modulo.Add(n);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index",new{ IdUsuario = IdUsuario});
        }
    }
}