using HotelApp.Models;

namespace HotelApp.Areas.Admin.ViewModels
{
    public class RoomDetailsVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string RoomTypeName { get; set; }
        public string AreaName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public List<string> Amenities { get; set; }
        public List<Image> Images { get; set; }
    }
}
