using System.Web.Http;
using System.Web.Mvc;
using Support.Models;

namespace Support.Controllers
{
    public class HomeController : Controller
    {

        private Core _core = Core.GetInstance();

        public ActionResult Index() {
            //_core.Update();

            var queries =  _core.GetQueries();
            ViewBag.Queries = queries;

            var employees = _core.GetEmployees();
            ViewBag.Employees = employees;

            var history = _core.GetHistory();
            ViewBag.History = history;

            ViewBag.Config = _core.ConfigStruct;

            return View();
        }


        [System.Web.Mvc.HttpPost()]
        public ActionResult Post([FromBody] Core.Config config) {

            _core.ConfigStruct = config;
            _core.Start();
            //return "Config changed <a href='/'>back<a>";
            return Redirect("/");
        }

    }
}