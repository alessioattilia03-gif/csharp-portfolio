using OffinicinaShared;

namespace Officina.API.Services.Interfaces
{
    public interface IVeicoloService : IService<VeicoloDTO>
    {
        VeicoloDTO? CercaPerTarga(string targa);
        IEnumerable<VeicoloDTO> CercaPerCliente(int clienteId);
    }
}