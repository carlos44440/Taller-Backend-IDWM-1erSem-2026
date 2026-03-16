namespace TiendaUCN.src.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required int Price { get; set; }
        public required int Stock { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public List<CartItem> CartItems { get; } = [];
        public List<OrderItem> OrderItems { get; } = [];
        public bool IsDeleted { get; set; } = false;
    }
}