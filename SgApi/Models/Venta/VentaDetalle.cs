namespace SgApi.Models.Venta
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public Venta Venta { get; set; }  = null!;
        public Producto Producto { get; set; } = null!;
    }
}
