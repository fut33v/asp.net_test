using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support.Classes
{
    public class Core
    {
        public void Loop()
        {
            while (true)
            {
                // pause for second
                // update all the shit 
            }
        }

        public void AddQuery(Query query)
        {
            // if there is free operator than give it to him
            // else add to queue
        }


        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        private Queue<Query> _queries = new Queue<Query>();
        private List<Employee> _employees = new List<Employee>();

        private uint Tm;
        private uint Td;
        private const uint QueryMinTime = 5;
        private const uint QueryMaxTime = 10;
    }
}
