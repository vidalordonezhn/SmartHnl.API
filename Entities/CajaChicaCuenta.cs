using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaCuenta : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "ACTIVE";
    }
}
