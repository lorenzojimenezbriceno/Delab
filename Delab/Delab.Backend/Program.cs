using Delab.AccessData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/*
 * Add services to the container y,
 * Para evitar las referencias ciclicas en la base de datos
 */

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

/*
 * Programacion de Swagger 
 */

builder.Services.AddOpenApi(); // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

/*
 * Conexion a la base de datos en otro proyecto (assembly)
 */

builder.Services.AddDbContext<DataContext>(x => 
    x.UseSqlServer("name=DefaultConnection", options => options.MigrationsAssembly("Delab.Backend")));

/*
 * Instalar servicio para sembrar datos en la base de datos
 */

builder.Services
    //.AddScoped
    //.AddSingleton
    .AddTransient<SeedDB>();

/*
 * Agregar CORS
 */

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
            .WithOrigins("https://localhost:7098") // Dominio de la aplicacion Blazor
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
    });
});

/*
 * 
 * 
 */

var app = builder.Build();

/*
 * Método para cargar los datos de prueba a la base de datos si está vacía
 */

SeedData(app);

/*
 * Middleware para activar el Swagger y abrir el navegador
 */

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    OpenBrowser("https://localhost:7165/swagger"); // URL de Swagger
}

/*
 * Configure the HTTP request pipeline
 */

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

/*
 * Método para cargar los datos de prueba a la base de datos si está vacía
 * Es una forma de ejecutar un servicio en tiempo de ejecución
 */

void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDB? service = scope.ServiceProvider.GetService<SeedDB>();
        service!.SeedAsync().Wait();
    }
}

/*
 * Método para abrir el navegador para el Swagger
 */

static void OpenBrowser(string url)
{
    try
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        };
        System.Diagnostics.Process.Start(psi);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al abrir el navegador: {ex.Message}");
    }
}
