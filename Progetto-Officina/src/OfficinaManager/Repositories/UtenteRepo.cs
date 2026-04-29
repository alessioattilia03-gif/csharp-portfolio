using Officina.API.Models;
using Officina.API.Repositories.Interfaces;

namespace Officina.API.Repositories
{
    public class UtenteRepo : IUtenteRepo
    {
        private readonly OfficinaNewContext _context;

        public UtenteRepo(OfficinaNewContext context)
        {
            _context = context;
        }

        // cerco l'utente usando la chiave primaria id per recuperare il profilo dal db
        public Utente? GetById(int id) => _context.Utentes.Find(id); // fine ricerca per id

        // prendo la lista completa di tutti gli utenti registrati nel sistema
        public IEnumerable<Utente> GetAll() => _context.Utentes.ToList(); // fine recupero tutti gli utenti

        public Utente? GetByUsername(string username)
        {
            // questo metodo è fondamentale per il login: cerco l'utente tramite il suo nickname
            return _context.Utentes.FirstOrDefault(u => u.Username == username);
        } // fine ricerca per username

        public bool Create(Utente entity)
        {
            // aggiungo un nuovo utente (admin o meccanico) alla tabella del database
            _context.Utentes.Add(entity);
            return _context.SaveChanges() > 0;
        } // fine creazione utente

        public bool Update(Utente entity)
        {
            // aggiorno i dati del profilo e salvo le modifiche nel db
            _context.Utentes.Update(entity);
            return _context.SaveChanges() > 0;
        } // fine aggiornamento utente

        public bool Delete(int id)
        {
            // cerco l'utente per id e se esiste lo rimuovo definitivamente
            var u = _context.Utentes.Find(id);
            if (u == null) return false;

            _context.Utentes.Remove(u);
            return _context.SaveChanges() > 0;
        } // fine eliminazione utente
    }
}