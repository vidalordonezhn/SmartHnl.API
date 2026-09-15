using SmartHnl.API.Data;
using SmartHnl.API.Features.Auth;
using SmartHnl.API.Features.Usuarios;
using SmartHnl.API.Features.Clientes;
using SmartHnl.API.Features.Proveedores;
using SmartHnl.API.Features.Acreedores;
using SmartHnl.API.Features.Categorias;
using SmartHnl.API.Features.Productos;
using SmartHnl.API.Features.Inventario;
using SmartHnl.API.Features.Facturacion;
using SmartHnl.API.Features.Cotizaciones;
using SmartHnl.API.Features.NotasCredito;
using SmartHnl.API.Features.Compras;
using SmartHnl.API.Features.BoletasCompra;
using SmartHnl.API.Features.CuentasCobrar;
using SmartHnl.API.Features.CuentasPagar;
using SmartHnl.API.Features.CajaChica;
using SmartHnl.API.Features.CosteoObras;
using SmartHnl.API.Features.Garantias;
using SmartHnl.API.Features.Reportes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Base de datos: Entity Framework Core + PostgreSQL
builder.Services.AddDbContext<SmartHnlDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Acceso a HttpContext (Auditoría automática)
builder.Services.AddHttpContextAccessor();

// 3. Inyección de Servicios de Aplicación
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<ClientesService>();
builder.Services.AddScoped<ProveedoresService>();
builder.Services.AddScoped<AcreedoresService>();
builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<ProductosService>();
builder.Services.AddScoped<InventarioService>();
builder.Services.AddScoped<FacturacionService>();
builder.Services.AddScoped<CotizacionesService>();
builder.Services.AddScoped<NotasCreditoService>();
builder.Services.AddScoped<ComprasService>();
builder.Services.AddScoped<BoletasCompraService>();
builder.Services.AddScoped<CuentasCobrarService>();
builder.Services.AddScoped<CuentasPagarService>();
builder.Services.AddScoped<CajaChicaService>();
builder.Services.AddScoped<CosteoObrasService>();
builder.Services.AddScoped<GarantiasService>();
builder.Services.AddScoped<ReportesService>();

// 4. Autenticación JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "SmartHnl_SuperSecretKey_2026_Fiscal_Honduras_SAR!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "SmartHnl.API",
        ValidAudience = jwtSettings["Audience"] ?? "SmartHnl.Client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// 5. CORS para Angular (localhost:4200)
builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartHnlCors", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// 6. Controllers
builder.Services.AddControllers();

// 7. Swagger UI con soporte Bearer Token
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Smart HNL POS & ERP API",
        Version = "v1",
        Description = "Sistema de facturación fiscal SAR Honduras, control de inventario, compras, cotizaciones y caja chica."
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT. Ejemplo: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 8. Inicialización de Base de Datos y Datos Semilla
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SmartHnlDbContext>();
    try
    {
        await DbSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SmartHnl.API] Error al sembrar base de datos: {ex.Message}");
    }
}

// 9. Pipeline de Middlewares HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SmartHnlCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
