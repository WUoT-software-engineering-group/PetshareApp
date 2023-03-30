using System.Linq.Expressions;

namespace Petshare.Domain.Repositories.Abstract
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAll();

        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);

        Task<T> Create(T entity);

        Task Update(T entity);

        Task Delete(T entity);
    }
}
