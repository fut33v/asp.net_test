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
