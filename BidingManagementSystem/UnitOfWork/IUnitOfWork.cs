namespace Biding_management_System.UnitOfWork;

using  Biding.Application.IRepositories;


public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    void SaveChanges();
}
