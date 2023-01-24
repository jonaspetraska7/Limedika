namespace LimedikaMVC.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserAction { get; set; }
    }
}
