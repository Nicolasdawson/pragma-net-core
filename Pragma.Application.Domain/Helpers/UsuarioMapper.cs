using Pragma.Application.Domain.Entities;
using Pragma.Application.Domain.Models;

namespace Pragma.Application.Domain.Helpers
{
    public static class UsuarioMapper
    {
        public static Usuario ToEntity(this UsuarioRequest request)
        {
            Usuario entity = new();
            if (request != null)
            {
                entity.Rut = request.Rut;
                entity.FechaNacimiento = request.FechaNacimiento;
                entity.Correo = request.Correo;
                entity.Nombre = request.Nombre;
            }

            return entity;
        }

        public static Usuario ToUpdate(this UsuarioRequest request, Usuario entity)
        {
            if (request != null && entity != null)
            {
                entity.Rut = request.Rut;
                entity.FechaNacimiento = request.FechaNacimiento;
                entity.Correo = request.Correo;
                entity.Nombre = request.Nombre;
            }

            return entity;
        }
    }
}
