namespace BibliotecaApi.Models;

public record PrestamoGet
(
    int IdPrestamo,
    int IdEstudiante,
    int IdLibro,
    DateTime FechaPrestamo,
    DateTime FechaDevolucionEsperada
)
{
    public PrestamoGet() : this(0, 0, 0, default, default) { }
}
public record PrestamoSet
(
    int IdEstudiante,
    int IdLibro,
    DateTime FechaDevolucionEsperada
);
