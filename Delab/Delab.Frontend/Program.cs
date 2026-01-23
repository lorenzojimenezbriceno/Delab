using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Frontend;
using Delab.Frontend.AuthenticationProviders;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

/*
 * Se inyecta el servicio http que luego podrá comunicarse con el backend
 */

const string backUrl = "https://localhost:7165";
builder.Services.AddScoped(sp => new HttpClient
{
    // .AddSingleton(
    // BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    BaseAddress = new Uri($"{backUrl}")
});

/*
 * Inyecta el servicio http para hacer las consultas al backend
 */

builder.Services.AddScoped<IRepository, Repository>();

/*
 * Inyecta el servicio se seguridad
 */

builder.Services.AddAuthorizationCore();

/*
 * Agrega el authenticator provider
 */

//Authentication Provider
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

/*
 * Manejar el SweetAlert2 de mensajes por toda la aplicacion
 */

builder.Services.AddSweetAlert2();

/*
 * Manejar el MudBlazor por toda la aplicacion
 */

builder.Services.AddMudServices();

/*
 * Inicio de la aplicacion
 */

await builder.Build().RunAsync();
