using Microsoft.EntityFrameworkCore;

namespace Petshare.DataPersistence.Repositories
{
    public static class RepositoryHelper
    {
        /// <summary>
        /// Method prevents from ToListAsync invocation when underlying type of IQueryable is not IAsyncEnumerable (present in EF queries) 
        /// </summary>
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (source is not IAsyncEnumerable<TSource>)
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }
    }
}
