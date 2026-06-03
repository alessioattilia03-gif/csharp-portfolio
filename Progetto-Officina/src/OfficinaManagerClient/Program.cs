using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Officina.Client.Components;
using Officina.Client.Services;
using Officina.Client.Services.Interfaces;
using OfficinaManagerClient.Services;

namespace Officina.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // --- SUPPORTO AUTH ---
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(); // Indica al sistema che uso i token jwt

            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<CustomAuthStateProvider>());

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();

            // Configurazione HttpClient puntata alla porta corretta dell'API (7292)
            // Nel Program.cs del CLIENT
            builder.Services.AddScoped(sp =>
            {
                // Creo un handler che ignora gli errori del certificato HTTPS locale
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:7292/")
                };
            });

            // Registro le implementazioni Client per le interfacce
            builder.Services.AddScoped<IClienteService, ClienteServiceClient>();
            builder.Services.AddScoped<IVeicoloService, VeicoloServiceClient>();
            builder.Services.AddScoped<IInterventoService, InterventoServiceClient>();
            builder.Services.AddScoped<IUtenteService, UtenteServiceClient>();
            builder.Services.AddScoped<IUtenteService, UtenteServiceClient>();

            var app = builder.Build();

            // Configuro la pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
