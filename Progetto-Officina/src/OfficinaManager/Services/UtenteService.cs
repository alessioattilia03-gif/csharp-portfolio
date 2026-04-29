using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Services
{
    public class UtenteService : IService<UtenteDTO>
    {
        private readonly IRepoLettura<Utente> _repoLettura;
        private readonly IRepoScrittura<Utente> _repoScrittura;

        public UtenteService(IRepoLettura<Utente> repoLettura, IRepoScrittura<Utente> repoScrittura)
        {
            _repoLettura = repoLettura;
            _repoScrittura = repoScrittura;
        }

        public IEnumerable<UtenteDTO> CercaTutti()
        {
            // prendo tutti gli utenti dal database e uso select per mappare i dati nei dto da mandare al client
            return _repoLettura.GetAll().Select(u => new UtenteDTO
            {
                CodicePub = u.CodicePub,
                Username = u.Username,
                Ruolo = u.Ruolo,
                Email = u.Email,
                Telefono = u.Telefono
            }).ToList();
        } // fine recupero lista completa utenti

        public UtenteDTO? CercaPerCodice(string codice)
        {
            // cerco un utente specifico filtrando per il suo codice pubblico univoco
            var u = _repoLettura.GetAll().FirstOrDefault(x => x.CodicePub == codice);
            if (u == null) return null;

            // se lo trovo trasformo l'oggetto del db in un dto per nascondere i campi sensibili come la password
            return new UtenteDTO
            {
                CodicePub = u.CodicePub,
                Username = u.Username,
                Ruolo = u.Ruolo,
                Email = u.Email,
                Telefono = u.Telefono
            };
        } // fine ricerca singola per codice pubblico

        public bool Inserisci(UtenteDTO entity)
        {
            // creo un nuovo oggetto utente assegnando un guid come codice pubblico e una password di default se manca
            var nuovo = new Utente
            {
                CodicePub = Guid.NewGuid().ToString().ToUpper(),
                Username = entity.Username,
                Ruolo = entity.Ruolo,
                PasswordHash = entity.Password ?? "PasswordTemporanea123!",
                Telefono = entity.Telefono,
                Email = entity.Email
            };
            return _repoScrittura.Create(nuovo);
        } // fine creazione nuovo utente

        public bool Aggiorna(UtenteDTO entity)
        {
            // recupero l'utente esistente dal db per sovrascrivere solo i campi che sono stati modificati nel form
            var esistente = _repoLettura.GetAll().FirstOrDefault(x => x.CodicePub == entity.CodicePub);
            if (esistente == null) return false;

            esistente.Username = entity.Username;
            esistente.Ruolo = entity.Ruolo;
            esistente.Email = entity.Email;
            esistente.Telefono = entity.Telefono;

            // se nel form è stata scritta una nuova password allora aggiorno anche il campo passwordhash
            if (!string.IsNullOrWhiteSpace(entity.Password)) esistente.PasswordHash = entity.Password;

            return _repoScrittura.Update(esistente);
        } // fine aggiornamento dati utente

        public bool EliminaPerCodice(string codice)
        {
            // trovo l'utente dal codice pubblico per recuperare l'id interno necessario alla cancellazione fisica
            var u = _repoLettura.GetAll().FirstOrDefault(x => x.CodicePub == codice);
            return u != null && _repoScrittura.Delete(u.UtenteId);
        } // fine eliminazione sicura per codice pubblico

        public bool Elimina(int id) => _repoScrittura.Delete(id); // fine eliminazione per id interno
    }
}