using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Support.Models;

namespace Support.Controllers
{
    public class HomeController : Controller
    {

        private Core _core = Core.GetInstance();

        public ActionResult Index() {
            _core.Update();
            var queries =  _core.GetQueries();
            ViewBag.Queries = queries;

            var employees = _core.GetEmployees();
            ViewBag.Employees = employees;
            return View();
        }

    }
}