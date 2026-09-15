using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaEmpleado : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string? IdentityNumber { get; set; }
        public string Status { get; set; } = "ACTIVE";
    }
}
