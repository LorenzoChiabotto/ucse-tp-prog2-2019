﻿using Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private static IServicioWeb servicio = new ImplementacionService.ImplementacionService();

        public ActionResult Index()
        {
            ViewBag.Grupo = servicio.ObtenerNombreGrupo();
            
            return View();
        }        
    }
}