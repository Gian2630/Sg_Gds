namespace SgApi.Models.Venta
{
    public class Venta
    { //agregar cliente y cambiarlo x proveedor
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string TipoComprobante { get; set; }
        public decimal Total { get; set; }

        public int IdCliente{ get; set; }
        public Cliente Cliente { get; set; }
        public List<VentaDetalle> Detalles { get; set; }
        public List<VentaPago> Pagos { get; set; }
    }
}
