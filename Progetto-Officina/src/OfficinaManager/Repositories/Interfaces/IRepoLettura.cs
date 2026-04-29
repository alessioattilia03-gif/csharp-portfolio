namespace Officina.API.Repositories.Interfaces
{
    public interface IRepoLettura<T>
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
    }
}
