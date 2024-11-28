namespace HotelApp.Areas.Admin.ViewModels
{
    public class RoomVM
    {
        public int TypeId { get; set; }
        public int AreaId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
    }
}
