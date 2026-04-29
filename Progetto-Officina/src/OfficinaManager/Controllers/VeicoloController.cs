using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Controllers
{
    [ApiController]
    [Route("api/veicoli")]
    // accesso consentito solo agli utenti loggati con token jwt
    [Authorize]
    public class VeicoloController : Controller
    {
        private readonly IVeicoloService _service; 

        public VeicoloController(IVeicoloService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Meccanico")]
        public ActionResult<IEnumerable<VeicoloDTO>> ListaVeicoli()
        {
            return Ok(_service.CercaTutti());
        } // fine metodo per recuperare tutti i veicoli

        [HttpGet("{targa}")]
        [Authorize(Roles = "Admin,Meccanico")]
        public ActionResult<VeicoloDTO?> VisualizzaVeicolo(string targa)
        {
            // controllo di sicurezza
            if (string.IsNullOrWhiteSpace(targa))
                return BadRequest("La targa è obbligatoria.");

            var risultato = _service.CercaPerTarga(targa);
            if (risultato == null)
                return NotFound("Veicolo non trovato.");

            return Ok(risultato);
        } // fine metodo per recuperare un veicolo specifico tramite targa

        [HttpGet("cliente/{clienteId}")]
        [Authorize(Roles = "Admin,Meccanico")]
        public ActionResult<IEnumerable<VeicoloDTO>> VeicoliPerCliente(int clienteId)
        {
            return Ok(_service.CercaPerCliente(clienteId));
        } // fine metodo per recuperare tutti i veicoli associati a un cliente specifico

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult InserisciVeicolo(VeicoloDTO veicoloDTO)
        {
            // la targa è il dato fondamentale per l'anagrafica auto
            if (string.IsNullOrWhiteSpace(veicoloDTO.Targa))
                return BadRequest("La targa è obbligatoria.");

            if (_service.Inserisci(veicoloDTO))
                return Ok("Veicolo registrato correttamente.");

            return BadRequest("Errore durante la registrazione del veicolo.");
        } // fine metodo per inserire un nuovo veicolo

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult AggiornaVeicolo(VeicoloDTO veicoloDTO)
        {
            if (_service.Aggiorna(veicoloDTO))
                return Ok("Dati veicolo aggiornati.");

            return NotFound("Veicolo non trovato.");
        } // fine metodo per aggiornare i dati di un veicolo esistente
    }
}
