using Dominio.Abstracciones;

namespace Dominio.Prestamos;

public static class ErroresPrestamo
{

public static readonly Error NoEncontrado = new(
        "Prestamo.NoEncontrado",
        "No se encontró el préstamo con el identificador especificado"
    );

public static readonly Error YaDevuelto = new(
        "Prestamo.YaDevuelto",
        "El préstamo ya ha sido devuelto"
    );

public static readonly Error NoActivo = new(
        "Prestamo.NoActivo",
        "El préstamo no está activo"
    );

public static readonly Error LibroNoDisponible = new(
        "Prestamo.LibroNoDisponible",
        "El libro no está disponible para préstamo"
    );
}
