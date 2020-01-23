using System;

namespace Support.Models
{
    public class Employee
    {
        public virtual void Process(Query query) {
            Query = query;
            query.Status = Query.StatusEnum.Processing;
            _startTime = DateTime.Now;
        }

        private bool CheckFree()
        {
            if (Query == null)
            {
                return true;
            }

            var diff = DateTime.Now - _startTime;
            if (diff.Seconds > Query.ProcessTimeSec) {
                Query.Status = Query.StatusEnum.Completed;
                Query = null;
                return true;
            }

            return false;
        }

        public bool Free => CheckFree();

        public void SetFree() {
            Query = null;
        }


        public string Name { get; protected set; }
        public Query Query { get; private set; }
        private DateTime _startTime ;
    }
}
