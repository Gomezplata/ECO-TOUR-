using System.ComponentModel.DataAnnotations;

namespace EcoTour.Models;

// ENTIDAD DEPENDIENTE (lado "N" de la relación): cada reserva pertenece a un destino
public class Reserva
{
    // Clave primaria
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 60 caracteres")]
    [Display(Name = "Nombre del turista")]
    public string NombreTurista { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    [StringLength(100)]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; }

    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }

    [Range(1, 10, ErrorMessage = "El número de personas debe estar entre 1 y 10")]
    [Display(Name = "Número de personas")]
    public int Personas { get; set; }

    [Required(ErrorMessage = "Selecciona una fecha")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de la caminata")]
    public DateTime? Fecha { get; set; }

    // Se calcula en el controlador: precio del destino x personas
    public int Total { get; set; }

    // Clave foránea: guarda el Id del destino elegido
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un destino")]
    [Display(Name = "Destino")]
    public int DestinoId { get; set; }

    // Propiedad de navegación: el destino completo al que pertenece
    public Destino Destino { get; set; }
}
