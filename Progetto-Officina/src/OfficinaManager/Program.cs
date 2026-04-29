using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Officina.API.Models;
using Officina.API.Repositories;
using Officina.API.Repositories.Interfaces;
using Officina.API.Services;
using Officina.API.Services.Interfaces;
using OffinicinaShared;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Officina.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- servizi base ---
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // evito che json trasformi tutto in minuscolo così i nomi restano uguali ai dto
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    // permetto al server di capire i json anche se hanno maiuscole/minuscole diverse
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                }); // fine configurazione json

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // aggiungo la protezione contro gli attacchi csrf
            builder.Services.AddAntiforgery();

            // risolvo i problemi di stato dell'autenticazione per far funzionare bene blazor
            builder.Services.AddCascadingAuthenticationState();

            // configurazione del database sql server usando la stringa nel file appsettings
            builder.Services.AddDbContext<OfficinaNewContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // fine config db

            // --- GESTIONE SICURA CHIAVE JWT ---
            // 1. Estraggo la chiave dal file di configurazione
            var jwtKey = builder.Configuration["Jwt:Key"];

            // 2. Controllo di sicurezza: blocco l'avvio se la chiave non esiste o è vuota
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Errore critico: la chiave JWT non è presente nel file appsettings.json!");
            }

            // configurazione dell'autenticazione tramite token jwt
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    // 3. Passo la variabile sicura 'jwtKey' al GetBytes
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
                };
            }); // fine config auth jwt

            // permetto al frontend di comunicare con l'api anche se girano su porte diverse
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:XXXX") // Assicurati di inserire la porta corretta del tuo Client
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            }); // fine config cors

            // --- registrazione repository (dependency injection) ---
            // qui dico al sistema quale classe usare quando un controller chiede un'interfaccia repo
            builder.Services.AddScoped<ICodPubRepo, ClienteRepo>();
            builder.Services.AddScoped<IClienteRepo, ClienteRepo>();
            builder.Services.AddScoped<IVeicoloRepo, VeicoloRepo>();
            builder.Services.AddScoped<IInterventoRepo, InterventoRepo>();
            builder.Services.AddScoped<IUtenteRepo, UtenteRepo>();
            builder.Services.AddScoped<IRepoScrittura<Cliente>, ClienteRepo>();
            builder.Services.AddScoped<IRepoLettura<Veicolo>, VeicoloRepo>();
            builder.Services.AddScoped<IRepoScrittura<Veicolo>, VeicoloRepo>();
            builder.Services.AddScoped<IRepoLettura<Intervento>, InterventoRepo>();
            builder.Services.AddScoped<IRepoScrittura<Intervento>, InterventoRepo>();
            builder.Services.AddScoped<IRepoLettura<Utente>, UtenteRepo>();
            builder.Services.AddScoped<IRepoScrittura<Utente>, UtenteRepo>();

            // --- registrazione service (dependency injection) ---
            // collego i servizi logici alle loro interfacce per farli usare dai controller
            builder.Services.AddScoped<IService<ClienteDTO>, ClienteService>();
            builder.Services.AddScoped<IVeicoloService, VeicoloService>();
            builder.Services.AddScoped<IInterventoService, InterventoService>();
            builder.Services.AddScoped<IService<UtenteDTO>, UtenteService>();

            var app = builder.Build();

            // --- pipeline di esecuzione ---
            if (app.Environment.IsDevelopment())
            {
                // attivo swagger solo in sviluppo per testare le api comodamente
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // attivo cors, autenticazione e autorizzazione nell'ordine corretto
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapControllers();

            app.Run();
        } // fine main
    } // fine classe program
}