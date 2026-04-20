using TiendaUCN.src.Domain.Models;

namespace TiendaUCN.src.Application.DTOs.CartDTO
{
    public class CheckoutResultDTO
    {
        public required CartDTO CartUpdated { get; set; }
        public CartUpdatesDTO CartUpdatesDTO { get; set; } = null!;
    }
}