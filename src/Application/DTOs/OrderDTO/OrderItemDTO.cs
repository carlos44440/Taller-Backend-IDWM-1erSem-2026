namespace TiendaUCN.src.Application.DTOs.OrderDTO
{
    public class OrderItemDTO
    {
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public required string MainImageURL { get; set; }
        public required string UnitPriceAtMoment { get; set; }
        public required string SubtotalPrice { get; set; }
        public required int Quantity { get; set; }
    }
}