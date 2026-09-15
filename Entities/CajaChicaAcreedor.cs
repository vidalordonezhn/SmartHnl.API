using System;

namespace SmartHnl.API.Entities
{
    public class CajaChicaAcreedor : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string? Rtn { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; } = "ACTIVE";
    }
}
