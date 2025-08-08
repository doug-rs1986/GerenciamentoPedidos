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
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers";
                return await connection.QueryAsync<Cliente>(sql);
            }
            catch (SqlException ex)
            {
                // Logar erro
                throw new Exception("Erro ao buscar clientes.", ex);
            }
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Cliente>(sql, new { Id = id });
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar cliente por Id.", ex);
            }
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string nomeOuEmail)
        {
            if (string.IsNullOrWhiteSpace(nomeOuEmail))
                throw new ArgumentException("Parâmetro de busca não pode ser vazio.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"SELECT Id, Name as Nome, Email, Phone as Telefone, CreatedDate as DataCadastro FROM Customers WHERE Name LIKE @q OR Email LIKE @q";
                return await connection.QueryAsync<Cliente>(sql, new { q = "%" + nomeOuEmail + "%" });
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar clientes por nome ou email.", ex);
            }
        }

        public async Task<int> AddAsync(ClienteDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto));
            if (string.IsNullOrWhiteSpace(clienteDto.Nome) || string.IsNullOrWhiteSpace(clienteDto.Email))
                throw new ArgumentException("Nome e Email são obrigatórios.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"INSERT INTO Customers (Name, Email, Phone, CreatedDate) VALUES (@Nome, @Email, @Telefone, @DataCadastro); SELECT CAST(SCOPE_IDENTITY() as int);";
                var id = await connection.QuerySingleAsync<int>(sql, new { clienteDto.Nome, clienteDto.Email, clienteDto.Telefone, DataCadastro = DateTime.Now });
                return id;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao adicionar cliente.", ex);
            }
        }

        public async Task<bool> UpdateAsync(int id, ClienteDto clienteDto)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto));
            if (string.IsNullOrWhiteSpace(clienteDto.Nome) || string.IsNullOrWhiteSpace(clienteDto.Email))
                throw new ArgumentException("Nome e Email são obrigatórios.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"UPDATE Customers SET Name = @Nome, Email = @Email, Phone = @Telefone WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, new { clienteDto.Nome, clienteDto.Email, clienteDto.Telefone, Id = id });
                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar cliente.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "DELETE FROM Customers WHERE Id = @Id";
                var rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao excluir cliente.", ex);
            }
        }
    }
}