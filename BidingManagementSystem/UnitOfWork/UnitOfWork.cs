
namespace Biding_management_System.UnitOfWork;

using Biding_management_System.Data;
using Biding.Application.IRepositories;
using Biding.Application.Repositories;


public class UnitOfWork : IUnitOfWork
{
    private readonly SystemDbContext _context;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(SystemDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
