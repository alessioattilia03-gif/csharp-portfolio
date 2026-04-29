using Officina.API.Models;

namespace Officina.API.Repositories.Interfaces
{
    public interface IUtenteRepo : IRepoLettura<Utente>, IRepoScrittura<Utente>
    {
        // metodo specifico per ottenere un utente tramite username
        Utente? GetByUsername(string username);
    }
}