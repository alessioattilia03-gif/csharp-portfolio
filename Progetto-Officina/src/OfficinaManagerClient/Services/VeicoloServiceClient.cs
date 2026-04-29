using OffinicinaShared;
using Officina.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace OfficinaManagerClient.Services
{
    public class VeicoloServiceClient : IVeicoloService
    {
        private readonly HttpClient _http;

        public VeicoloServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<VeicoloDTO>> CercaTutti()
        {
            // faccio una richiesta get per scaricare la lista completa di tutte le auto nel db
            var lista = await _http.GetFromJsonAsync<IEnumerable<VeicoloDTO>>("api/veicoli");
            return lista ?? new List<VeicoloDTO>();
        } // fine recupero lista veicoli

        public async Task<VeicoloDTO?> CercaPerTarga(string targa)
        {
            // cerco i dati di un veicolo specifico passando la targa nell'indirizzo url
            var veicolo = await _http.GetFromJsonAsync<VeicoloDTO>($"api/veicoli/targa/{targa}");
            return veicolo;
        } // fine ricerca per targa

        public async Task<bool> Inserisci(VeicoloDTO dto)
        {
            // mando i dati della nuova macchina all'api per registrarla nell'anagrafica
            var invio = await _http.PostAsJsonAsync("api/veicoli", dto);
            return invio.IsSuccessStatusCode;
        } // fine inserimento veicolo
    } // fine classe veicoloserviceclient
}