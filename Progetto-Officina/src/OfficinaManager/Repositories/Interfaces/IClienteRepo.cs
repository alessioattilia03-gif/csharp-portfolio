using Officina.API.Models;

namespace Officina.API.Repositories.Interfaces
{
    public interface IClienteRepo : IRepoLettura<Cliente>, IRepoScrittura<Cliente>
    {
        // traduzione del codice pubblico in ID interno
        Cliente? GetByCodicePub(string codicePub);
    }
}
