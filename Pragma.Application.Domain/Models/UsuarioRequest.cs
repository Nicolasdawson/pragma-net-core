using System.ComponentModel.DataAnnotations;

namespace Pragma.Application.Domain.Models
{
    public class UsuarioRequest
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Rut { get; set; }

        [EmailAddress]
        public string? Correo { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }
    }
}
