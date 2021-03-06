﻿using System;

namespace Support.Models
{
    public class Employee
    {
        public Employee() {
            Id = _idCounter++;
        }

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
                _completedCallback?.Invoke(this, Query);
                Query = null;
                return true;
            }

            return false;
        }

        public bool Free => CheckFree();

        public void SetFree() {
            Query = null;
        }

        public void SetCompletedCallback(Action<Employee, Query> callback) {
            _completedCallback = callback;
        }

        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Employee);
        }
        public bool Equals(Employee obj)
        {
            return obj != null && obj.Id == this.Id;
        }


        public string Position { get; protected set; }
        public Query Query { get; private set; }
        /// <summary>
        /// Time of start of processing current query
        /// </summary>
        private DateTime _startTime ;
        /// <summary>
        /// unique Id of employee
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Counter of all the employees
        /// </summary>
        private static int _idCounter = 0;
        /// <summary>
        /// Callback for completed query
        /// </summary>
        private Action<Employee, Query> _completedCallback;
    }
}
