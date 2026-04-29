using OffinicinaShared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Officina.Client.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> CercaTutti();
        Task<ClienteDTO?> CercaPerCodice(string codicePub);
        Task<bool> Inserisci(ClienteDTO dto);
        Task<bool> Aggiorna(ClienteDTO dto); 
        Task<bool> Elimina(string codicePub); 
    }
}