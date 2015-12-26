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
    public class HomeController : BaseController
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        public ActionResult Index()
        {
            var empresa_actual = EmpresaActual();
            if (empresa_actual != null) {
                ViewBag.empresa_actual = empresa_actual;
            }
            return View();
        }

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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}