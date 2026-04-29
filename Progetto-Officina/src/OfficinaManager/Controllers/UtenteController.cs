using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Controllers
{
    [ApiController]
    [Route("api/utenti")]
    //solo chi ha il ruolo di amministratore può gestire gli account
    [Authorize(Roles = "Admin,admin")] // Solo gli admin gestiscono il personale
    public class UtenteController : ControllerBase
    {
        private readonly IService<UtenteDTO> _service;

        public UtenteController(IService<UtenteDTO> service) => _service = service;

        // recupero la lista di tutto il personale registrato nel sistema
        [HttpGet]
        public ActionResult<IEnumerable<UtenteDTO>> Get() => Ok(_service.CercaTutti());


        [HttpPost]
        public IActionResult Post(UtenteDTO dto)
        {
            try
            {
                // provo a inserire i dati passando dal servizio
                var successo = _service.Inserisci(dto);
                // se il db accetta i dati restituisco 200, altrimenti un errore generico
                return successo ? Ok() : BadRequest("Il database ha rifiutato l'inserimento.");
            }
            catch (Exception ex)
            {
                // recupero il messaggio più profondo 
                var erroreReale = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                // loggo l'errore nella console di Visual Studio
                Console.WriteLine($"--- ERRORE CRITICO DB: {erroreReale}");

                // restituisco l'errore al client così lo leggo in F12
                return StatusCode(500, erroreReale);
            }
        } // fine metodo per creare un nuovo account (meccanico o admin)

        [HttpPut]
        public IActionResult Put(UtenteDTO dto) => _service.Aggiorna(dto) ? Ok() : NotFound();

        [HttpDelete("{codicePub}")]
        public IActionResult Delete(string codicePub)
        {
            var srv = _service as Officina.API.Services.UtenteService;
            return srv != null && srv.EliminaPerCodice(codicePub) ? Ok() : NotFound();
        } // fine aggiornamento o eliminazione di un account (meccanico o admin)
    }
}