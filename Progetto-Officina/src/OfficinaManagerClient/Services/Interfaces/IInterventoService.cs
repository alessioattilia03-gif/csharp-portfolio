using OffinicinaShared;

namespace Officina.Client.Services.Interfaces
{
    public interface IInterventoService
    {
        Task<IEnumerable<InterventoDTO>> CercaTutti();
        Task<IEnumerable<InterventoDTO>> CercaAttivi();
        Task<bool> Inserisci(InterventoDTO dto);
        Task<InterventoDTO?> CercaPerCodice(string codicePub);
        Task<bool> Aggiorna(InterventoDTO dto);
        Task<bool> Elimina(string codicePub);
    }
}