using System.Globalization;

namespace EcoTour.Models;

// Ayuda para mostrar los precios como $45.000 sin depender de la configuración del servidor
public static class Moneda
{
    public static string Pesos(decimal valor)
    {
        return "$" + valor.ToString("N0", new NumberFormatInfo { NumberGroupSeparator = "." });
    }
}
