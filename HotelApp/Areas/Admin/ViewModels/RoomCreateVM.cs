using HotelApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelApp.Areas.Admin.ViewModels
{
    public class RoomCreateVM
    {
        public int TypeId { get; set; }
        public string Code { get; set; }
        public int AreaId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Area> Areas { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<int> SelectedAmenities { get; set; }
    }
}
