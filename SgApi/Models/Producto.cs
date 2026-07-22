using System.ComponentModel.DataAnnotations;
using SgApi.Enums;
namespace SgApi.Models

{
    public class Producto
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]    
        public string Nombre { get; set; } = null!;
        public string? Familia { get; set; }
        public TipoProducto Tipo { get; set; }
        public decimal? PrecioCompra { get; set; }

        [Required]
        public decimal PrecioVenta { get; set; }
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }
        public bool Activo { get; set; } = true;

        public DateTime FechaActualizacion { get; set; }

        public int? IdProveedor { get; set; }       
        public Proveedor? Proveedor { get; set; }
    }
}
