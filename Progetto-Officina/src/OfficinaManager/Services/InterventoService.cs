using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Services
{
    public class InterventoService : IInterventoService
    {
        private readonly IInterventoRepo _repo;
        private readonly IVeicoloRepo _veicoloRepo;

        public InterventoService(IInterventoRepo repo, IVeicoloRepo veicoloRepo)
        {
            _repo = repo;
            _veicoloRepo = veicoloRepo;
        }

        public IEnumerable<InterventoDTO> CercaTutti()
        {
            // prendo tutti i record dal db e li trasformo in dto per il frontend usando il metodo di mapping in fondo
            var entities = _repo.GetAll();
            var listaDto = new List<InterventoDTO>();
            foreach (var i in entities) { listaDto.Add(MappaA_DTO(i)); }
            return listaDto;
        } // fine recupero totale

        public InterventoDTO? CercaPerCodice(string codicePub)
        {
            // controllo se il codice esiste prima di interrogare il repository per evitare crash
            if (string.IsNullOrWhiteSpace(codicePub)) return null;

            var i = _repo.GetByCodicePub(codicePub);
            if (i == null) return null;
            return MappaA_DTO(i);
        } // fine ricerca singola per codice

        public IEnumerable<InterventoDTO> CercaAttivi()
        {
            // filtro la lista per escludere quelli già completati e mappare solo quelli ancora da gestire
            var entities = _repo.GetAll().Where(x => x.Stato != "completato");
            var listaDto = new List<InterventoDTO>();
            foreach (var i in entities) { listaDto.Add(MappaA_DTO(i)); }
            return listaDto;
        } // fine recupero interventi aperti

        public IEnumerable<InterventoDTO> CercaPerVeicolo(int veicoloId)
        {
            // recupero lo storico dei lavori fatti su una macchina specifica tramite il suo id interno
            var entities = _repo.GetAll().Where(x => x.VeicoloId == veicoloId);
            var listaDto = new List<InterventoDTO>();
            foreach (var i in entities) { listaDto.Add(MappaA_DTO(i)); }
            return listaDto;
        } // fine ricerca per id veicolo

        public bool Inserisci(InterventoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.VeicoloTarga)) return false;

            // cerco il veicolo nel db partendo dalla targa scritta nella dashboard
            var veicolo = _veicoloRepo.GetAll().FirstOrDefault(v => v.Targa.ToLower() == dto.VeicoloTarga.Trim().ToLower());
            if (veicolo == null) return false;

            // se il veicolo ha ancora i dati di default, provo a inserire marca e modello separando la stringa info
            if (!string.IsNullOrWhiteSpace(dto.VeicoloInfo) && veicolo.Marca == "Da definire")
            {
                var infoParts = dto.VeicoloInfo.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                veicolo.Marca = infoParts.Length > 0 ? infoParts[0] : "N/D";
                veicolo.Modello = infoParts.Length > 1 ? infoParts[1] : "N/D";

                _veicoloRepo.Update(veicolo);
            }

            // creo l'oggetto intervento fisico e assegno un nuovo guid come codice pubblico
            var entity = new Intervento
            {
                CodicePub = Guid.NewGuid().ToString().ToUpper(),
                Descrizione = dto.Descrizione,
                Prezzo = dto.Prezzo ?? 0,
                Stato = (string.IsNullOrWhiteSpace(dto.Stato) ? "da fare" : dto.Stato).ToLower(),
                VeicoloId = veicolo.VeicoloId,
                DataIngresso = dto.DataIngresso == default ? DateTime.Now : dto.DataIngresso,
                DataFine = dto.DataFine
            };
            return _repo.Create(entity);
        } // fine logica inserimento intervento e update auto

        public bool CambiaStato(int id, string nuovoStato)
        {
            // cerco l'intervento e aggiorno lo stato, se è completato segno anche la data di fine attuale
            var entity = _repo.GetById(id);
            if (entity == null) return false;

            entity.Stato = nuovoStato;
            if (nuovoStato.ToLower() == "completato") { entity.DataFine = DateTime.Now; }
            return _repo.Update(entity);
        } // fine cambio stato rapido

        public bool Aggiorna(InterventoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CodicePub)) return false;

            // trovo il record originale e sovrascrivo i campi con i nuovi dati arrivati dal form di modifica
            var entity = _repo.GetByCodicePub(dto.CodicePub);
            if (entity == null) return false;

            entity.Descrizione = dto.Descrizione;
            entity.Prezzo = dto.Prezzo ?? 0;
            entity.Stato = dto.Stato;
            entity.DataIngresso = dto.DataIngresso;
            entity.DataFine = dto.DataFine;

            return _repo.Update(entity);
        } // fine aggiornamento completo

        public bool Elimina(int id)
        {
            // elimino l'intervento usando il suo id numerico
            return _repo.Delete(id);
        } // fine eliminazione per id

        private InterventoDTO MappaA_DTO(Intervento i)
        {
            // metodo helper per trasformare i dati del database nel formato dto per il client blazor
            return new InterventoDTO
            {
                CodicePub = i.CodicePub,
                Descrizione = i.Descrizione,
                DataIngresso = i.DataIngresso,
                DataFine = i.DataFine,
                Prezzo = i.Prezzo,
                Stato = i.Stato,
                VeicoloCodicePub = i.Veicolo?.CodicePub,
                VeicoloInfo = i.Veicolo != null ? $"{i.Veicolo.Marca} {i.Veicolo.Modello}" : "Veicolo non assegnato",
                VeicoloTarga = i.Veicolo?.Targa ?? "N/D"
            };
        } // fine metodo privato di mapping

        public bool EliminaPerCodice(string codicePub)
        {
            // recupero l'id interno dal codice pubblico e procedo alla cancellazione fisica sul db
            var entity = _repo.GetByCodicePub(codicePub);

            if (entity == null) return false;
            return _repo.Delete(entity.InterventoId);
        } // fine eliminazione sicura per codice pubblico
    }
}