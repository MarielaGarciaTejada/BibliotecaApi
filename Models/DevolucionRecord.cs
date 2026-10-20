namespace BibliotecaApi.Models;

public record DevolucionGet
(
    int IdDevolucion,
    int IdPrestamo,
    DateTime FechaDevolucion,
    string? Observaciones
)
{
    public DevolucionGet() : this(0, 0, default, null) { }
}
public record DevolucionSet(int IdPrestamo, string? Observaciones);
