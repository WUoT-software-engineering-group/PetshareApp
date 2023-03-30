using Petshare.Domain.Repositories.Abstract;
using System.Linq.Expressions;

namespace Petshare.DataPersistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IPetshareDbContext _repositoryDbContext;

        public Repository(IPetshareDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<IEnumerable<T>> FindAll() => await _repositoryDbContext.Set<T>().ToListAsyncSafe();

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression) =>
            await _repositoryDbContext.Set<T>().Where(expression).ToListAsyncSafe();

        public async Task<T> Create(T entity) => (await _repositoryDbContext.Set<T>().AddAsync(entity)).Entity;

        public Task Update(T entity)
        {
            _repositoryDbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            _repositoryDbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}