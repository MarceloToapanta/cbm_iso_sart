﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBM_SART.Controllers
{
    public class SistemaGestionController : Controller
    {
        //
        // GET: /SistemaGestion/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nivel_1(int id)
        {
            ViewBag.id = id;
            return View();
        }
	}
}