using Officina.API.Models;

namespace Officina.API.Repositories.Interfaces
{
    public interface IVeicoloRepo : IRepoLettura<Veicolo>, IRepoScrittura<Veicolo>
    {
        Veicolo? GetByTarga(string targa);

        Veicolo? GetByCodicePub(string codicePub);

        IEnumerable<Veicolo> GetByCliente(int clienteId);
    }
}