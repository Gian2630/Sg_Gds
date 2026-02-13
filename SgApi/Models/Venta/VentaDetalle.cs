namespace SgApi.Models.Venta
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Venta Venta { get; set; }
    public Producto Producto { get; set; }
    }
}
