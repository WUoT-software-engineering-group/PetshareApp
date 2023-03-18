using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Petshare.Domain.Repositories.Abstract;
using Petshare.WebAPI.Data;

namespace Petshare.DataPersistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PetshareDbContext _repositoryDbContext;

        public Repository(PetshareDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task<IEnumerable<T>> FindAll() => await _repositoryDbContext.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression) =>
            await _repositoryDbContext.Set<T>().Where(expression).ToListAsync();

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