using Microsoft.AspNetCore.Components.Authorization;
using Officina.Client.Services;
using Officina.Client.Services.Interfaces;
using OffinicinaShared;
using System.Text.Json;

namespace OfficinaManagerClient.Services
{
    public class InterventoServiceClient : IInterventoService
    {
        private readonly HttpClient _http;
        private readonly CustomAuthStateProvider _authStateProvider;

        // ricevo il provider nel costruttore per poter recuperare il token salvato al login
        public InterventoServiceClient(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        }

        // questo metodo serve a "timbrare" ogni richiesta con il token jwt prima di mandarla al server
        private void ApplicaToken(HttpRequestMessage request)
        {
            if (_authStateProvider is CustomAuthStateProvider customProvider &&
                !string.IsNullOrEmpty(customProvider.Token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customProvider.Token);
            }
        } // fine applicazione token

        public async Task<IEnumerable<InterventoDTO>> CercaAttivi()
        {
            // creo la richiesta per i lavori in corso e ci attacco il token
            var request = new HttpRequestMessage(HttpMethod.Get, "api/interventi/attivi");
            ApplicaToken(request);

            try
            {
                var response = await _http.SendAsync(request);
                var rawJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // leggo il json stando attento a ignorare le differenze tra maiuscole e minuscole
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<IEnumerable<InterventoDTO>>(rawJson, options) ?? [];
                }
                return [];
            }
            catch (Exception)
            {
                return [];
            }
        } // fine recupero interventi aperti

        public async Task<IEnumerable<InterventoDTO>> CercaTutti()
        {
            // scarico lo storico completo di tutti gli interventi fatti dall'officina
            var request = new HttpRequestMessage(HttpMethod.Get, "api/interventi");
            ApplicaToken(request);

            var response = await _http.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<InterventoDTO>>() ?? [];
            }
            return [];
        } // fine recupero storico totale

        public async Task<bool> Inserisci(InterventoDTO dto)
        {
            // creo un post e serializzo il nuovo intervento nel corpo della richiesta
            var request = new HttpRequestMessage(HttpMethod.Post, "api/interventi");
            request.Content = JsonContent.Create(dto);

            // firmo la richiesta col token altrimenti il server blocca l'inserimento
            ApplicaToken(request);
            try
            {
                var invio = await _http.SendAsync(request);
                return invio.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[debug] errore durante l'inserimento: {ex.Message}");
                return false;
            }
        } // fine invio nuovo intervento

        public async Task<InterventoDTO?> CercaPerCodice(string codicePub)
        {
            // recupero il dettaglio di un singolo lavoro usando il suo codice univoco
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/interventi/{codicePub}");
            ApplicaToken(request);

            try
            {
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var rawJson = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<InterventoDTO>(rawJson, options);
                }

                Console.WriteLine($"errore api dettaglio: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"eccezione client dettaglio: {ex.Message}");
                return null;
            }
        } // fine ricerca singola per codice

        public async Task<bool> Aggiorna(InterventoDTO dto)
        {
            // preparo la modifica (put) e ci attacco il token per l'autorizzazione
            var request = new HttpRequestMessage(HttpMethod.Put, "api/interventi/aggiorna");
            request.Content = JsonContent.Create(dto);
            ApplicaToken(request);

            try
            {
                var invio = await _http.SendAsync(request);
                return invio.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        } // fine salvataggio modifiche intervento

        public async Task<bool> Elimina(string codicePub)
        {
            // uso il metodo delete puntando al codice pubblico per cancellare il lavoro dal db
            var response = await _http.DeleteAsync($"api/interventi/{codicePub}");
            return response.IsSuccessStatusCode;
        } // fine eliminazione intervento client
    } // fine classe intereventoserviceclient
}