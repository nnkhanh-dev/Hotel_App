using HotelApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.ViewModels
{
    public class RoomVM
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public string StatusStr { get; set; }
        public List<string> ImageUrls { get; set; } 
        public List<string> AmenityNames { get; set; } 
    }
}
