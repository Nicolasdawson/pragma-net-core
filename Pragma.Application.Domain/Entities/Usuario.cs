namespace Pragma.Application.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string Rut { get; set; }

        public string Correo { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
