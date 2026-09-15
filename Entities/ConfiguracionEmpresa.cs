using System;

namespace SmartHnl.API.Entities
{
    public class ConfiguracionEmpresa : AuditableEntity
    {
        public int Id { get; set; } = 1;
        public string Name { get; set; } = null!;
        public string CommercialName { get; set; } = "";
        public string DisplayNameType { get; set; } = "RAZON_SOCIAL";
        public string Rtn { get; set; } = null!;
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string LogoUrl { get; set; } = "";
        
        // CAI Principal (Casa Matriz)
        public string Cai { get; set; } = "";
        public string RangeFrom { get; set; } = "";
        public string RangeTo { get; set; } = "";
        public string ExpiryDate { get; set; } = "";
        public string CurrentInvoiceNumber { get; set; } = "";
        public string Location { get; set; } = "";
        public string? WarrantyTerms { get; set; }

        // CAI Secundario (Sucursal / Recibos por Honorarios)
        public string? HonorariosCai { get; set; }
        public string? HonorariosRangeFrom { get; set; }
        public string? HonorariosRangeTo { get; set; }
        public string? HonorariosExpiryDate { get; set; }
        public string? HonorariosCurrentNumber { get; set; }
        public bool HideHonorarios { get; set; } = false;
    }
}
