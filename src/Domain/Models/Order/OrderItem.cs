namespace TiendaUCN.src.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required int Quantity { get; set; }
        public required string NameAtMoment { get; set; }
        public required string DescriptionAtMoment { get; set; }
        public required int UnitPriceAtMoment { get; set; }
        public required string BrandAtMoment { get; set; }
        public required string CategoryAtMoment { get; set; }
        public required string ImageUrlAtMoment { get; set; }
        public required int SubtotalPrice { get; set; }
        public int OrderId { get; set; } // Establece la relación con Order (Un pedido puede tener muchos OrderItems)
        public Order Order { get; set; } = null!;
    }
}