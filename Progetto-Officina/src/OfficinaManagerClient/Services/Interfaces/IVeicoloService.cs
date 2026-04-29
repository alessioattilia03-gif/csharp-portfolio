using OffinicinaShared;

namespace Officina.Client.Services.Interfaces
{
    public interface IVeicoloService
    {
        Task<IEnumerable<VeicoloDTO>> CercaTutti();
        Task<VeicoloDTO?> CercaPerTarga(string targa);
        Task<bool> Inserisci(VeicoloDTO dto);
    }
}