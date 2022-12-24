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

        public static Expression<Func<Usuario, object>> SetSortableColumn(string sortColumName)
        {
            return sortColumName?.Trim().ToLower() switch
            {
                "fechaNacimiento" => x => x.FechaNacimiento,
                "correo" => x => x.Correo,
                "nombre" => x => x.Nombre,
                "rut" => x => x.Rut,
                _ => x => x.Id
            };
        }
    }
}
