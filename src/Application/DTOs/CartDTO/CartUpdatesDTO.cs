namespace TiendaUCN.src.Application.DTOs.CartDTO
{
    public class CartUpdatesDTO
    {
        public List<string> UpdatedItemsNames { get; set; } = null!;
        public List<string> RemovedItemsNames { get; set; } = null!;
    }
}