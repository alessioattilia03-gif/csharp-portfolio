using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Services
{
    public class ClienteService : IService<ClienteDTO>
    {
        // preparo i repository necessari per leggere e scrivere i dati
        private readonly ICodPubRepo _repoLettura;
        private readonly IRepoScrittura<Cliente> _repoScrittura;
        private readonly IVeicoloRepo _veicoloRepo;

        public ClienteService(ICodPubRepo repoLettura, IRepoScrittura<Cliente> repoScrittura, IVeicoloRepo veicoloRepo)
        {
            _repoLettura = repoLettura;
            _repoScrittura = repoScrittura;
            _veicoloRepo = veicoloRepo;
        }

        public ClienteDTO? CercaPerCodice(string codice)
        {
            // vado a prendere il cliente dal database usando il suo codice pubblico
            var clienteDb = _repoLettura.GetByCodice(codice);
            if (clienteDb == null) return null;

            // travaso i dati dall'oggetto del database al dto per il frontend
            var dto = new ClienteDTO
            {
                Codice = clienteDb.CodicePub,
                Nome = clienteDb.Nome,
                Cognome = clienteDb.Cognome,
                Telefono = clienteDb.Telefono,
                Email = clienteDb.Email,
                Indirizzo = clienteDb.Indirizzo,
                // recupero anche le targhe dei veicoli associati a questo cliente
                Veicoli = clienteDb.Veicolos != null ? clienteDb.Veicolos.Select(v => v.Targa).ToList() : new List<string>()
            };

            return dto;
        } // fine ricerca singola e mapping dto

        public IEnumerable<ClienteDTO> CercaTutti()
        {
            // recupero la lista completa dei clienti dal database 
            IEnumerable<Cliente> listaDb = _repoLettura.GetAll();

            List<ClienteDTO> risultato = new List<ClienteDTO>();

            // ciclo su ogni riga del db per trasformarla in un dto leggibile dal client
            foreach (var c in listaDb)
            {
                var dto = new ClienteDTO
                {
                    Codice = c.CodicePub,
                    Nome = c.Nome,
                    Cognome = c.Cognome,
                    Telefono = c.Telefono,
                    Email = c.Email,
                    Indirizzo = c.Indirizzo,
                    Veicoli = c.Veicolos != null ? c.Veicolos.Select(v => v.Targa).ToList() : new List<string>()
                };
                risultato.Add(dto);
            }

            return risultato;
        } // fine recupero lista e mapping massivo

        public bool Aggiorna(ClienteDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Codice))
            {
                return false;
            }

            // cerco il record originale sul db prima di provare a sovrascrivere i dati
            var clienteEsistente = _repoLettura.GetByCodice(entity.Codice);

            if (clienteEsistente == null)
            {
                return false;
            }

            // aggiorno solo i campi necessari senza toccare le chiavi primarie o i codici pub
            clienteEsistente.Nome = entity.Nome;
            clienteEsistente.Cognome = entity.Cognome;
            clienteEsistente.Indirizzo = entity.Indirizzo;
            clienteEsistente.Telefono = entity.Telefono;
            clienteEsistente.Email = entity.Email;

            // mando l'entità aggiornata al repo di scrittura per fare l'update fisico sul db
            return _repoScrittura.Update(clienteEsistente);
        } // fine logica di aggiornamento

        public bool Elimina(int id)
        {
            // metodo standard per eliminare tramite id numerico
            return _repoScrittura.Delete(id);
        } // fine eliminazione per id

        public bool EliminaPerCodice(string codice)
        {
            // trovo il cliente dal codice pubblico per recuperare il suo id interno e poi lo cancello
            var cliente = _repoLettura.GetByCodice(codice);
            if (cliente == null) return false;

            return _repoScrittura.Delete(cliente.ClienteId);
        } // fine eliminazione sicura per codice pubblico

        public bool Inserisci(ClienteDTO entity)
        {
            // creo l'oggetto cliente e se non ha un codice ne genero uno nuovo univoco
            Cliente cliente = new Cliente()
            {
                CodicePub = entity.Codice ?? Guid.NewGuid().ToString().ToUpper(),
                Nome = entity.Nome,
                Cognome = entity.Cognome,
                Indirizzo = entity.Indirizzo,
                Telefono = entity.Telefono,
                Email = entity.Email
            };

            // salvo il cliente e lascio che entity framework si recuperi l'id generato dal database
            bool successoCliente = _repoScrittura.Create(cliente);

            // se il cliente è stato creato e l'utente ha inserito una targa, registro subito anche l'auto
            if (successoCliente && !string.IsNullOrWhiteSpace(entity.NuovaTarga))
            {
                var nuovoVeicolo = new Veicolo
                {
                    CodicePub = Guid.NewGuid().ToString().ToUpper(),
                    Targa = entity.NuovaTarga.Trim().ToUpper(),
                    Marca = "Da definire",
                    Modello = "Da definire",
                    ClienteId = cliente.ClienteId // qui collego l'auto al cliente appena nato
                };
                _veicoloRepo.Create(nuovoVeicolo);
            }

            return successoCliente;
        } // fine inserimento cliente e veicolo iniziale
    }
}