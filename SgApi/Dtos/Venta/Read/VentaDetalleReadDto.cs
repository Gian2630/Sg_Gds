namespace SgApi.Dtos.Venta.Read
{
    public class VentaDetalleReadDto
    {
        public int IdProducto { get; set; }
        public string ProductoNombre { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
