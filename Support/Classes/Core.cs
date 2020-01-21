using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support.Classes {
    public class Core {
        private static readonly Lazy<Core> lazy =
            new Lazy<Core>(() => new Core());

        private Core() {
        }

        public static Core GetInstance() {
            return lazy.Value;
        }

        public void Loop() {
            while (true) {
                // pause for second
                // update all the shit 
            }
        }

        public void AddQuery(Query query) {
            // if there is free operator than give it to him
            // else add to queue
        }


        public void AddEmployee(Employee employee) {
            _employees.Add(employee);
        }

        private Queue<Query> _queries = new Queue<Query>();
        private List<Employee> _employees = new List<Employee>();

        private uint Tm;
        private uint Td;
        private const uint QueryMinTime = 5;
        private const uint QueryMaxTime = 10;

        /// <summary>
        /// Get status of Query
        /// </summary>
        /// <param name="id"></param>
        public Query.StatusEnum GetStatus(in int id) {
            throw new NotImplementedException();
        }

        public void CancelQuery(in uint id) {

            // find in queue and cancel it 

            //throw new NotImplementedException();
        }
    }
}