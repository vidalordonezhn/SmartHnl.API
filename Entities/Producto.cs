using System;
using System.Collections.Generic;

namespace SmartHnl.API.Entities
{
    public class Producto : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ProductCode { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public Categoria? Category { get; set; }
        public decimal PurchasePrice { get; set; } = 0.00m;
        public decimal SellPrice { get; set; } = 0.00m;
        public decimal MarginPercent { get; set; } = 0.00m;
        public string TaxType { get; set; } = "GRAVADO"; // GRAVADO, GRAVADO_18, EXENTO
        public decimal Stock { get; set; } = 0.00m;
        public short IsService { get; set; } = 0;
        public string? Barcode { get; set; }
        public string? FastCode { get; set; }
        public string? Brand { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? LocationBodega { get; set; }
        public decimal StockMin { get; set; } = 10.00m;
        public string? PricesJson { get; set; }
        public string? ImageUrl { get; set; }
        public string RoundingType { get; set; } = "NONE";
        public short ManageSeries { get; set; } = 0;
        public string TieneGarantia { get; set; } = "No";
        public int DiasGarantia { get; set; } = 0;

        public ICollection<Kardex> KardexEntries { get; set; } = new List<Kardex>();
        public ICollection<Serie> Series { get; set; } = new List<Serie>();
    }
}
