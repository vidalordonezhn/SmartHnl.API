using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class ClienteCategoria : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
