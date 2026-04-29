using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Officina.API.Repositories
{
    public class InterventoRepo : IInterventoRepo
    {
        private readonly OfficinaNewContext _context;

        public InterventoRepo(OfficinaNewContext context)
        {
            _context = context;
        }

        public Intervento? GetByCodicePub(string codicePub)
        {
            // carico anche i dati del veicolo così il service può leggere il suo codice pubblico
            var intervento = _context.Interventos
                                     .Include(i => i.Veicolo)
                                     .FirstOrDefault(i => i.CodicePub == codicePub);

            return intervento;
        } // fine ricerca per codice pubblico

        public IEnumerable<Intervento> GetAttivi()
        {
            // prendo solo i lavori non finiti e uso asnotracking per far andare la query più veloce
            return _context.Interventos
                           .Include(i => i.Veicolo)
                           .Where(i => i.Stato != "completato")
                           .AsNoTracking()
                           .ToList();
        } // fine recupero interventi in corso

        public IEnumerable<Intervento> GetByVeicolo(int veicoloId)
        {
            // cerco tutti i lavori collegati a una specifica macchina usando l'id del veicolo
            var lista = _context.Interventos
                                .Where(i => i.VeicoloId == veicoloId)
                                .ToList();

            return lista;
        } // fine ricerca storico per veicolo

        public IEnumerable<Intervento> GetAll()
        {
            // prendo proprio tutto lo storico degli interventi caricando anche le info dell'auto
            var query = _context.Interventos
                                .Include(i => i.Veicolo);

            var result = query.ToList();

            return result;
        } // fine recupero lista totale

        public Intervento? GetById(int id)
        {
            // ricerca classica usando la chiave primaria id
            var intervento = _context.Interventos.Find(id);

            return intervento;
        } // fine ricerca per id interno

        public bool Create(Intervento entity)
        {
            // aggiungo l'intervento al db e controllo se il salvataggio ha scritto davvero i dati
            _context.Interventos.Add(entity);
            var salvataggio = _context.SaveChanges();

            return salvataggio > 0;
        } // fine creazione nuovo intervento

        public bool Update(Intervento entity)
        {
            // aggiorno le info dell'intervento (tipo se il meccanico aggiunge note o cambia lo stato)
            _context.Interventos.Update(entity);
            var salvataggio = _context.SaveChanges();

            return salvataggio > 0;
        } // fine aggiornamento intervento

        public bool Delete(int id)
        {
            // cerco l'intervento e se lo trovo lo cancello definitivamente dal database
            var entity = _context.Interventos.Find(id);

            if (entity == null)
            {
                return false;
            }

            _context.Interventos.Remove(entity);
            var salvataggio = _context.SaveChanges();

            return salvataggio > 0;
        } // fine eliminazione intervento
    }
}