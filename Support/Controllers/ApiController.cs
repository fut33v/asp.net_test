﻿using System.Collections.Generic;
using System.Web.Http;
using Support.Models;

namespace Support.Controllers
{
    public class WebAPIController : ApiController
    {

        private Core _core = Core.GetInstance();

        [HttpGet]
        public Query.StatusEnum? GetStatus(int id) {
            return _core.GetStatus(id);
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
