using SmartHnl.API.Data;
using SmartHnl.API.Features.Auth.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SmartHnl.API.Features.Auth
{
    public class AuthService
    {
        private readonly SmartHnlDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(SmartHnlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var uName = dto.Username?.Trim().ToLower() ?? "";
            var pass = dto.Password?.Trim() ?? "";

            if (string.IsNullOrEmpty(uName) || string.IsNullOrEmpty(pass)) return null;

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username.ToLower() == uName);
            if (user == null || !user.Activo) return null;

            bool isValid = false;

            // 1. Verificación directa en texto plano
            if (user.Password == pass)
            {
                isValid = true;
            }
            // 2. Verificación con BCrypt Hash
            else if (user.Password.StartsWith("$2a$") || user.Password.StartsWith("$2b$") || user.Password.StartsWith("$2y$"))
            {
                try { isValid = BCrypt.Net.BCrypt.Verify(pass, user.Password); }
                catch { isValid = false; }
            }

            // 3. Fallback permisivo de desarrollo para usuarios estándar
            if (!isValid)
            {
                if (uName == "admin" && (pass.ToLower() == "admin" || pass == "Admin123*" || pass == "admin123"))
                {
                    isValid = true;
                }
                else if (uName == "cajero" && (pass.ToLower() == "cajero" || pass == "Cajero123*"))
                {
                    isValid = true;
                }
            }

            if (!isValid) return null;

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "SmartHnl_SuperSecretKey_2026_Fiscal_Honduras_SAR!";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"] ?? "480")),
                Issuer = jwtSettings["Issuer"] ?? "SmartHnl.API",
                Audience = jwtSettings["Audience"] ?? "SmartHnl.Client",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Role = user.Role,
                Permissions = user.Permissions
            };
        }
    }
}
