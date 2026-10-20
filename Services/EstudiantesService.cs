using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BibliotecaApi.Models;

namespace BibliotecaApi.Services;

public class EstudiantesService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("AzureSqlConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena AzureSqlConnection");
    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<EstudianteGet>> GetAllAsync()
    {
        const string sql = "SELECT IdEstudiante, Nombre, Matricula, Email FROM Estudiantes;";
        using var conn = CreateConnection();
        return await conn.QueryAsync<EstudianteGet>(sql);
    }

    public async Task<EstudianteGet?> GetByIdAsync(int idEstudiante)
    {
        const string consulta = "SELECT IdEstudiante, Nombre, Matricula, Email FROM Estudiantes WHERE IdEstudiante = @IdEstudiante;";
        using var conn = CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<EstudianteGet>(consulta, new { IdEstudiante = idEstudiante });
    }

    public async Task<int> CreateAsync(EstudianteSet estudiante)
    {
        // En Azure SQL e SQL Server se usa SCOPE_IDENTITY() para obtener el ID generado
        const string consulta = @"
            INSERT INTO Estudiantes (Nombre, Matricula, Email)
            VALUES (@Nombre, @Matricula, @Email);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(consulta, estudiante);
    }

    public async Task<bool> UpdateAsync(int idEstudiante, EstudianteSet estudiante)
    {
        const string consulta = @"
            UPDATE Estudiantes
            SET Nombre = @Nombre, Matricula = @Matricula, Email = @Email
            WHERE IdEstudiante = @IdEstudiante;";

        using var conn = CreateConnection();
        var filas = await conn.ExecuteAsync(consulta, new
        {
            IdEstudiante = idEstudiante,
            estudiante.Nombre,
            estudiante.Matricula,
            estudiante.Email
        });
        return filas > 0;
    }

    public async Task<bool> DeleteAsync(int idEstudiante)
    {
        const string consulta = "DELETE FROM Estudiantes WHERE IdEstudiante = @IdEstudiante;";
        using var conn = CreateConnection();
        var filas = await conn.ExecuteAsync(consulta, new { IdEstudiante = idEstudiante });
        return filas > 0;
    }

}
