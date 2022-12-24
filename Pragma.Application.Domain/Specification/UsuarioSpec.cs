using Pragma.Application.Domain.Entities;

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
    }
}
