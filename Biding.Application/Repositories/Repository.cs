using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biding_management_System.Data;
using Biding.Application.IRepositories;

namespace Biding.Application.Repositories
{
    //My genirc Repository 
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly SystemDbContext _context;

        public Repository(SystemDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
