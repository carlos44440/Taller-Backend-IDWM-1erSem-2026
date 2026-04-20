namespace TiendaUCN.src.Application.DTOs.OrderDTO
{
    public class OrderDetailDTO
    {
        public required string Code { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required string TotalPrice { get; set; }
        public required List<OrderItemDTO> Items { get; set; }
    }
}