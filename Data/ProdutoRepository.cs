using Microsoft.Data.SqlClient;
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
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products";
                return await connection.QueryAsync<Produto>(sql);
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar produtos.", ex);
            }
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar produto por Id.", ex);
            }
        }

        public async Task<IEnumerable<Produto>> SearchAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Parâmetro de busca não pode ser vazio.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Name as Nome, Description as Descricao, Price as Preco, StockQuantity as QuantidadeEstoque FROM Products WHERE Name LIKE @q";
                return await connection.QueryAsync<Produto>(sql, new { q = "%" + nome + "%" });
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar produtos por nome.", ex);
            }
        }

        public async Task<int> AddAsync(ProdutoDto produtoDto)
        {
            if (produtoDto == null)
                throw new ArgumentNullException(nameof(produtoDto));
            if (string.IsNullOrWhiteSpace(produtoDto.Nome) || produtoDto.Preco <= 0)
                throw new ArgumentException("Nome e Preço são obrigatórios e válidos.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO Products (Name, Description, Price, StockQuantity) 
                            VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque); 
                            SELECT CAST(SCOPE_IDENTITY() as int);";
                return await connection.QuerySingleAsync<int>(sql, produtoDto);
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao adicionar produto.", ex);
            }
        }

        public async Task<bool> UpdateAsync(int id, ProdutoDto produtoDto)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (produtoDto == null)
                throw new ArgumentNullException(nameof(produtoDto));
            if (string.IsNullOrWhiteSpace(produtoDto.Nome) || produtoDto.Preco <= 0)
                throw new ArgumentException("Nome e Preço são obrigatórios e válidos.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"UPDATE Products SET Name = @Nome, Description = @Descricao, Price = @Preco, StockQuantity = @QuantidadeEstoque WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, new { produtoDto.Nome, produtoDto.Descricao, produtoDto.Preco, produtoDto.QuantidadeEstoque, Id = id });
                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar produto.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "DELETE FROM Products WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao excluir produto.", ex);
            }
        }

        public async Task<bool> HasStockAsync(int produtoId, int quantidade)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT StockQuantity FROM Products WHERE Id = @Id";
            var estoque = await connection.QuerySingleOrDefaultAsync<int>(sql, new { Id = produtoId });
            return estoque >= quantidade;
        }

        public async Task<bool> DecrementStockAsync(int produtoId, int quantidade)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Products SET StockQuantity = StockQuantity - @Quantidade WHERE Id = @Id AND StockQuantity >= @Quantidade";
            var rows = await connection.ExecuteAsync(sql, new { Id = produtoId, Quantidade = quantidade });
            return rows > 0;
        }
    }
}