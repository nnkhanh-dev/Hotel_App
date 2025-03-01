using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class CCCD
    {
        public int Id { get; set; }
        public string FrontImg { get; set; }
        public string BackImg { get; set; }
        [ForeignKey(nameof(Booking))]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
