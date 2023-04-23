using System.Linq.Expressions;
using System.Security.Claims;

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

        public static Guid? GetId(this ClaimsIdentity identity)
        {
            if (Guid.TryParse(identity.FindFirst("db_id")?.Value, out var id))
                return id;
            return null;
        }

        public static string? GetRole(this ClaimsIdentity identity) => identity.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
    }
}
