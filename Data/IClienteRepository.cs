using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciamentoPedidos.Data
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task<IEnumerable<Cliente>> SearchAsync(string nomeOuEmail);
        Task<int> AddAsync(ClienteDto clienteDto);
        Task<bool> UpdateAsync(int id, ClienteDto clienteDto);
        Task<bool> DeleteAsync(int id);
    }
}