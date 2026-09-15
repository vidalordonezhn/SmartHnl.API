using System;

namespace SmartHnl.API.Entities
{
    public class Banco : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }
        public string? AccountNumber { get; set; }
        public string AccountType { get; set; } = "CHEQUES";
        public string Currency { get; set; } = "HNL";
        public string Status { get; set; } = "ACTIVE";
    }
}
