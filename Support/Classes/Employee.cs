using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Support.Classes
{
    public class Employee
    {
        public virtual void Process(Query query)
        {
            _startTime = DateTime.Now;
        }

        private bool CheckFree()
        {
            if (Query == null)
            {
                return true;
            }

            var diff = DateTime.Now - _startTime;
            if (diff.Seconds > Query.ProcessTimeSec)
            {
                Query = null;
                return true;
            }

            return false;
        }

        public bool Free => CheckFree();

        public void SetFree() {
            Query = null;
        }

        public Query Query { get; private set; }
        private DateTime _startTime ;
    }
}
