using EcoTour.Data;
using EcoTour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoTour.Controllers;

public class ReservasController : Controller
{
    private readonly EcoTourContext _context;

    public ReservasController(EcoTourContext context)
    {
        _context = context;
    }

    // CONSULTAR - GET /Reservas  o  /Reservas?destinoId=2 (filtrar por destino)
    public async Task<IActionResult> Index(int? destinoId)
    {
        // Include trae el destino de cada reserva para mostrar su nombre
        var consulta = _context.Reservas
            .Include(r => r.Destino)
            .AsQueryable();

        if (destinoId.HasValue)
        {
            consulta = consulta.Where(r => r.DestinoId == destinoId.Value);
            var destino = await _context.Destinos.FindAsync(destinoId.Value);
            ViewBag.Filtro = destino?.Nombre;
        }

        var reservas = await consulta.OrderBy(r => r.Fecha).ToListAsync();
        return View(reservas);
    }

    // CREAR - GET muestra el formulario con la lista desplegable de destinos
    [HttpGet]
    public async Task<IActionResult> Crear(int? destinoId)
    {
        await CargarDestinos();
        return View(new Reserva { DestinoId = destinoId ?? 0, Personas = 1 });
    }

    // CREAR - POST valida, calcula el total y hace el INSERT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("NombreTurista,Correo,Personas,Fecha,DestinoId")] Reserva reserva)
    {
        if (reserva.Fecha.HasValue && reserva.Fecha.Value.Date < HoyEnColombia())
        {
            ModelState.AddModelError(nameof(Reserva.Fecha), "La fecha no puede ser anterior a hoy");
        }

        // Se busca el destino elegido en la base de datos para usar su precio
        var destino = await _context.Destinos.FindAsync(reserva.DestinoId);
        if (reserva.DestinoId > 0 && destino == null)
        {
            ModelState.AddModelError(nameof(Reserva.DestinoId), "El destino no existe");
        }

        if (!ModelState.IsValid)
        {
            await CargarDestinos();
            return View(reserva);
        }

        reserva.Total = destino.PrecioPorPersona * reserva.Personas;
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva de {reserva.NombreTurista} a {destino.Nombre} guardada. Total: {Moneda.Pesos(reserva.Total)}";
        return RedirectToAction(nameof(Index));
    }

    // MODIFICAR - GET busca la reserva y la muestra en el formulario
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        await CargarDestinos();
        return View(reserva);
    }

    // MODIFICAR - POST recalcula el total y hace el UPDATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("Id,NombreTurista,Correo,Personas,Fecha,DestinoId")] Reserva reserva)
    {
        if (id != reserva.Id)
        {
            return NotFound();
        }

        var destino = await _context.Destinos.FindAsync(reserva.DestinoId);
        if (reserva.DestinoId > 0 && destino == null)
        {
            ModelState.AddModelError(nameof(Reserva.DestinoId), "El destino no existe");
        }

        if (!ModelState.IsValid)
        {
            await CargarDestinos();
            return View(reserva);
        }

        reserva.Total = destino.PrecioPorPersona * reserva.Personas;
        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva de {reserva.NombreTurista} actualizada";
        return RedirectToAction(nameof(Index));
    }

    // ELIMINAR - GET muestra la confirmación con los datos de la reserva
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Destino)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    // ELIMINAR - POST hace el DELETE
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva de {reserva.NombreTurista} eliminada";
        return RedirectToAction(nameof(Index));
    }

    // Consulta los destinos para llenar la lista desplegable
    private async Task CargarDestinos()
    {
        ViewBag.Destinos = await _context.Destinos.OrderBy(d => d.Nombre).ToListAsync();
    }

    // Colombia es UTC-5: así "hoy" es el día del usuario aunque el servidor esté en otro país
    private static DateTime HoyEnColombia()
    {
        return DateTime.UtcNow.AddHours(-5).Date;
    }
}
