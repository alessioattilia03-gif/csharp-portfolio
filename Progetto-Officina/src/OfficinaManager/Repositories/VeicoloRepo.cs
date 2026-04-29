using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Officina.API.Repositories
{
    public class VeicoloRepo : IVeicoloRepo
    {
        private readonly OfficinaNewContext _context;

        public VeicoloRepo(OfficinaNewContext context)
        {
            _context = context;
        }

        public bool Create(Veicolo entity)
        {
            // aggiungo la nuova auto al database e salvo
            _context.Veicolos.Add(entity);
            return _context.SaveChanges() > 0;
        } // fine creazione veicolo

        public bool Update(Veicolo entity)
        {
            // modifico i dati di un veicolo già esistente nel db
            _context.Veicolos.Update(entity);
            return _context.SaveChanges() > 0;
        } // fine aggiornamento veicolo

        public bool Delete(int id)
        {
            // cerco il veicolo tramite l'id e se lo trovo lo cancello
            var entity = _context.Veicolos.Find(id);
            if (entity == null) return false;
            _context.Veicolos.Remove(entity);
            return _context.SaveChanges() > 0;
        } // fine eliminazione veicolo

        public Veicolo? GetByCodicePub(string codicePub)
        {
            // recupero l'auto tramite il codice pubblico e includo anche i dati del proprietario
            return _context.Veicolos
                .Include(v => v.Cliente)
                .FirstOrDefault(v => v.CodicePub == codicePub);
        } // fine ricerca per codice pubblico

        public IEnumerable<Veicolo> GetAll() => _context.Veicolos.ToList(); // fine recupero lista completa

        public Veicolo? GetById(int id) => _context.Veicolos.Find(id); // fine ricerca per id interno

        // implementazione del metodo specifico per cercare un'auto dalla targa
        public Veicolo? GetByTarga(string targa)
        {
            return _context.Veicolos.FirstOrDefault(v => v.Targa == targa);
        } // fine ricerca per targa

        public IEnumerable<Veicolo> GetByCliente(int clienteId)
        {
            // filtro tutti i veicoli per trovare quelli che appartengono a un determinato cliente
            return _context.Veicolos.Where(v => v.ClienteId == clienteId).ToList();
        } // fine ricerca veicoli per cliente
    }
}