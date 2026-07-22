using SgApi.Enums;

namespace SgApi.Models.Venta
{
    public class Venta
    { 
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public TipoComprobante TipoComprobante { get; set; }
        public decimal Total { get; set; }
        public bool Activo {get; set; } = true;

        public int IdCliente{ get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
        public ICollection<VentaPago> Pagos { get; set; } = new List<VentaPago>();
    }
}
