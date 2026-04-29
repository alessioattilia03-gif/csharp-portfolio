using Officina.API.Models;
using Officina.API.Repositories.Interfaces;
using Officina.API.Services.Interfaces;
using OffinicinaShared;

namespace Officina.API.Services
{
    public class VeicoloService : IVeicoloService
    {
        private readonly IVeicoloRepo _repo;
        private readonly IClienteRepo _clienteRepo;

        public VeicoloService(IVeicoloRepo repo, IClienteRepo clienteRepo)
        {
            _repo = repo;
            _clienteRepo = clienteRepo;
        }

        public IEnumerable<VeicoloDTO> CercaTutti()
        {
            // prendo tutti i veicoli dal db e li trasformo uno per uno in dto usando il metodo di mapping
            var entities = _repo.GetAll();
            var listaDto = new List<VeicoloDTO>();

            foreach (var v in entities)
            {
                var dto = MappaA_DTO(v);
                listaDto.Add(dto);
            }

            return listaDto;
        } // fine recupero lista completa veicoli

        public VeicoloDTO? CercaPerCodice(string codicePub)
        {
            // cerco una macchina specifica nel database tramite il suo codice pubblico univoco
            var v = _repo.GetByCodicePub(codicePub);
            if (v == null) return null;

            return MappaA_DTO(v);
        } // fine ricerca per codice pubblico

        public VeicoloDTO? CercaPerTarga(string targa)
        {
            // metodo utilissimo per la dashboard: cerco i dati dell'auto partendo dalla targa
            var v = _repo.GetByTarga(targa);
            if (v == null) return null;

            return MappaA_DTO(v);
        } // fine ricerca per targa

        public IEnumerable<VeicoloDTO> CercaPerCliente(int clienteId)
        {
            // recupero tutte le auto che appartengono a un determinato cliente usando il suo id
            var entities = _repo.GetByCliente(clienteId);
            var listaDto = new List<VeicoloDTO>();

            foreach (var v in entities)
            {
                listaDto.Add(MappaA_DTO(v));
            }

            return listaDto;
        } // fine ricerca veicoli di un cliente

        public bool Inserisci(VeicoloDTO dto)
        {
            // cerco il proprietario nel db tramite il codice pubblico per poter collegare la nuova auto
            var codiceCliente = dto.ClienteCodicePub;
            var cliente = _clienteRepo.GetByCodicePub(codiceCliente);

            if (cliente == null) return false;

            // creo l'oggetto veicolo fisico e gli assegno l'id numerico del cliente trovato
            var entity = new Veicolo();
            entity.Targa = dto.Targa;
            entity.Marca = dto.Marca;
            entity.Modello = dto.Modello;
            entity.Anno = dto.Anno;
            entity.ClienteId = cliente.ClienteId;

            return _repo.Create(entity);
        } // fine inserimento nuovo veicolo

        public bool Aggiorna(VeicoloDTO dto)
        {
            // trovo il veicolo nel db e aggiorno i campi marca, modello o anno
            var entity = _repo.GetByCodicePub(dto.CodicePub);
            if (entity == null) return false;

            entity.Targa = dto.Targa;
            entity.Marca = dto.Marca;
            entity.Modello = dto.Modello;
            entity.Anno = dto.Anno;

            return _repo.Update(entity);
        } // fine aggiornamento dati veicolo

        public bool Elimina(int id)
        {
            // elimino l'auto dal database usando il suo id numerico interno
            return _repo.Delete(id);
        } // fine eliminazione per id

        private VeicoloDTO MappaA_DTO(Veicolo v)
        {
            // metodo per trasformare l'oggetto del database nel formato dto da inviare al client blazor
            var dto = new VeicoloDTO();
            dto.CodicePub = v.CodicePub;
            dto.Targa = v.Targa;
            dto.Marca = v.Marca;
            dto.Modello = v.Modello;
            dto.Anno = v.Anno;

            if (v.Cliente != null)
            {
                // se l'auto ha un proprietario assegnato, passo anche il suo codice pubblico
                dto.ClienteCodicePub = v.Cliente.CodicePub;
            }
            else
            {
                dto.ClienteCodicePub = string.Empty;
            }

            return dto;
        } // fine metodo privato di mapping
    }
}