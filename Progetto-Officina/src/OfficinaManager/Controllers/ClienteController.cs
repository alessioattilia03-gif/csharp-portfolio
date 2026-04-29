using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Officina.API.Models;
using Officina.API.Repositories;
using Officina.API.Services;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Controllers
{
    [ApiController]
    [Route("api/clienti")]
    // blocco l'accesso a tutta la classe: senza un token jwt valido non si passa
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IService<ClienteDTO> _service;

        public ClienteController(IService<ClienteDTO> service)
        {
            _service = service;
        }

        // accessibile sia da Admin che da Meccanico
        [HttpGet]
        [Authorize(Roles = "Admin,admin,Meccanico,meccanico")] // Assicurati di coprire le maiuscole/minuscole
        public ActionResult<IEnumerable<ClienteDTO>> ListaClienti()
        {
            // chiamo il service per avere tutti i record dal database
            var risultati = _service.CercaTutti().ToList();

            // controllo i ruoli per capire chi sta facendo la richiesta
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");

            // logica di privacy: se non sei admin ti faccio vedere la lista ma nascondo i dati privati
            if (!isAdmin)
            {
                foreach (var c in risultati)
                {
                    c.Indirizzo = "*** Riservato ***";
                    c.Email = "*** Riservato ***";
                }
            }

            return Ok(risultati);
        } // fine metodo per recuperare la lista completa dei clienti

        // accessibile sia da Admin che da Meccanico
        [HttpGet("{varCodice}")]
        [Authorize(Roles = "Admin,admin,Meccanico,meccanico")]
        public ActionResult<ClienteDTO?> VisualizzaCliente(string varCodice)
        {
            // validazione veloce dell'input prima di interrogare il servizio
            if (string.IsNullOrWhiteSpace(varCodice))
                return BadRequest("Il codice del cliente è obbligatorio.");

            ClienteDTO? risultato = _service.CercaPerCodice(varCodice);
            if (risultato == null)
                return NotFound("Cliente non trovato.");

            // se sei un meccanico oscuro i campi sensibili per sicurezza
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("admin");
            if (!isAdmin)
            {
                risultato.Indirizzo = "*** Riservato ***";
                risultato.Email = "*** Riservato ***";
            }

            return Ok(risultato);
        } // fine recupero del dettaglio di un singolo cliente tramite il suo codice pubblico

        // solo l'Admin può inserire nuovi clienti
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult InserisciCliente(ClienteDTO clienteDTO)
        {
            // controllo che i dati minimi siano presenti nel corpo della richiesta
            if (string.IsNullOrWhiteSpace(clienteDTO.Nome) || string.IsNullOrWhiteSpace(clienteDTO.Cognome))
                return BadRequest("Nome e Cognome sono obbligatori.");

            // se il service restituisce true, l'insert nel db è andata a buon fine
            if (_service.Inserisci(clienteDTO))
                return Ok("Cliente inserito correttamente.");

            return BadRequest("Errore durante l'inserimento.");
        } // fine logica inserimento cliente

        // solo l'Admin può aggiornare i dati
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult AggiornaCliente(ClienteDTO clienteDTO)
        {
            // per aggiornare devo avere il codice, altrimenti non so quale riga modificare
            if (string.IsNullOrWhiteSpace(clienteDTO.Codice))
                return BadRequest("Il codice cliente è necessario per l'aggiornamento.");

            if (_service.Aggiorna(clienteDTO))
                return Ok("Cliente aggiornato con successo.");

            return NotFound("Cliente non trovato o errore nell'aggiornamento.");
        } // fine aggiornamento dati cliente

        // solo l'Admin può eliminare 
        [HttpDelete("{codicePub}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EliminaCliente(string codicePub)
        {
            if (string.IsNullOrWhiteSpace(codicePub))
                return BadRequest("Codice non valido.");

            // faccio un cast per usare il metodo specifico del service concreto
            var serviceConcreto = _service as ClienteService;

            if (serviceConcreto != null && serviceConcreto.EliminaPerCodice(codicePub))
                return Ok($"Cliente rimosso con successo.");

            return NotFound("Cliente non trovato o errore nell'eliminazione.");
        } // fine eliminazione cliente
    }
}

