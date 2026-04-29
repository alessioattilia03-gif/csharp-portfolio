using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Officina.API.Repositories.Interfaces;
using OffinicinaShared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Officina.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        // recupero il repository degli utenti tramite dependency injection
        private readonly IUtenteRepo _utenteRepo;

        public AuthController(IUtenteRepo utenteRepo)
        {
            _utenteRepo = utenteRepo;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // vado a cercare nel db se esiste un utente con quello username
            var utente = _utenteRepo.GetByUsername(request.Username);

            if (utente == null)
            {
                // se il database non restituisce nulla, blocco subito l'accesso
                return Unauthorized(new { message = "Utente non trovato" });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, utente.PasswordHash))
            {
                return Unauthorized(new { message = "Password errata" });
            }

            // qui definisco le impostazioni del token che devono combaciare con la configurazione del server
            var chiaveSegreta = "UnaChiaveSegretaMoltoLungaEComplessa123!";
            var issuer = "OfficinaManager"; // Corretto
            var audience = "OfficinaUsers";  // Corretto

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(chiaveSegreta);

            // preparo il "corpo" del token inserendo i dati dell'utente (claims)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // questi dati verranno letti dal client per gestire permessi e interfaccia
                    new Claim(ClaimTypes.Name, utente.Username),
                    new Claim(ClaimTypes.Role, utente.Ruolo),
                    new Claim("CodicePubblico", utente.CodicePub)
                }),
                // il token scadrà automaticamente dopo 8 ore di inattività
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = issuer,
                Audience = audience,
                // firmo il token con l'algoritmo hmac per assicurarmi che non venga alterato
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            // genero il token fisico
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // impacchetto la risposta da rimandare al frontend blazor
            var response = new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                Username = utente.Username,
                Ruolo = utente.Ruolo
            };

            return Ok(response);
        } // fine logica auth, se arrivo qui l'utente è dentro
    }
}