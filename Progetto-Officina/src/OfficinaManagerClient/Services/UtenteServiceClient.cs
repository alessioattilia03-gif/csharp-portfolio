using OffinicinaShared;
using Officina.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace OfficinaManagerClient.Services
{
    public class UtenteServiceClient : IUtenteService
    {
        private readonly HttpClient _http;
        public UtenteServiceClient(HttpClient http) => _http = http;

        public async Task<IEnumerable<UtenteDTO>> OttieniTutti() =>
            // scarico la lista di tutto il personale (admin e meccanici) registrato nel sistema
            await _http.GetFromJsonAsync<IEnumerable<UtenteDTO>>("api/utenti") ?? new List<UtenteDTO>();
        // fine recupero lista utenti

        public async Task<bool> Registra(UtenteDTO dto) =>
            // mando i dati del nuovo utente all'api per salvarlo nel database
            (await _http.PostAsJsonAsync("api/utenti", dto)).IsSuccessStatusCode;
        // fine registrazione nuovo utente

        public async Task<bool> Aggiorna(UtenteDTO dto) =>
            // invio le modifiche al profilo utente tramite il metodo put
            (await _http.PutAsJsonAsync("api/utenti", dto)).IsSuccessStatusCode;
        // fine aggiornamento dati utente

        public async Task<bool> Elimina(string codicePub) =>
            // chiamo la delete passando il codice pubblico per rimuovere l'utente dal db
            (await _http.DeleteAsync($"api/utenti/{codicePub}")).IsSuccessStatusCode;
        // fine eliminazione utente
    } // fine classe utenteserviceclient
}