using Officina.API.Models;

namespace Officina.API.Repositories.Interfaces
{
    public interface IInterventoRepo : IRepoLettura<Intervento>, IRepoScrittura<Intervento>
    {
        IEnumerable<Intervento> GetAttivi();
        IEnumerable<Intervento> GetByVeicolo(int veicoloId);
        Intervento? GetByCodicePub(string codicePub);
    }
}