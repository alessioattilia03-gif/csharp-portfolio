using OffinicinaShared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Officina.Client.Services.Interfaces
{
    public interface IUtenteService
    {
        Task<IEnumerable<UtenteDTO>> OttieniTutti();
        Task<bool> Registra(UtenteDTO dto);
        Task<bool> Aggiorna(UtenteDTO dto);
        Task<bool> Elimina(string codicePub);
    }
}