using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Controllers
{
    [ApiController]
    [Route("api/interventi")]
    // bisogna essere loggati per gestire
    [Authorize]
    public class InterventoController : Controller
    {
        private readonly IInterventoService _service;

        public InterventoController(IInterventoService service)
        {
            _service = service;
        }

        [HttpGet("attivi")]
        [Authorize(Roles = "Admin,Meccanico")]
        public ActionResult<IEnumerable<InterventoDTO>> InterventiInCorso()
        {
            var risultato = _service.CercaAttivi();
            return Ok(risultato);
        } // fine metodo interventi in corso

        
        [HttpGet("veicolo/{veicoloCodicePub}")]
        [Authorize(Roles = "Admin,Meccanico")]
        public ActionResult<IEnumerable<InterventoDTO>> StoricoVeicolo(string veicoloCodicePub)
        {
            // controllo correttezza codice macchina
            if (string.IsNullOrWhiteSpace(veicoloCodicePub))
            {
                return BadRequest("Il codice pubblico del veicolo è obbligatorio.");
            }

            // filtro la lista per estrarre solo quelli con il codice pubblico del veicolo specificato
            var risultato = _service.CercaTutti().Where(x => x.VeicoloCodicePub == veicoloCodicePub);
            return Ok(risultato);
        } // fine metodo storico interventi di un veicolo specifico


        [HttpPost]
        [Authorize(Roles = "Admin,Meccanico")]
        public IActionResult InserisciIntervento(InterventoDTO interventoDTO)
        {
            // collego il lavoro alla targa scritta sulla dashboard
            if (string.IsNullOrWhiteSpace(interventoDTO.VeicoloTarga))
            {
                return BadRequest("La targa del veicolo è obbligatoria.");
            }

            var esito = _service.Inserisci(interventoDTO);

            if (esito)
            {
                return Ok("Intervento aperto con successo.");
            }

            return BadRequest("Errore: targa non trovata in anagrafica o dati non validi.");
        } // fine metodo per aprire un nuovo intervento

        [HttpPatch("{codicePub}/stato")]
        [Authorize(Roles = "Admin,Meccanico")]
        public IActionResult AggiornaStato(string codicePub, [FromBody] string nuovoStato)
        {
            var intervento = _service.CercaPerCodice(codicePub);

            if (intervento == null)
            {
                return NotFound("Intervento non trovato.");
            }

            intervento.Stato = nuovoStato;
            var esito = _service.Aggiorna(intervento);

            if (esito)
            {
                return Ok($"stato aggiornato a {nuovoStato} correttamente.");
            }

            return BadRequest("errore durante il salvataggio del nuovo stato.");
        } // fine metodo per aggiornare lo stato di un intervento (es. da "In Corso" a "Completato")

        [HttpDelete("{codicePub}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EliminaIntervento(string codicePub)
        {
            var intervento = _service.CercaPerCodice(codicePub);
            if (intervento == null) return NotFound("Intervento non trovato.");

            var esito = _service.EliminaPerCodice(codicePub);

            if (esito) return Ok("Intervento rimosso.");
            return BadRequest("Errore durante l'eliminazione.");
        } // fine metodo per eliminare un intervento, utile per correggere errori o rimuovere interventi duplicati

        [HttpGet("{codicePub}")]
        [Authorize(Roles = "Admin,Meccanico,admin,meccanico")]
        public ActionResult<InterventoDTO> GetDettaglio(string codicePub)
        {
            var intervento = _service.CercaPerCodice(codicePub);

            if (intervento == null)
            {
                return NotFound("Intervento non trovato.");
            }

            return Ok(intervento);
        } // fine metodo per ottenere i dettagli di un intervento specifico, utile per la Dashboard Admin e Meccanico

        [HttpPut("aggiorna")]
        [Authorize(Roles = "Admin,admin,Meccanico,meccanico")]
        public IActionResult AggiornaIntervento([FromBody] InterventoDTO dto)
        {
            var esito = _service.Aggiorna(dto);
            if (esito) return Ok("Aggiornato con successo");
            return BadRequest("Impossibile aggiornare l'intervento");
        } // fine metodo per aggiornare un intervento, utile per modificare descrizione o note

        [HttpGet]
        [Authorize(Roles = "Admin,admin,Meccanico,meccanico")]
        public ActionResult<IEnumerable<InterventoDTO>> CercaTutti()
        {
            var risultato = _service.CercaTutti();
            return Ok(risultato);
        } // fine metodo per cercare tutti gli interventi, utile per la Dashboard Admin e Meccanico
    }
}