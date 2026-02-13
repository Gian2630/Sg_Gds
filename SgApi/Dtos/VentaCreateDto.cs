namespace SgApi.Dtos
{
    public class VentaCreateDto
    {
       // public string Cliente { get; set; }
        public string TipoComprobante { get; set; }
        public int IdCliente { get; set; }
        public decimal Total { get; set; }

        public List<DetalleDto> Detalles { get; set; }
        public List<PagoDto> Pagos { get; set; }
    }

    public class DetalleDto
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class PagoDto
    {
        public int MedioPagoId { get; set; }
        public decimal Importe { get; set; }
    }
}

