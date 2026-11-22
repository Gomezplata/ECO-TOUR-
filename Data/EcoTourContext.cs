using EcoTour.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoTour.Data;

// El DbContext es el puente entre el código C# y la base de datos
public class EcoTourContext : DbContext
{
    public EcoTourContext(DbContextOptions<EcoTourContext> options) : base(options)
    {
    }

    // Cada DbSet se convierte en una tabla
    public DbSet<Destino> Destinos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación 1:N -> una Reserva tiene un Destino, un Destino tiene muchas Reservas
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Destino)
            .WithMany(d => d.Reservas)
            .HasForeignKey(r => r.DestinoId)
            .OnDelete(DeleteBehavior.Restrict); // no se puede borrar un destino que tenga reservas

        // Datos iniciales que se insertan con la migración
        modelBuilder.Entity<Destino>().HasData(
            new Destino { Id = 1, Nombre = "Parque Arví", Descripcion = "Senderos entre bosque de niebla.", PrecioPorPersona = 45000 },
            new Destino { Id = 2, Nombre = "Río Claro", Descripcion = "Caminata junto al cañón y cuevas.", PrecioPorPersona = 80000 },
            new Destino { Id = 3, Nombre = "Jardín", Descripcion = "Avistamiento de aves y cascadas.", PrecioPorPersona = 60000 }
        );
    }
}
