using System.ComponentModel.DataAnnotations;

namespace EcoTour.Models;

// ENTIDAD PRINCIPAL (lado "1" de la relación): un destino tiene muchas reservas
public class Destino
{
    // Clave primaria: EF la reconoce porque se llama Id
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, ErrorMessage = "Máximo 60 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; }

    // int porque los pesos colombianos no manejan centavos
    [Range(1000, 5000000, ErrorMessage = "El precio debe estar entre $1.000 y $5.000.000")]
    [Display(Name = "Precio por persona")]
    public int PrecioPorPersona { get; set; }

    // Propiedad de navegación: todas las reservas de este destino
    public List<Reserva> Reservas { get; set; }
}
