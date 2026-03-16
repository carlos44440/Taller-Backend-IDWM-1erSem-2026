namespace TiendaUCN.src.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required int Price { get; set; }
        public required int Stock { get; set; }
        public int BrandId { get; set; } // Establece la relación con Brand (Una marca puede tener muchos productos)
        public Brand Brand { get; set; } = null!;
        public int CategoryId { get; set; } // Establece la relación con Category (Una categoría puede tener muchos productos)
        public Category Category { get; set; } = null!;
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public List<CartItem> CartItems { get; } = []; // Relación con CartItem (Un producto puede estar en muchos CartItems)
        public List<OrderItem> OrderItems { get; } = []; // Relación con OrderItem (Un producto puede estar en muchos OrderItems)
        public bool IsDeleted { get; set; } = false;
    }
}