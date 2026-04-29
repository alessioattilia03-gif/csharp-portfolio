using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Officina.API.Repositories
{
    public class ClienteRepo : ICodPubRepo, IClienteRepo
    {
        private readonly OfficinaNewContext _context;

        public ClienteRepo(OfficinaNewContext context)
        {
            _context = context;
        }

        public Cliente? GetByCodicePub(string codicePub)
        {
            // uso include per caricare anche i veicoli del cliente senza fare altre chiamate al db
            var cliente = _context.Clientes
                                  .Include(c => c.Veicolos)
                                  .FirstOrDefault(c => c.CodicePub == codicePub);

            return cliente;
        } // fine recupero per codice pubblico

        public Cliente? GetByCodice(string cod)
        {
            // richiamo il metodo sopra per non riscrivere la stessa logica di ricerca
            var risultato = GetByCodicePub(cod);

            return risultato;
        } // fine metodo alias per codice

        public IEnumerable<Cliente> GetAll()
        {
            // prendo tutti i clienti dal db e mi porto dietro anche la lista delle loro auto
            var lista = _context.Clientes
                                .Include(c => c.Veicolos)
                                .ToList();

            return lista;
        } // fine recupero lista completa

        public Cliente? GetById(int id)
        {
            // cerco il cliente usando la chiave primaria id (quella numerica)
            var cliente = _context.Clientes.Find(id);

            return cliente;
        } // fine ricerca per id interno

        public bool Create(Cliente entity)
        {
            // aggiungo il nuovo oggetto alla tabella e salvo le modifiche sul db
            _context.Clientes.Add(entity);
            var righeEffettuate = _context.SaveChanges();

            // se ha modificato almeno una riga vuol dire che l'inserimento è andato bene
            return righeEffettuate > 0;
        } // fine inserimento nuovo cliente

        public bool Update(Cliente entity)
        {
            // dico a entity framework di aggiornare i dati di questo cliente specifico
            _context.Clientes.Update(entity);
            var righeEffettuate = _context.SaveChanges();

            return righeEffettuate > 0;
        } // fine aggiornamento dati

        public bool Delete(int id)
        {
            // cerco prima se il cliente esiste davvero prima di provare a cancellarlo
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
            {
                // se non lo trovo ritorno false così il controller sa che non c'era nulla da eliminare
                return false;
            }

            _context.Clientes.Remove(cliente);
            var righeEffettuate = _context.SaveChanges();

            return righeEffettuate > 0;
        } // fine eliminazione cliente
    }
}