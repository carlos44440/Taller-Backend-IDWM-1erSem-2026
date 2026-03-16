namespace TiendaUCN.src.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public required int Quantity { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
    }
}