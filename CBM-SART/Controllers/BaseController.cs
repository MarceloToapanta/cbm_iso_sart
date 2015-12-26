using CBM_SART.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBM_SART.Controllers
{
    public class BaseController : Controller
    {
        private cbm_iso_sart_entities db = new cbm_iso_sart_entities();
        
        public CBM_SART.Models.iso_empresa EmpresaActual()
        {
            CBM_SART.Models.iso_empresa empresa_actual = db.iso_empresa.Where(x => x.iem_actividad == "si").First();    
            if (empresa_actual != null)
            {
                return empresa_actual;
            }
            else
            {
                return null;
            }
        }
        public string NombreEmpresa()
        {
            return EmpresaActual().iem_nombre_empresa;
        }
        public int IdEmpresa()
        {
            return EmpresaActual().iem_cod_empresa;
        }	
    }
}