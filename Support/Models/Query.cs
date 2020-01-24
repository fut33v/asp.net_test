namespace Support.Models
{
    public class Query {
        public enum StatusEnum {
            Completed = 0,
            Processing,
            Queued,
            Cancelled
        };

        public Query(string text) {
            Status = StatusEnum.Processing;
            Id = _idCounter++;
            Text = text;
        }

        public uint Id { get; }
        public string Text { get; }
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Time needed for process this query
        /// </summary>
        public uint ProcessTimeSec { get; set; }

        /// <summary>
        /// Counter of all the queries
        /// </summary>
        private static uint _idCounter = 1;
    }
}
