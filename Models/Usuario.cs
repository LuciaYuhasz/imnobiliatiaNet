namespace imnobiliatiaNet.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Rol { get; set; } = "Empleado"; // "Administrador" o "Empleado"
        public string ClaveHash { get; set; } = "";   // Contraseña encriptada
        public string? AvatarUrl { get; set; }
    }
}
