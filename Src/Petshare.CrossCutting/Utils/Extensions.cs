using System.Linq.Expressions;

namespace Petshare.CrossCutting.Utils
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> collection) => collection is null || collection.Count == 0;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
