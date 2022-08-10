namespace InternetSP.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime PaymentTime { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Packge Package { get; set; }
        public int PackageId { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public int PaymentStatusId { get; set; }
    }
}
