using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.DataPersistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public Repository(RepositoryDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public IQueryable<T> FindAll() => _repositoryDbContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _repositoryDbContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity) => _repositoryDbContext.Set<T>().Add(entity);

        public void Update(T entity) => _repositoryDbContext.Set<T>().Update(entity);

        public void Delete(T entity) => _repositoryDbContext.Set<T>().Remove(entity);
    }
}