using System.ComponentModel.DataAnnotations;

namespace Pragma.Application.Domain.Models
{
    public class UsuarioRequest
    {
        public string Nombre { get; set; }

        public string Rut { get; set; }

        [EmailAddress]
        public string? Correo { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
