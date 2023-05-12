using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WizardBase.Models;

namespace WizardBase.Controllers
{
    public class WizardController : Controller
    {
        // GET: Wizard
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClienteStep(Clientes cliente)
        {
            
            if (ModelState.IsValid)
            {

                return View("ClientesDetails");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ClienteDetailsStep(ClientesDetails clienteDetails)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            return View();
        }
    }
}