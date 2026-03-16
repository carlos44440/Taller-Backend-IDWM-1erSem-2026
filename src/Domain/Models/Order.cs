namespace TiendaUCN.src.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string DeliveryCode { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public required string DeliveryAddress { get; set; }
        public required int TotalPrice { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; } = [];
        public bool IsDeleted { get; set; } = false;
    }
}