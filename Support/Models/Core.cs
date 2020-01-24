using System;
using System.Collections.Generic;
using System.Threading;

namespace Support.Models {
    public class Core {
        private static readonly Lazy<Core> lazy =
            new Lazy<Core>(() => new Core());


        /// <summary>
        /// Private constructor of Singleton
        /// </summary>
        private Core() {
            ConfigStruct = new Config
            {
                QueryMinTime = 1, QueryMaxTime = 15, Tm=4, Td=8,
                NumOfOperators = 4, NumOfManagers = 3, NumOfDirectors = 1
            };
        }

        /// <summary>
        /// Start updating timer and fullfill data
        /// </summary>
        public void Start() {
            if (_started)
            {
                return;
            }

            TestData();

            _timer = new Timer(Update, null, 1000, 1000);
            _started = true;
        }

        /// <summary>
        /// Create initial employees and queries
        /// </summary>
        private void TestData() {
            int operators = ConfigStruct.NumOfOperators;
            int managers = ConfigStruct.NumOfManagers;
            int directors = ConfigStruct.NumOfDirectors;

            for (int i = 0; i < operators; ++i) {
                AddEmployee(new Operator());
            }
            for (int i = 0; i < managers; ++i) {
                AddEmployee(new Manager());
            }
            for (int i = 0; i < directors; ++i) {
                AddEmployee(new Director());
            }

            for (int i = 0; i < 20; ++i) {
                Query q = new Query("Default Text");
                AddQuery(q);
            }
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static Core GetInstance() {
            return lazy.Value;
        }

        /// <summary>
        /// Update logic state by timer
        /// </summary>
        /// <param name="state"></param>
        public void Update(object state) {
            if (_queue.Count == 0) {
                return;
            }

            bool nothingToDo = false;
            while (!nothingToDo) {
                var tq = _queue.Peek();
                var diff = DateTime.Now - tq.Item1;

                bool manager = diff.Seconds > ConfigStruct.Tm, director = diff.Seconds > ConfigStruct.Td;

                foreach (var employee in _employees) {
                    if (_queue.Count == 0) {
                        nothingToDo = true;
                        break;
                    }

                    if (employee is Operator o && o.Free) {
                        var timeQuery = _queue.Dequeue();
                        o.Process(timeQuery.Item2);
                    }
                    else if (manager && employee is Manager m && m.Free) {
                        var timeQuery = _queue.Dequeue();
                        m.Process(timeQuery.Item2);
                    }
                    else if (director && employee is Director d && d.Free) {
                        var timeQuery = _queue.Dequeue();
                        d.Process(timeQuery.Item2);
                    }
                    else {
                        nothingToDo = true;
                    }
                }
            }
        }

        /// <summary>
        /// Callback for completed query
        /// </summary>
        /// <param name="employee">who completed</param>
        /// <param name="query"></param>
        private void CompletedCallback(Employee employee, Query query) {
            if (!_history.ContainsKey(employee)) {
                _history[employee] = new List<Query>();
            }
            _history[employee].Add(query);
        }

        public void AddEmployee(Employee employee) {
            employee.SetCompletedCallback(CompletedCallback);
            _employees.Add(employee);
        }

        public void AddQuery(Query query) {
            query.ProcessTimeSec = (uint) _random.Next(ConfigStruct.QueryMinTime, ConfigStruct.QueryMaxTime);

            _queries.Add(query);

            bool freeFound = false;
            foreach (var employee in _employees) {
                if (employee is Operator o) {
                    if (o.Free) {
                        o.Process(query);
                        freeFound = true;
                        break;
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

        public Dictionary<Employee, List<Query>> GetHistory() {
            return _history;
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
            foreach (var query in _queries) {
                if (query.Id == id) {
                    if (query.Status == Query.StatusEnum.Completed) {
                        return;
                    }

                    if (query.Status == Query.StatusEnum.Processing) {
                        foreach (var employee in _employees) {
                            if (employee.Query?.Id == id) {
                                employee.Query.Status = Query.StatusEnum.Cancelled;
                                employee.SetFree();
                            }
                        }
                    }
                    else {
                        query.Status = Query.StatusEnum.Cancelled;
                    }
                }
            }
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

        /// <summary>
        /// History of completed queries
        /// </summary>
        private Dictionary<Employee, List<Query>> _history = new Dictionary<Employee, List<Query>>();

        /// <summary>
        /// Random for filling query random processing time
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Timer for updating state of logic
        /// </summary>
        private Timer _timer;
        /// <summary>
        /// Is timer started
        /// </summary>
        private bool _started;

        /// <summary>
        /// Configuration of logic
        /// </summary>
        public Config ConfigStruct { get; set; }

        /// <summary>
        /// Struct of configuration of logic
        /// </summary>
        public struct Config 
        {
            /// <summary>
            /// Time for manager to work
            /// </summary>
            public int Tm { get; set; }
            /// <summary>
            /// Time for director to work
            /// </summary>
            public int Td { get; set; }

            /// <summary>
            /// Min query processing time
            /// </summary>
            public int QueryMinTime { get; set; }
            /// <summary>
            /// Max query processing time
            /// </summary>
            public int QueryMaxTime { get; set; }

            /// <summary>
            /// Number of operators to create
            /// </summary>
            public int NumOfOperators { get; set; }
            /// <summary>
            /// Number of managers to create
            /// </summary>
            public int NumOfManagers { get; set; }
            /// <summary>
            /// Number of directors to create
            /// </summary>
            public int NumOfDirectors { get; set; }
        }
    }
}