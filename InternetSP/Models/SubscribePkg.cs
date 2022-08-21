namespace InternetSP.Models
{
    public class SubscribePkg
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public DateTime Dateime { get; set; }
        public virtual Packge Packge { get; set; }
        public int PackgeId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
