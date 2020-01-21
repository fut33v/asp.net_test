using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support.Classes {
    public class Core {
        private static readonly Lazy<Core> lazy =
            new Lazy<Core>(() => new Core());

        private Core() {
            uint operators = 6;
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
        }

        public static Core GetInstance() {
            return lazy.Value;
        }

        public void Loop() {
            while (true) {
                // pause for second
                System.Threading.Thread.Sleep(1000);

                foreach (var employee in _employees) {
                    if (employee is Operator o) {
                        if (o.Free && _queue.Count != 0) {
                            var timeQuery = _queue.Dequeue();
                            o.Process(timeQuery.Item2);
                        }
                    }
                }

                // if queue not empty then check time and give it to top dicks

                foreach (var timeQuery in _queue) {
                    var query = timeQuery.Item2;
                    var diff = DateTime.Now - timeQuery.Item1;
                    bool manager = diff.Seconds > Tm, director = diff.Seconds > Td;

                    foreach (var employee in _employees) {
                        if (employee is Manager m && manager) {
                            m.Process(query);
                        }

                        if (employee is Director d && director) {
                            d.Process(query);
                        }
                    }
                }
            }
        }

        public void AddQuery(Query query) {
            // set time for processing query
            Random r = new Random();
            query.ProcessTimeSec = (uint) r.Next(QueryMinTime, QueryMaxTime);

            _queries.Add(query);

            foreach (var employee in _employees) {
                if (employee is Operator o) {
                    if (o.Free) {
                        o.Process(query);
                        _queries.Add(query);
                    }
                    else {
                        _queue.Enqueue(new Tuple<DateTime, Query>(DateTime.Now, query));
                    }
                }
            }
        }


        /// <summary>
        /// Get status of Query
        /// </summary>
        /// <param name="id"></param>
        public Query.StatusEnum GetStatus(int id) {
            // TODO: do something if not found
            var q = _queries.Find(x => x.Id == id);
            return q.Status;
        }

        /// <summary>
        /// Get all queries list
        /// </summary>
        /// <returns></returns>
        public List<Query> GetQueries() {
            return _queries;
        }

        public void CancelQuery(uint id) {
            foreach (var employee in _employees) {
                if (employee.Query.Id == id) {
                    employee.SetFree();
                }
            }

            _queries.Remove(_queries.Find(x => x.Id == id));
        }


        private Queue<Tuple<DateTime, Query>> _queue = new Queue<Tuple<DateTime, Query>>();
        private List<Query> _queries = new List<Query>();

        private List<Employee> _employees = new List<Employee>();


        private const uint Tm = 4;
        private const uint Td = 8;
        private const int QueryMinTime = 1;
        private const int QueryMaxTime = 15;
    }
}