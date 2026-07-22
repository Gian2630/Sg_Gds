using SgApi.Enums;

namespace SgApi.Dtos.Venta.Create
{
    public class VentaPagoCreateDto
    {
        public MetodoPago Metodo { get; set; }
        public decimal Importe { get; set; }
        public string? Referencia { get; set; }
    }
}
