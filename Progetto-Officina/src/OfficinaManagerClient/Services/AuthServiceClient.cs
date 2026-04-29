using Microsoft.AspNetCore.Components;
using OffinicinaShared;
using System.Net.Http.Json;

namespace Officina.Client.Services
{
    public class AuthServiceClient
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _nav;

        public AuthServiceClient(HttpClient http, NavigationManager nav)
        {
            _http = http;
            _nav = nav;
        }

        public async Task<string?> Login(LoginRequest request)
        {
            // mando le credenziali (username e password) all'api di login
            var risposta = await _http.PostAsJsonAsync("api/auth/login", request);

            // se le credenziali sono giuste il server risponde con successo
            if (risposta.IsSuccessStatusCode)
            {
                // leggo il json di risposta per estrarre il token jwt
                var loginResponse = await risposta.Content.ReadFromJsonAsync<LoginResponse>();

                // restituisco il token che servirà per tutte le altre chiamate protette
                return loginResponse?.Token;
            }

            // se il login fallisce ritorno null così la pagina di login può mostrare l'errore
            return null;
        } // fine metodo di login client
    } // fine classe authserviceclient
}