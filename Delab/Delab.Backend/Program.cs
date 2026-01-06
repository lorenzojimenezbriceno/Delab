using Delab.AccessData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

// builder.Services.AddTransient<SeedDB>();

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
 * Middleware para activar el Swagger y abrir el navegador
 */

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    string swaggerUrl = "https://localhost:7165/swagger"; // URL de Swagger
    Task.Run(() => OpenBrowser(swaggerUrl));
}

/*
 * Configure the HTTP request pipeline
 */

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


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
