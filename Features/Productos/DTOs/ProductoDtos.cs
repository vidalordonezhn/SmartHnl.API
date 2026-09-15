using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Productos.DTOs
{
    public class ProductoDto
    {
        public string Id { get; set; } = null!;
        public string? ProductCode { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal MarginPercent { get; set; }
        public string TaxType { get; set; } = "GRAVADO";
        public decimal Stock { get; set; }
        public short IsService { get; set; }
        public string? Barcode { get; set; }
        public string? FastCode { get; set; }
        public string? Brand { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? LocationBodega { get; set; }
        public decimal StockMin { get; set; }
        public string? PricesJson { get; set; }
        public string? ImageUrl { get; set; }
        public string RoundingType { get; set; } = "NONE";
        public short ManageSeries { get; set; }
        public string TieneGarantia { get; set; } = "No";
        public int DiasGarantia { get; set; }
    }

    public class ProductoCreateDto
    {
        public string? ProductCode { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public decimal PurchasePrice { get; set; } = 0.00m;
        public decimal SellPrice { get; set; } = 0.00m;
        public decimal MarginPercent { get; set; } = 0.00m;
        public string TaxType { get; set; } = "GRAVADO";
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
    }
}
