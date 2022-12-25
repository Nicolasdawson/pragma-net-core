using Pragma.Application.Domain.Entities;
using System.Linq.Expressions;

namespace Pragma.Application.Domain.Specification
{
    public static class UsuarioSpec
    {
        public static IQueryable<Usuario> SearchQuery(IQueryable<Usuario> query, string searchParm)
        {
            return query.Where(x => x.Id.ToString().Contains(searchParm)
            || x.Correo.ToLower().Contains(searchParm)
            || x.Nombre.ToLower().Contains(searchParm)
            || x.Rut.ToLower().Contains(searchParm));
        }

        public static IQueryable<Usuario> SortColumns(IQueryable<Usuario> query, string sortColumn, string sortOrder)
        {
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return query;
            }

            var expression = CreateExpression<Usuario, string>(sortColumn);

            if (sortOrder == "asc")
            {
                query = query.OrderBy(expression);
            }
            else
            {
                query = query.OrderByDescending(expression);
            }

            return query;
        }

        public static Expression<Func<TSource, TKey>> CreateExpression<TSource, TKey>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TSource), "x");
            var property = Expression.Property(parameter, propertyName);


            var lambda = Expression.Lambda<Func<TSource, TKey>>(property, parameter);
            return lambda;
        }
    }
}
