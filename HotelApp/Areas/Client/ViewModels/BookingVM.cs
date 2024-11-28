namespace HotelApp.Areas.Client.ViewModels
{
    public class BookingVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public int? VoucherId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int? PayType { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public int AreaId { get; set; }
        public string? AreaName { get; set; }
        public decimal Price { get; set; }
        public decimal? Paid { get; set; }
        public decimal? DiscountRoom { get; set; }
        public decimal? DiscountVoucher { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public string? StatusStr { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Description { get; set; }
        public string? PaymentCode { get; set; }

    }
}
