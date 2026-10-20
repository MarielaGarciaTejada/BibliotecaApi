namespace BibliotecaApi.Models;

public record LibroGet
(
    int IdLibro,
    string Titulo,
    string Autor,
    int Stock
)
{
    public LibroGet() : this(0, string.Empty, string.Empty, 0) { }
}
public record LibroSet
(
    string Titulo,
    string Autor,
    int Stock
);
