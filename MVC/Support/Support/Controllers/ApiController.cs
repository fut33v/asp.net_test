using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Support.Models;

namespace Support.Controllers
{
    public class WebAPIController : ApiController
    {

        private Core _core = Core.GetInstance();

        [HttpGet]
        public int GetStatus(int id) {
            _core.GetStatus(id);
            return id;
        }

        [HttpGet]
        public List<Query> GetQueries() {
            return _core.GetQueries();
        }

        [HttpPost()]
        public void PostAdd([FromBody] string text) {
            Query query = new Query(text);
            _core.AddQuery(query);
        }

        [HttpPost]
        public void PostCancel([FromBody] uint id) {
            _core.CancelQuery(id);
        }

    }
}
