namespace GerenciamentoPedidos.DTOs
{
    public class PedidoDto
    {
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ItemPedidoDto> Itens { get; set; } = new List<ItemPedidoDto>();
    }
}