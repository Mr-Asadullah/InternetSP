namespace InternetSP.Models
{
    public class Packge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Price { get; set; }
        public string Img { get; set; }
        public virtual Volume Volume { get; set; }
        public int VolumeId { get; set; }
        public virtual Speed Speed { get; set; }
        public int SpeedId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
