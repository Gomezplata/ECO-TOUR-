using EcoTour.Data;
using EcoTour.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoTour.Controllers;

public class DestinosController : Controller
{
    private readonly EcoTourContext _context;

    // ASP.NET entrega el DbContext automáticamente (inyección de dependencias)
    public DestinosController(EcoTourContext context)
    {
        _context = context;
    }

    // CONSULTAR - GET /Destinos (página principal)
    public async Task<IActionResult> Index()
    {
        // Include trae también las reservas de cada destino (JOIN)
        var destinos = await _context.Destinos
            .Include(d => d.Reservas)
            .OrderBy(d => d.Nombre)
            .ToListAsync();

        return View(destinos);
    }

    // CREAR - GET muestra el formulario vacío
    [HttpGet]
    public IActionResult Crear()
    {
        return View(new Destino());
    }

    // CREAR - POST recibe el formulario y hace el INSERT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind("Nombre,Descripcion,PrecioPorPersona")] Destino destino)
    {
        if (!ModelState.IsValid)
        {
            return View(destino);
        }

        _context.Destinos.Add(destino);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Destino \"{destino.Nombre}\" creado";
        return RedirectToAction(nameof(Index));
    }

    // MODIFICAR - GET busca el destino por su Id y lo muestra en el formulario
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var destino = await _context.Destinos.FindAsync(id);
        if (destino == null)
        {
            return NotFound();
        }

        return View(destino);
    }

    // MODIFICAR - POST hace el UPDATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, [Bind("Id,Nombre,Descripcion,PrecioPorPersona")] Destino destino)
    {
        if (id != destino.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(destino);
        }

        _context.Destinos.Update(destino);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Destino \"{destino.Nombre}\" actualizado";
        return RedirectToAction(nameof(Index));
    }

    // ELIMINAR - GET muestra la confirmación
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        var destino = await _context.Destinos
            .Include(d => d.Reservas)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (destino == null)
        {
            return NotFound();
        }

        return View(destino);
    }

    // ELIMINAR - POST hace el DELETE
    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarEliminar(int id)
    {
        var destino = await _context.Destinos
            .Include(d => d.Reservas)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (destino == null)
        {
            return NotFound();
        }

        // La clave foránea no deja borrar un destino con reservas
        if (destino.Reservas.Count > 0)
        {
            TempData["Error"] = $"No se puede eliminar \"{destino.Nombre}\" porque tiene reservas";
            return RedirectToAction(nameof(Index));
        }

        _context.Destinos.Remove(destino);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Destino \"{destino.Nombre}\" eliminado";
        return RedirectToAction(nameof(Index));
    }
}
