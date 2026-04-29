using Officina.API.Models;

namespace Officina.API.Repositories.Interfaces
{
    public interface ICodPubRepo : IRepoLettura<Cliente>, IRepoScrittura<Cliente>
    {
        Cliente? GetByCodice(string cod);
    }
}
