namespace BlazorClient.Models;

public record EstudianteGet(int IdEstudiante, string Nombre, string Matricula, string Email)
{
    public EstudianteGet() : this(0, string.Empty, string.Empty, string.Empty) { }
}
public record EstudianteSet(string Nombre, string Matricula, string Email);
