using EcoTour.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(opciones =>
{
    // Mensajes en español cuando un campo numérico llega vacío o con texto
    opciones.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Este campo es obligatorio");
    opciones.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((valor, campo) => "El valor no es válido");
});

// Registra el DbContext y lo conecta con SQL Server usando la cadena de appsettings.json
builder.Services.AddDbContext<EcoTourContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("EcoTourConnection")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Ruta por defecto: /Controlador/Accion/id  -> la página principal es Destinos/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Destinos}/{action=Index}/{id?}");

app.Run();
