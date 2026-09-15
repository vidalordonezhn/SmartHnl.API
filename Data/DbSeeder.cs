using SmartHnl.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SmartHnl.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(SmartHnlDbContext context)
        {
            // Asegurar que las tablas existan
            await context.Database.EnsureCreatedAsync();

            // 1. Usuarios iniciales
            if (!await context.Usuarios.AnyAsync())
            {
                var adminUser = new Usuario
                {
                    Id = "admin-id",
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Name = "Administrador Smart HNL",
                    Role = "ADMIN",
                    Permissions = "{\"users\":true,\"clients\":true,\"providers\":true,\"products\":true,\"inventory\":true,\"inventory_adjustment\":true,\"billing\":true,\"accounts_receivable\":true,\"accounts_payable\":true,\"reports\":true,\"settings\":true,\"credit_notes\":true,\"boleta_compra\":true,\"quotations\":true,\"garantias\":true,\"petty_cash\":true,\"service_costs\":true}",
                    Activo = true
                };

                var cajeroUser = new Usuario
                {
                    Id = "cajero-id",
                    Username = "cajero",
                    Password = BCrypt.Net.BCrypt.HashPassword("cajero"),
                    Name = "Cajero de Turno",
                    Role = "CAJERO",
                    Permissions = "{\"users\":false,\"clients\":true,\"providers\":false,\"products\":false,\"inventory\":false,\"inventory_adjustment\":false,\"billing\":true,\"accounts_receivable\":true,\"accounts_payable\":false,\"reports\":false,\"settings\":false,\"credit_notes\":true,\"boleta_compra\":false,\"quotations\":true,\"garantias\":true,\"petty_cash\":true,\"service_costs\":true}",
                    Activo = true
                };

                await context.Usuarios.AddRangeAsync(adminUser, cajeroUser);
                await context.SaveChangesAsync();
            }

            // 2. Categorías de Clientes
            if (!await context.ClienteCategorias.AnyAsync())
            {
                var cats = new List<ClienteCategoria>
                {
                    new() { Id = "cc-general", Name = "General" },
                    new() { Id = "cc-wholesale", Name = "Mayorista" },
                    new() { Id = "cc-vip", Name = "VIP" },
                    new() { Id = "cc-frecuente", Name = "Frecuente" }
                };
                await context.ClienteCategorias.AddRangeAsync(cats);
                await context.SaveChangesAsync();
            }

            // 3. Consumidor Final
            if (!await context.Clientes.AnyAsync(c => c.Id == "cf-id"))
            {
                var cf = new Cliente
                {
                    Id = "cf-id",
                    Name = "Consumidor Final",
                    Rtn = "00000000000000",
                    Email = "consumidorfinal@smarthnl.com",
                    Phone = "+504 0000-0000",
                    Address = "Tegucigalpa, Honduras",
                    ExonerationActive = 0,
                    Status = "ACTIVO",
                    CategoryId = "cc-general",
                    CreditLimit = 0.00m
                };
                await context.Clientes.AddAsync(cf);
                await context.SaveChangesAsync();
            }

            // 4. Términos de Pago
            if (!await context.TerminosPago.AnyAsync())
            {
                var terms = new List<TerminoPago>
                {
                    new() { Id = "term_efectivo", Name = "EFECTIVO", Type = "CONTADO", Days = 0, IsActive = 1 },
                    new() { Id = "term_linkpago", Name = "LINK PAGO", Type = "CONTADO", Days = 0, IsActive = 1 },
                    new() { Id = "term_tarjeta", Name = "TARJETA DE CREDITO/DEBITO", Type = "CONTADO", Days = 0, IsActive = 1 },
                    new() { Id = "term_transferencia", Name = "TRANSFERENCIA BANCARIA", Type = "CONTADO", Days = 0, IsActive = 1 },
                    new() { Id = "term_credito_15", Name = "CREDITO 15 DIAS", Type = "CREDITO", Days = 15, IsActive = 1 },
                    new() { Id = "term_credito_30", Name = "CREDITO 30 DIAS", Type = "CREDITO", Days = 30, IsActive = 1 },
                    new() { Id = "term_credito_90", Name = "CREDITO 90 DIAS", Type = "CREDITO", Days = 90, IsActive = 1 },
                    new() { Id = "term_credito_120", Name = "CREDITO 120 DIAS", Type = "CREDITO", Days = 120, IsActive = 1 }
                };
                await context.TerminosPago.AddRangeAsync(terms);
                await context.SaveChangesAsync();
            }

            // 5. Configuración Fiscal Empresa (Matriz + Sucursal)
            if (!await context.Configuracion.AnyAsync())
            {
                var config = new ConfiguracionEmpresa
                {
                    Id = 1,
                    Name = "Empresa Honduras S.A.",
                    CommercialName = "Smart HNL Retail",
                    DisplayNameType = "RAZON_SOCIAL",
                    Rtn = "05019654135885",
                    Address = "Boulevard Morazan, Tegucigalpa, Honduras",
                    Phone = "+504 9091-9293",
                    Email = "empresa@smarthnl.com",
                    LogoUrl = "https://images.unsplash.com/photo-1560179707-f14e90ef3623?w=150&auto=format&fit=crop&q=80",
                    Cai = "3AE914-B7C623-SD8B2F-40FD9A-8721BC-4E",
                    RangeFrom = "000-001-01-00000001",
                    RangeTo = "000-001-01-00000100",
                    ExpiryDate = "2026-12-31",
                    CurrentInvoiceNumber = "000-001-01-00000000",
                    Location = "Tegucigalpa, Honduras",
                    HonorariosCai = "3AE914-B7C623-SD8B2F-40FD9A-8721BC-4E",
                    HonorariosRangeFrom = "000-002-01-00000001",
                    HonorariosRangeTo = "000-002-01-00000100",
                    HonorariosExpiryDate = "2026-12-31",
                    HonorariosCurrentNumber = "000-002-01-00000000",
                    HideHonorarios = false
                };
                await context.Configuracion.AddAsync(config);
                await context.SaveChangesAsync();
            }

            // 6. Bancos
            if (!await context.Bancos.AnyAsync())
            {
                var bancos = new List<Banco>
                {
                    new() { Id = "bank-ficohsa", Name = "Banco Ficohsa", ShortName = "Ficohsa" },
                    new() { Id = "bank-atlantida", Name = "Banco Atlántida", ShortName = "Atlántida" },
                    new() { Id = "bank-bac", Name = "BAC Credomatic", ShortName = "BAC" },
                    new() { Id = "bank-occidente", Name = "Banco de Occidente", ShortName = "Occidente" },
                    new() { Id = "bank-banpais", Name = "Banpaís", ShortName = "Banpaís" },
                    new() { Id = "bank-davivienda", Name = "Davivienda", ShortName = "Davivienda" }
                };
                await context.Bancos.AddRangeAsync(bancos);
                await context.SaveChangesAsync();
            }

            // 7. Cuentas Contables Caja Chica
            if (!await context.CajaChicaCuentas.AnyAsync())
            {
                var cuentas = new List<CajaChicaCuenta>
                {
                    new() { Id = "pca-5101", Code = "5101", Name = "Gastos de Oficina", Description = "Artículos de papelería, útiles y consumibles" },
                    new() { Id = "pca-5102", Code = "5102", Name = "Transporte", Description = "Gastos de traslados, fletes y pasajes locales" },
                    new() { Id = "pca-5103", Code = "5103", Name = "Alimentación", Description = "Alimentación de personal y refrigerios autorizados" },
                    new() { Id = "pca-5104", Code = "5104", Name = "Combustible", Description = "Combustible y lubricantes para vehículos de operación" },
                    new() { Id = "pca-5105", Code = "5105", Name = "Reparaciones", Description = "Mantenimiento menor, repuestos y reparaciones" },
                    new() { Id = "pca-1105", Code = "1105", Name = "Anticipo de Salario", Description = "Anticipos de sueldos al personal con cargo a planilla" },
                    new() { Id = "pca-5199", Code = "5199", Name = "Otros Gastos", Description = "Gastos operativos y desembolsos menores no catalogados" }
                };
                await context.CajaChicaCuentas.AddRangeAsync(cuentas);
                await context.SaveChangesAsync();
            }

            // 8. Caja Chica Principal
            if (!await context.CajasChicas.AnyAsync())
            {
                var pc = new CajaChica
                {
                    Id = "pc-main",
                    Name = "Caja Chica Principal",
                    BaseAmount = 0.00m,
                    MinLimit = 1000.00m,
                    Responsible = "Administrador Smart HNL",
                    FundType = "CHECK",
                    Status = "ACTIVE",
                    Notes = "Fondo de operaciones y liquidación de comprobantes"
                };
                await context.CajasChicas.AddAsync(pc);
                await context.SaveChangesAsync();
            }
        }
    }
}
