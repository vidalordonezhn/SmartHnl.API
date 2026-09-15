using System;

namespace SmartHnl.API.Entities
{
    public class TerminoPago : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string Type { get; set; } = "CONTADO"; // CONTADO o CREDITO
        public int Days { get; set; } = 0;
        public short IsActive { get; set; } = 1;
    }
}
