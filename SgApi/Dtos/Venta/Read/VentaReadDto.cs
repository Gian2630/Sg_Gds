using SgApi.Enums;

namespace SgApi.Dtos.Venta.Read
{
    public class VentaReadDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = null!;

        public TipoComprobante TipoComprobante { get; set; }

        public decimal Total { get; set; }

        public List<VentaDetalleReadDto> Detalles { get; set; } = new();
        public List<VentaPagoReadDto> Pagos { get; set; } = new();
    }
}
