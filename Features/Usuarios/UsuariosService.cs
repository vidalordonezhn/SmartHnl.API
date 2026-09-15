using SmartHnl.API.Data;
using SmartHnl.API.Entities;
using SmartHnl.API.Features.Usuarios.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SmartHnl.API.Features.Usuarios
{
    public class UsuariosService
    {
        private readonly SmartHnlDbContext _context;

        public UsuariosService(SmartHnlDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioResponseDto>> GetAllAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Role = u.Role,
                    Permissions = u.Permissions,
                    Activo = u.Activo
                }).ToListAsync();
        }

        public async Task<UsuarioResponseDto?> CreateAsync(UsuarioCreateDto dto)
        {
            var exists = await _context.Usuarios.AnyAsync(u => u.Username.ToLower() == dto.Username.ToLower());
            if (exists) throw new Exception("El nombre de usuario ya está en uso.");

            var user = new Usuario
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Role = dto.Role,
                Permissions = dto.Permissions,
                Activo = true
            };

            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UsuarioResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Role = user.Role,
                Permissions = user.Permissions,
                Activo = user.Activo
            };
        }

        public async Task<UsuarioResponseDto?> UpdateAsync(string id, UsuarioUpdateDto dto)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(dto.Username)) user.Username = dto.Username;
            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Role)) user.Role = dto.Role;
            if (!string.IsNullOrEmpty(dto.Permissions)) user.Permissions = dto.Permissions;
            if (dto.Activo.HasValue) user.Activo = dto.Activo.Value;
            if (!string.IsNullOrEmpty(dto.Password)) user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();

            return new UsuarioResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Role = user.Role,
                Permissions = user.Permissions,
                Activo = user.Activo
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return false;

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
