using System.Data.SqlClient;
using Dapper;
using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;

namespace GerenciamentoPedidos.Data
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _connectionString;
        public PedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CustomerId as ClienteId, OrderDate as DataPedido, TotalValue as ValorTotal, Status FROM Orders";
            return await connection.QueryAsync<Pedido>(sql);
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CustomerId as ClienteId, OrderDate as DataPedido, TotalValue as ValorTotal, Status FROM Orders WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(PedidoDto pedidoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Orders (CustomerId, OrderDate, TotalValue, Status) VALUES (@ClienteId, @DataPedido, @ValorTotal, @Status); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, new { pedidoDto.ClienteId, pedidoDto.DataPedido, pedidoDto.ValorTotal, pedidoDto.Status });
            return id;
        }

        public async Task<bool> UpdateAsync(int id, PedidoDto pedidoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Orders SET CustomerId = @ClienteId, OrderDate = @DataPedido, TotalValue = @ValorTotal, Status = @Status WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { pedidoDto.ClienteId, pedidoDto.DataPedido, pedidoDto.ValorTotal, pedidoDto.Status, Id = id });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Orders WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}