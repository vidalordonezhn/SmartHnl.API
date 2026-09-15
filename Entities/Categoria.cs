using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Categoria : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
