using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Officina.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        // preparo un utente anonimo (non loggato) di base
        private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private ClaimsPrincipal _currentUser;

        // qui salvo il token jwt così posso riusarlo per le chiamate api
        public string? Token { get; private set; }

        public CustomAuthStateProvider() => _currentUser = _anonymous;

        // questo metodo dice a blazor se l'utente attuale è loggato oppure no
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(_currentUser));

        public void MarkUserAsAuthenticated(string token)
        {
            // salvo il token e inizio a smontarlo per leggere cosa c'è dentro
            Token = token;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // leggo i permessi (claims) dal token e specifico quali campi indicano il nome e il ruolo
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt", "name", "role");
            _currentUser = new ClaimsPrincipal(identity);

            // avviso tutta l'app blazor che l'utente è entrato, così cambiano i menu e le pagine
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        } // fine login utente e gestione claims

        public void MarkUserAsLoggedOut()
        {
            // cancello il token e riporto l'utente allo stato anonimo
            Token = null;
            _currentUser = _anonymous;

            // avviso l'app che l'utente è uscito così viene rispedito alla pagina di login
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        } // fine logout utente
    } // fine classe customauthstateprovider
}