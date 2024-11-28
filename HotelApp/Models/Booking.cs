using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public string UserID { get; set; }
        [Required]
        [ForeignKey(nameof(Room))]
        public int RoomID { get; set; }
        [ForeignKey(nameof(Voucher))]
        public int? VoucherID { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        [Column(TypeName ="decimal(18,2)")]
        public Decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Total {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal? Paid { get; set; }
        [Required]
        public int? PayType { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public AppUser User { get; set; }
        public Room Room { get; set; }
        public Voucher Voucher { get; set; }
        public ICollection<CCCD> CCCDs { get; set; }
        public string? PaymentCode { get; set; }

    }
}
