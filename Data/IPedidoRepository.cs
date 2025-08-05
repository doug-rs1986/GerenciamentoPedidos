using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;

namespace GerenciamentoPedidos.Data
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<Pedido> GetByIdAsync(int id);
        Task<int> AddAsync(PedidoDto pedidoDto);
        Task<bool> UpdateAsync(int id, PedidoDto pedidoDto);
        Task<bool> DeleteAsync(int id);
    }
}