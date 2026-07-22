using SgApi.Models;
namespace SgApi.Dtos.Productos
{
    public class ProductoReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Familia { get; set; }
        public string Tipo { get; set; } = null!;

        public decimal PrecioVenta { get; set; }
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }

        public bool Activo { get; set; }
       // public int? IdProveedor { get; set; }
    }
}
