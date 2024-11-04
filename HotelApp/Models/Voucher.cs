using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public int Status {  get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Discount { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; }
    }
}
