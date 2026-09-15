using SmartHnl.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHnl.API.Data
{
    public class SmartHnlDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SmartHnlDbContext(DbContextOptions<SmartHnlDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<ClienteCategoria> ClienteCategorias => Set<ClienteCategoria>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<Acreedor> Acreedores => Set<Acreedor>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Kardex> KardexEntries => Set<Kardex>();
        public DbSet<Serie> Series => Set<Serie>();
        public DbSet<Factura> Facturas => Set<Factura>();
        public DbSet<FacturaDetalle> FacturaDetalles => Set<FacturaDetalle>();
        public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
        public DbSet<CotizacionDetalle> CotizacionDetalles => Set<CotizacionDetalle>();
        public DbSet<Compra> Compras => Set<Compra>();
        public DbSet<CompraDetalle> CompraDetalles => Set<CompraDetalle>();
        public DbSet<AutorizacionCaiNotaCredito> CaiNotasCredito => Set<AutorizacionCaiNotaCredito>();
        public DbSet<NotaCredito> NotasCredito => Set<NotaCredito>();
        public DbSet<NotaCreditoDetalle> NotaCreditoDetalles => Set<NotaCreditoDetalle>();
        public DbSet<CuentaPorCobrar> CuentasPorCobrar => Set<CuentaPorCobrar>();
        public DbSet<CuentaPorPagar> CuentasPorPagar => Set<CuentaPorPagar>();
        public DbSet<ConfiguracionEmpresa> Configuracion => Set<ConfiguracionEmpresa>();
        public DbSet<TerminoPago> TerminosPago => Set<TerminoPago>();
        public DbSet<Garantia> Garantias => Set<Garantia>();
        public DbSet<AutorizacionCaiBoletaCompra> CaiBoletasCompra => Set<AutorizacionCaiBoletaCompra>();
        public DbSet<BoletaCompra> BoletasCompra => Set<BoletaCompra>();
        public DbSet<BoletaCompraDetalle> BoletaCompraDetalles => Set<BoletaCompraDetalle>();
        public DbSet<Banco> Bancos => Set<Banco>();
        public DbSet<CajaChicaAcreedor> CajaChicaAcreedores => Set<CajaChicaAcreedor>();
        public DbSet<CajaChicaEmpleado> CajaChicaEmpleados => Set<CajaChicaEmpleado>();
        public DbSet<CajaChica> CajasChicas => Set<CajaChica>();
        public DbSet<CajaChicaCuenta> CajaChicaCuentas => Set<CajaChicaCuenta>();
        public DbSet<CajaChicaGasto> CajaChicaGastos => Set<CajaChicaGasto>();
        public DbSet<CajaChicaDocumento> CajaChicaDocumentos => Set<CajaChicaDocumento>();
        public DbSet<CajaChicaDocumentoItem> CajaChicaDocumentoItems => Set<CajaChicaDocumentoItem>();
        public DbSet<CajaChicaReembolso> CajaChicaReembolsos => Set<CajaChicaReembolso>();
        public DbSet<CajaChicaMovimiento> CajaChicaMovimientos => Set<CajaChicaMovimiento>();
        public DbSet<ProyectoCosto> ProyectosCostos => Set<ProyectoCosto>();
        public DbSet<ProyectoCostoItem> ProyectoCostoItems => Set<ProyectoCostoItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo de tablas PostgreSQL en snake_case
            modelBuilder.Entity<Usuario>().ToTable("users").HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<ClienteCategoria>().ToTable("client_categories");
            modelBuilder.Entity<Cliente>().ToTable("clients");
            modelBuilder.Entity<Proveedor>().ToTable("providers");
            modelBuilder.Entity<Acreedor>().ToTable("acreedores");
            modelBuilder.Entity<Categoria>().ToTable("categories");
            modelBuilder.Entity<Producto>().ToTable("products");
            modelBuilder.Entity<Kardex>().ToTable("kardex");
            modelBuilder.Entity<Serie>().ToTable("series");
            modelBuilder.Entity<Factura>().ToTable("invoices");
            modelBuilder.Entity<FacturaDetalle>().ToTable("invoice_details");
            modelBuilder.Entity<Cotizacion>().ToTable("quotations");
            modelBuilder.Entity<CotizacionDetalle>().ToTable("quotation_details");
            modelBuilder.Entity<Compra>().ToTable("purchases");
            modelBuilder.Entity<CompraDetalle>().ToTable("purchase_details");
            modelBuilder.Entity<AutorizacionCaiNotaCredito>().ToTable("credit_note_cai");
            modelBuilder.Entity<NotaCredito>().ToTable("credit_notes");
            modelBuilder.Entity<NotaCreditoDetalle>().ToTable("credit_note_details");
            modelBuilder.Entity<CuentaPorCobrar>().ToTable("accounts_receivable");
            modelBuilder.Entity<CuentaPorPagar>().ToTable("accounts_payable");
            modelBuilder.Entity<ConfiguracionEmpresa>().ToTable("settings");
            modelBuilder.Entity<TerminoPago>().ToTable("payment_terms");
            modelBuilder.Entity<Garantia>().ToTable("garantias");
            modelBuilder.Entity<AutorizacionCaiBoletaCompra>().ToTable("boleta_compra_cai");
            modelBuilder.Entity<BoletaCompra>().ToTable("boletas_compra");
            modelBuilder.Entity<BoletaCompraDetalle>().ToTable("boleta_detalle");
            modelBuilder.Entity<Banco>().ToTable("banks");
            modelBuilder.Entity<CajaChicaAcreedor>().ToTable("petty_cash_creditors");
            modelBuilder.Entity<CajaChicaEmpleado>().ToTable("petty_cash_employees");
            modelBuilder.Entity<CajaChica>().ToTable("petty_cash");
            modelBuilder.Entity<CajaChicaCuenta>().ToTable("petty_cash_accounts");
            modelBuilder.Entity<CajaChicaGasto>().ToTable("petty_cash_expenses");
            modelBuilder.Entity<CajaChicaDocumento>().ToTable("petty_cash_documents");
            modelBuilder.Entity<CajaChicaDocumentoItem>().ToTable("petty_cash_document_items");
            modelBuilder.Entity<CajaChicaReembolso>().ToTable("petty_cash_reimbursements");
            modelBuilder.Entity<CajaChicaMovimiento>().ToTable("petty_cash_movements");
            modelBuilder.Entity<ProyectoCosto>().ToTable("project_costs");
            modelBuilder.Entity<ProyectoCostoItem>().ToTable("project_cost_items");

            // Configurar relaciones con cascada controlada
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(d => d.Invoice)
                .WithMany(i => i.Details)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CotizacionDetalle>()
                .HasOne(d => d.Quotation)
                .WithMany(q => q.Details)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompraDetalle>()
                .HasOne(d => d.Purchase)
                .WithMany(p => p.Details)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotaCreditoDetalle>()
                .HasOne(d => d.CreditNote)
                .WithMany(n => n.Details)
                .HasForeignKey(d => d.CreditNoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoletaCompraDetalle>()
                .HasOne(d => d.Boleta)
                .WithMany(b => b.Details)
                .HasForeignKey(d => d.BoletaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CajaChicaDocumentoItem>()
                .HasOne(i => i.Document)
                .WithMany(d => d.Items)
                .HasForeignKey(i => i.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProyectoCostoItem>()
                .HasOne(i => i.ProjectCost)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.ProjectCostId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var usuarioActual = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "sistema";
            var ahora = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.FechaCreacion = ahora;
                    entry.Entity.CreadoPor = usuarioActual;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.FechaModificacion = ahora;
                    entry.Entity.ModificadoPor = usuarioActual;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
