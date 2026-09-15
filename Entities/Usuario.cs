using System;

namespace SmartHnl.API.Entities
{
    public class Usuario : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = "CAJERO"; // ADMIN, CAJERO, VISUALIZADOR, GESTOR
        public string Permissions { get; set; } = "{}"; // JSON con matriz de permisos
        public bool Activo { get; set; } = true;
    }
}
