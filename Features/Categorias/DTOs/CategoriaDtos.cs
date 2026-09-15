using System.ComponentModel.DataAnnotations;

namespace SmartHnl.API.Features.Categorias.DTOs
{
    public class CategoriaDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class CategoriaCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
