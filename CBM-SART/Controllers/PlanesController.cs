using CBM_SART.ActionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBM_SART.Controllers
{
    [UserFilter]
    public class PlanesController : BaseController
    {
        //
        // GET: /Planes/
        public ActionResult Index()
        {
            return View();
        }
	}
}