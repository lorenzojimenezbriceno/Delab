using Delab.AccessData.Data;
using Delab.Helpers;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

const string backUrl = "https://localhost:7165";
const string frontUrl = "https://localhost:7056";

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
 * Authenticator token provider
 * Para realizar logueo de los usuarios
 */

builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    // Agregamos Validar Correo para dar de alta al Usuario

    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;

    // Sistema para bloquear por 5 minutos al usuario por intento fallido

    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // TODO: Cambiar Tiempo de Bloqueo a Usuarios
    cfg.Lockout.AllowedForNewUsers = true;

})
    .AddDefaultTokenProviders()  // Complemento Validar Correo
    .AddEntityFrameworkStores<DataContext>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

/*
 * Instalar servicio para sembrar datos en la base de datos
 */

builder.Services
    //.AddScoped
    //.AddSingleton
    .AddTransient<SeedDB>();

/*
 * Instalar servicio para salvar archivos en Azure Blob Storage
 */

builder.Services.AddScoped<IFileStorage, FileStorage>();

/*
 * Agregar CORS
 */

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
            // Agregar el dominio de la aplicacion Blazor
            .WithOrigins($"{frontUrl}") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            // Agregar dos headers personalizados para la paginacion
            .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
    });
});

/*
 * 
 * 
 */

var app = builder.Build();

/*
 * Llamar el Servicio de CORS
 */

app.UseCors("AllowSpecificOrigin");

/*
 * Configuración para servir archivos estáticos desde la carpeta Images, directorio dentro de wwwroot
 */

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider
    (
        Path.Combine(builder.Environment.WebRootPath, "Images")
    ),
    RequestPath = "/Images"
});

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
    OpenBrowser($"{backUrl}/swagger"); // URL de Swagger
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
