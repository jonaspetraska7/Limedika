using LinqToDB.Mapping;

namespace Common.Entities
{
    public class Log
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserAction { get; set; }
    }
}
