namespace TiendaUCN.src.Domain.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required int Quantity { get; set; }
        public required int PriceAtMoment { get; set; }
        public required string NameAtMoment { get; set; }
        public string BrandAtMoment { get; set; } = string.Empty;
        public string CategoryAtMoment { get; set; } = string.Empty;
        public int SubtotalPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
    }
}