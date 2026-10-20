using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services
{
    public class DevolucionesService(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("AzureSqlConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena AzureSqlConnection");

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<DevolucionGet>> GetAllAsync()
        {
            const string consulta = "SELECT IdDevolucion, IdPrestamo, FechaDevolucion, Observaciones FROM Devoluciones;";
            using var conn = CreateConnection();
            return await conn.QueryAsync<DevolucionGet>(consulta);
        }

        public async Task<DevolucionGet?> GetByIdAsync(int idDevolucion)
        {
            const string consulta = "SELECT IdDevolucion, IdPrestamo, FechaDevolucion, Observaciones FROM Devoluciones WHERE IdDevolucion = @IdDevolucion;";
            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<DevolucionGet>(consulta, new { IdDevolucion = idDevolucion });
        }

        public async Task<int> CreateAsync(DevolucionSet devolucion)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();
            using var trans = conn.BeginTransaction();

            try
            {
                // Obtener el IdLibro del préstamo
                const string getPrestamoConsulta = "SELECT IdLibro FROM Prestamos WHERE IdPrestamo = @IdPrestamo;";
                var libroId = await conn.ExecuteScalarAsync<int?>(getPrestamoConsulta, new { devolucion.IdPrestamo }, trans);

                if (libroId is null)
                    throw new KeyNotFoundException($"No existe ningún préstamo con ID {devolucion.IdPrestamo}.");

                // Validar que no tenga devolución previa
                const string validarDevolucion = "SELECT COUNT(1) FROM Devoluciones WHERE IdPrestamo = @IdPrestamo;";
                var count = await conn.ExecuteScalarAsync<int>(validarDevolucion, new { devolucion.IdPrestamo }, trans);

                if (count > 0)
                    throw new InvalidOperationException($"El préstamo {devolucion.IdPrestamo} ya fue devuelto previamente.");

                // Registrar la devolución
                const string insertarDevolucionLibro = @"
                INSERT INTO Devoluciones (IdPrestamo, FechaDevolucion, Observaciones)
                VALUES (@IdPrestamo, GETDATE(), @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var devolucionId = await conn.ExecuteScalarAsync<int>(insertarDevolucionLibro, devolucion, trans);

                // Reintegrar stock al libro
                const string updateStockSql = "UPDATE Libros SET Stock = Stock + 1 WHERE IdLibro = @IdLibro;";
                await conn.ExecuteAsync(updateStockSql, new { IdLibro = libroId.Value }, trans);

                trans.Commit();
                return devolucionId;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int idDevolucion)
        {
            const string consulta = "DELETE FROM Devoluciones WHERE IdDevolucion = @IdDevolucion;";
            using var conn = CreateConnection();
            var filas = await conn.ExecuteAsync(consulta, new { IdDevolucion = idDevolucion });
            return filas > 0;
        }
    }
}
