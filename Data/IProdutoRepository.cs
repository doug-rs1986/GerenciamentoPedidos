using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;

namespace GerenciamentoPedidos.Data
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(int id);
        Task<IEnumerable<Produto>> SearchAsync(string nome);
        Task<int> AddAsync(ProdutoDto produtoDto);
        Task<bool> UpdateAsync(int id, ProdutoDto produtoDto);
        Task<bool> DeleteAsync(int id);
    }
}