using SgApi.Enums;

namespace SgApi.Dtos.Venta.Read
{
    public class VentaPagoReadDto
    {
        public MetodoPago Metodo { get; set; }
        public decimal Importe { get; set; }
        public string? Referencia { get; set; }
    }
}
