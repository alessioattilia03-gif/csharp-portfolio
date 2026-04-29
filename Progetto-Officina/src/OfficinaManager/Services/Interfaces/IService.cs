namespace Officina.API.Services.Interfaces
{
    public interface IService<T>
    {
        T? CercaPerCodice(string codice);
        IEnumerable<T> CercaTutti();
        bool Inserisci(T entity);
        bool Aggiorna(T entity);
        bool Elimina(int id);
    }
}
