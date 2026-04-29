using OffinicinaShared;

namespace Officina.API.Services.Interfaces
{
    public interface IInterventoService : IService<InterventoDTO>
    {
        IEnumerable<InterventoDTO> CercaAttivi();
        IEnumerable<InterventoDTO> CercaPerVeicolo(int veicoloId);
        bool CambiaStato(int id, string nuovoStato);
        bool EliminaPerCodice(string codicePub);
    }
}