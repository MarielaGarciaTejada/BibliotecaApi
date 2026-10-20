using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services
{
    public class PrestamosService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("AzureSqlConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena AzureSqlConnection");

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<PrestamoGet>> GetAllAsync()
        {
            const string consulta = "SELECT IdPrestamo, IdEstudiante, IdLibro, FechaPrestamo, FechaDevolucionEsperada FROM Prestamos;";
            using var conn = CreateConnection();
            return await conn.QueryAsync<PrestamoGet>(consulta);
        }

        public async Task<PrestamoGet?> GetByIdAsync(int idPrestamo)
        {
            const string consulta = "SELECT IdPrestamo, IdEstudiante, IdLibro, FechaPrestamo, FechaDevolucionEsperada FROM Prestamos WHERE IdPrestamo = @IdPrestamo;";
            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<PrestamoGet>(consulta, new { IdPrestamo = idPrestamo });
        }

        public async Task<int> CreateAsync(PrestamoSet prestamo)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();
            using var trans = conn.BeginTransaction();

            try
            {
                // Validar stock con IdLibro
                const string verificarStockLibro = "SELECT Stock FROM Libros WHERE IdLibro = @IdLibro;";
                var stock = await conn.ExecuteScalarAsync<int?>(verificarStockLibro, new { prestamo.IdLibro }, trans);

                if (stock is null)
                    throw new KeyNotFoundException($"El libro con ID {prestamo.IdLibro} no existe.");

                if (stock <= 0)
                    throw new InvalidOperationException("No hay stock disponible para prestar este libro.");

                const string insertarPrestamo = @"
                INSERT INTO Prestamos (IdEstudiante, IdLibro, FechaPrestamo, FechaDevolucionEsperada)
                VALUES (@IdEstudiante, @IdLibro, GETDATE(), @FechaDevolucionEsperada);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var prestamoId = await conn.ExecuteScalarAsync<int>(insertarPrestamo, prestamo, trans);

                // Descontar stock
                const string actualizarStockLibro = "UPDATE Libros SET Stock = Stock - 1 WHERE IdLibro = @IdLibro;";
                await conn.ExecuteAsync(actualizarStockLibro, new { prestamo.IdLibro }, trans);

                trans.Commit(); // Confirma todos los cambios en la base de datos si no hubo errores
                return prestamoId;
            }
            catch
            {
                trans.Rollback(); // Revierte y deshace todas las operaciones previas si ocurrió algún fallo
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int idPrestamo)
        {
            const string consulta = "DELETE FROM Prestamos WHERE IdPrestamo = @IdPrestamo;";
            using var conn = CreateConnection();
            var filas = await conn.ExecuteAsync(consulta, new { IdPrestamo = idPrestamo });
            return filas > 0;
        }


    }
}
