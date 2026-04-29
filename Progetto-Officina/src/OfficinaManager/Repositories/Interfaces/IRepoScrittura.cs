namespace Officina.API.Repositories.Interfaces
{
    public interface IRepoScrittura<T>
    {
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(int id);    
    }
}
