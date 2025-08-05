using Microsoft.Data.SqlClient;
using Dapper;
using GerenciamentoPedidos.Models;
using GerenciamentoPedidos.DTOs;

namespace GerenciamentoPedidos.Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _connectionString;
        public ClienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers";
            return await connection.QueryAsync<Cliente>(sql);
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string nomeOuEmail)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers WHERE Name LIKE @q OR Email LIKE @q";
            return await connection.QueryAsync<Cliente>(sql, new { q = "%" + nomeOuEmail + "%" });
        }

        public async Task<int> AddAsync(ClienteDto clienteDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Customers (Name, Email, Phone, CreatedDate) VALUES (@Nome, @Email, @Telefone, @DataCadastro); SELECT CAST(SCOPE_IDENTITY() as int);";
            var id = await connection.QuerySingleAsync<int>(sql, new { clienteDto.Nome, clienteDto.Email, clienteDto.Telefone, DataCadastro = DateTime.Now });
            return id;
        }

        public async Task<bool> UpdateAsync(int id, ClienteDto clienteDto)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE Customers SET Name = @Nome, Email = @Email, Phone = @Telefone WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { clienteDto.Nome, clienteDto.Email, clienteDto.Telefone, Id = id });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Customers WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}