using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Proveedor : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public string Rtn { get; set; } = null!;
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactName { get; set; } = "";

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
