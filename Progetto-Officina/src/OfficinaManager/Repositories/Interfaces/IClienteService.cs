using OffinicinaShared;

namespace Officina.API.Repositories.Interfaces
{
    public interface IClienteService<T>
    {
        T? CercaPerCodice(string codice);
        IEnumerable<T> CercaTutti();
        bool Inserisci(T entity);
        bool Aggiorna(T entity);
        bool Elimina(int id);
    }
}
