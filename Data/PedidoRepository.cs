using Microsoft.Data.SqlClient;
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
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CustomerId as ClienteId, OrderDate as DataPedido, TotalAmount as ValorTotal, Status FROM Orders";
            return await connection.QueryAsync<Pedido>(sql);
        }

        public async Task<Pedido> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CustomerId as ClienteId, OrderDate as DataPedido, TotalAmount as ValorTotal, Status FROM Orders WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(PedidoDto pedidoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Orders (CustomerId, OrderDate, TotalAmount, Status) VALUES (@ClienteId, @DataPedido, @ValorTotal, @Status); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, new { pedidoDto.ClienteId, pedidoDto.DataPedido, pedidoDto.ValorTotal, pedidoDto.Status });
            return id;
        }

        public async Task<bool> UpdateAsync(int id, PedidoDto pedidoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Orders SET CustomerId = @ClienteId, OrderDate = @DataPedido, Status = @Status WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { pedidoDto.ClienteId, pedidoDto.DataPedido, pedidoDto.Status, Id = id });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Orders WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<int> AddItemAsync(ItemPedido item)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, item);
        }

        public async Task<IEnumerable<ItemPedido>> GetItensByPedidoIdAsync(int pedidoId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, OrderId as PedidoId, ProductId as ProdutoId, Quantity as Quantidade, UnitPrice as PrecoUnitario FROM OrderItems WHERE OrderId = @PedidoId";
            return await connection.QueryAsync<ItemPedido>(sql, new { PedidoId = pedidoId });
        }

        public async Task<IEnumerable<Pedido>> GetFilteredAsync(int? clienteId = null, string status = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CustomerId as ClienteId, OrderDate as DataPedido, TotalAmount as ValorTotal, Status FROM Orders WHERE 1=1";
            var parameters = new DynamicParameters();

            if (clienteId.HasValue)
            {
                sql += " AND CustomerId = @ClienteId";
                parameters.Add("@ClienteId", clienteId.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sql += " AND Status = @Status";
                parameters.Add("@Status", status);
            }

            return await connection.QueryAsync<Pedido>(sql, parameters);
        }
    }
}