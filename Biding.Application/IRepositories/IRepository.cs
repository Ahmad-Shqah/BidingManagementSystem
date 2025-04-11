namespace Biding.Application.IRepositories
{
    public interface IRepository<T>
    {
        //my genirc Irepository :)
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
