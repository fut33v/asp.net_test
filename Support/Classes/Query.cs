using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support.Classes
{
    public class Query {
        public enum StatusEnum {
            Completed = 0,
            NotCompleted,
            Queued
        };

        public Query(string text) {
            Status = StatusEnum.NotCompleted;
            Id = IdCounter++;
            Text = text;
        }

        public uint Id { get; }
        public string Text { get; }
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Time needed for process this query
        /// </summary>
        public uint ProcessTimeSec { get; set; }

        private static uint IdCounter = 0;
    }
}
