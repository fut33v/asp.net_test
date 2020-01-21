using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Support.Classes;

namespace Support.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SupportApiController : ControllerBase
    {
        // GET: api/SupportApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "get:status", "post:add", "post:cancel" };
        }

        /// <summary>
        /// Get status of existing Query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("status", Name = "GetStatus")]
        public int GetStatus(int id) {
            _core.GetStatus(id);
            return id;
        }

        [HttpGet("queries")]
        public List<Query> GetQueries() {
            return _core.GetQueries();
        }


        /// <summary>
        /// Add new Query
        /// </summary>
        /// <param name="text"></param>
        [HttpPost("add")]
        public void PostAdd([FromBody] string text) {
            Query query = new Query(text);
            _core.AddQuery(query);
        }

        /// <summary>
        /// Cancel existing query
        /// </summary>
        [HttpPost("cancel")]
        public void PostCancel([FromBody] uint id) {
            _core.CancelQuery(id);
        }

        private Core _core = Core.GetInstance();

    }
}
