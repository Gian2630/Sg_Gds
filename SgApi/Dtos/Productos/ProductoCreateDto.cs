using SgApi.Enums;
using SgApi.Models;

namespace SgApi.Dtos.Productos
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Familia { get; set; }
        public TipoProducto Tipo { get; set; }

        public decimal? PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }

        public int? IdProveedor { get; set; }
    }
}
