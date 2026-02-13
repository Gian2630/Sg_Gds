namespace SgApi.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal? preciocompra { get; set; }
        public decimal precioventa { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? Familia { get; set; }
        public string? Tipo { get; set; }
        public int IdProveedor { get; set; } 
    }
}
