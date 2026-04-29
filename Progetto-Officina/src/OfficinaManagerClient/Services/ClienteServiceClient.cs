using OffinicinaShared;
using Officina.Client.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OfficinaManagerClient.Services
{
    public class ClienteServiceClient : IClienteService
    {
        private readonly HttpClient _http;

        public ClienteServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<ClienteDTO>> CercaTutti()
        {
            // faccio una chiamata get all'api per scaricare la lista di tutti i clienti
            var risposta = await _http.GetFromJsonAsync<IEnumerable<ClienteDTO>>("api/clienti");
            return risposta ?? new List<ClienteDTO>();
        } // fine recupero lista clienti

        public async Task<ClienteDTO?> CercaPerCodice(string codicePub)
        {
            // scarico i dati di un singolo cliente usando il suo codice pubblico nell'url
            return await _http.GetFromJsonAsync<ClienteDTO>($"api/clienti/{codicePub}");
        } // fine ricerca cliente per codice

        public async Task<bool> Inserisci(ClienteDTO dto)
        {
            // mando un post all'api con i dati del nuovo cliente da salvare nel db
            var risposta = await _http.PostAsJsonAsync("api/clienti", dto);
            return risposta.IsSuccessStatusCode;
        } // fine creazione nuovo cliente

        public async Task<bool> Aggiorna(ClienteDTO dto)
        {
            // invio i dati aggiornati tramite il metodo put per modificare un cliente esistente
            var response = await _http.PutAsJsonAsync("api/clienti", dto);
            return response.IsSuccessStatusCode;
        } // fine aggiornamento cliente

        public async Task<bool> Elimina(string codicePub)
        {
            // chiamo il metodo delete passando il codice pubblico per cancellare il cliente dal sistema
            var response = await _http.DeleteAsync($"api/clienti/{codicePub}");
            return response.IsSuccessStatusCode;
        } // fine eliminazione cliente
    } // fine classe clienteserviceclient
}