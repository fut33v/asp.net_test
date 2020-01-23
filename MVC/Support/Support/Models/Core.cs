using System;
using System.Collections.Generic;

namespace Support.Models {
    public class Core {
        private static readonly Lazy<Core> lazy =
            new Lazy<Core>(() => new Core());

        private Core() {

            TestData();
        }

        private void TestData() {
            uint operators = 3;
            uint managers = 2;
            uint directors = 1;

            for (int i = 0; i < operators; ++i) {
                _employees.Add(new Operator());
            }

            for (int i = 0; i < managers; ++i) {
                _employees.Add(new Manager());
            }

            for (int i = 0; i < directors; ++i) {
                _employees.Add(new Director());
            }

            for (int i = 0; i < 20; ++i) {
                Query q = new Query("Default Text");
                AddQuery(q);
            }
        }

        public static Core GetInstance() {
            return lazy.Value;
        }

        public void Update() {

            if (_queue.Count == 0) {
                return;
            }

            bool nothingToDo = false;
            while (!nothingToDo) {
                var tq = _queue.Peek(); 
                var query = tq.Item2;
                var diff = DateTime.Now - tq.Item1;

                bool manager = diff.Seconds > Tm, director = diff.Seconds > Td;

                foreach (var employee in _employees) {
                    if (_queue.Count == 0) {
                        nothingToDo = true;
                        break;
                    }
                    if (employee is Operator o && o.Free) {
                        var timeQuery = _queue.Dequeue();
                        o.Process(timeQuery.Item2);
                    } else if (manager && employee is Manager m && m.Free) {
                        var timeQuery = _queue.Dequeue();
                        m.Process(timeQuery.Item2);
                    } else if (director && employee is Director d && d.Free) {
                        var timeQuery = _queue.Dequeue();
                        d.Process(timeQuery.Item2);
                    }
                    else {
                        nothingToDo = true;
                    }

                }

            }

        }

        public void AddQuery(Query query) {
            // set time for processing query
            query.ProcessTimeSec = (uint) _random.Next(QueryMinTime, QueryMaxTime);

            _queries.Add(query);

            bool freeFound = false;
            foreach (var employee in _employees) {
                if (employee is Operator o) {
                    if (o.Free) {
                        o.Process(query);
                        freeFound = true;
                    }
                }
            }

            if (!freeFound) {
                query.Status = Query.StatusEnum.Queued;
                _queue.Enqueue(new Tuple<DateTime, Query>(DateTime.Now, query));
            }

        }


        /// <summary>
        /// Get status of Query
        /// </summary>
        /// <param name="id"></param>
        public Query.StatusEnum? GetStatus(int id) {
            var q = _queries.Find(x => x.Id == id);
            return q?.Status;
        }

        /// <summary>
        /// Get all queries list
        /// </summary>
        /// <returns></returns>
        public List<Query> GetQueries() {
            return _queries;
        }

        /// <summary>
        /// Get all employees list
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetEmployees() {
            return _employees;
        }

        /// <summary>
        /// Cancel existing query 
        /// </summary>
        /// <param name="id"></param>
        public void CancelQuery(uint id) {
            foreach (var employee in _employees) {
                if (employee.Query.Id == id) {
                    employee.SetFree();
                }
            }

            // or set another status?
            _queries.Remove(_queries.Find(x => x.Id == id));
        }


        /// <summary>
        /// Queue of not assigned queries
        /// </summary>
        private Queue<Tuple<DateTime, Query>> _queue = new Queue<Tuple<DateTime, Query>>();

        /// <summary>
        /// All the queries
        /// </summary>
        private List<Query> _queries = new List<Query>();

        /// <summary>
        /// All the employees
        /// </summary>
        private List<Employee> _employees = new List<Employee>();

        private Random _random = new Random();
        private const uint Tm = 4;
        private const uint Td = 8;
        private const int QueryMinTime = 1;
        private const int QueryMaxTime = 15;
    }
}