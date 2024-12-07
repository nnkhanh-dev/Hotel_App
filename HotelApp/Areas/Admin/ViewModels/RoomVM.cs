namespace HotelApp.Areas.Admin.ViewModels
{
    public class RoomVM
    {
        public int Id { get; set; }
        public string RoomTypeName { get; set; } // Tên loại phòng
        public string AreaName { get; set; } // Tên khu vực
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
    }

}
