using CBM_SART.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBM_SART.Controllers
{
    public class PermisoVigilaciaController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        //
        // GET: /PermisoVigilacia/
        public ActionResult Index(int? IdUsuario)
        {
            var userlist = new SelectList(db.iso_usuario.Where(x => x.ius_deshabilitado == "N").ToList(), "ius_id_usuario", "ius_nombre_usuario", IdUsuario);
            ViewData["Usuarios"] = userlist;
            if (IdUsuario != null)
            {
                ViewBag.Usuario = db.iso_usuario.Find(IdUsuario);
            }
            return View();
        }
	}
}