using System.Data.SqlClient;
using Dapper;
using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;

namespace GerenciamentoPedidos.Data
{
    public class ProdutoRepository : IProdutoRepository 
    {
        private readonly string _connectionString;
        public ProdutoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products";
            return await connection.QueryAsync<Produto>(sql);
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Produto>> SearchAsync(string nome)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products WHERE Name LIKE @q";
            return await connection.QueryAsync<Produto>(sql, new { q = "%" + nome + "%" });
        }

        public async Task<int> AddAsync(ProdutoDto produtoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Products (Name, Description, Price, StockQuantity) 
                        VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque); 
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.QuerySingleAsync<int>(sql, produtoDto);
        }

        public async Task<bool> UpdateAsync(int id, ProdutoDto produtoDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Products SET Name = @Nome, Description = @Descricao, Price = @Preco, StockQuantity = @QuantidadeEstoque WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, produtoDto.QuantidadeEstoque, Id = id });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Products WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}