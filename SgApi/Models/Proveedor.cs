using System.ComponentModel.DataAnnotations;

namespace SgApi.Models
{
    public class Proveedor
    {

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string RazonSocial { get; set; } = null!;
        public string? Cuit { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public ICollection<Producto>? Productos { get; set; }


    }
}
