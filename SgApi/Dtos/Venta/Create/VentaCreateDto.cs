using SgApi.Enums;

namespace SgApi.Dtos.Venta.Create
{
    public class VentaCreateDto
    {
        public  int IdCliente { get; set; }

        public   TipoComprobante TipoComprobante { get; set; }

        public List<VentaDetalleCreateDto> Detalles { get; set; } = new();

        public List<VentaPagoCreateDto> Pagos { get; set; } = new();
    }
}
