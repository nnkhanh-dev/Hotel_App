namespace HotelApp.Models
{
    public class CCCD
    {
        public int Id { get; set; }
        public string FrontImg { get; set; }
        public string BackImg { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
