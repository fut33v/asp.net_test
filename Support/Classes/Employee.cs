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
            // get current time
            _startTime = DateTime.Now;
            // save it

            // process for some time, get this time?
        }

        private bool CheckFree()
        {
            if (_query == null)
            {
                return true;
            }

            var diff = DateTime.Now - _startTime;
            if (diff.Seconds > _query.ProcessTimeSec)
            {
                _query = null;
                return true;
            }

            return false;
        }

        public bool Free => CheckFree();
        //private bool free;

        private Query _query = null;
        private DateTime _startTime ;
    }
}
