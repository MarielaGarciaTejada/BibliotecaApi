using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services
{
    public class LibrosService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("AzureSqlConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena AzureSqlConnection");

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<LibroGet>> GetAllAsync()
        {
            const string consulta = "SELECT IdLibro, Titulo, Autor, Stock FROM Libros;";
            using var conn = CreateConnection();
            return await conn.QueryAsync<LibroGet>(consulta);
        }

        public async Task<LibroGet?> GetByIdAsync(int idLibro)
        {
            const string consulta = "SELECT IdLibro, Titulo, Autor, Stock FROM Libros WHERE IdLibro = @IdLibro;";
            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LibroGet>(consulta, new { IdLibro = idLibro });
        }

        public async Task<int> CreateAsync(LibroSet libro)
        {
            const string consulta = @"
            INSERT INTO Libros (Titulo, Autor, Stock)
            VALUES (@Titulo, @Autor, @Stock);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = CreateConnection();
            return await conn.ExecuteScalarAsync<int>(consulta, libro);
        }

        public async Task<bool> UpdateAsync(int idLibro, LibroSet libro)
        {
            const string consulta = @"
            UPDATE Libros
            SET Titulo = @Titulo, Autor = @Autor, Stock = @Stock
            WHERE IdLibro = @IdLibro;";

            using var conn = CreateConnection();
            var filas = await conn.ExecuteAsync(consulta, new
            {
                IdLibro = idLibro,
                libro.Titulo,
                libro.Autor,
                libro.Stock
            });
            return filas > 0;
        }

        public async Task<bool> DeleteAsync(int idLibro)
        {
            const string consulta = "DELETE FROM Libros WHERE IdLibro = @IdLibro;";
            using var conn = CreateConnection();
            var filas = await conn.ExecuteAsync(consulta, new { IdLibro = idLibro });
            return filas > 0;
        }
    }
}
