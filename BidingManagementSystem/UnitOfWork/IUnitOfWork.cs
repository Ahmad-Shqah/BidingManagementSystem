namespace Biding_management_System.UnitOfWork;

using  Biding.Application.IRepositories;

//not used: for future updates.

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    void SaveChanges();
}
