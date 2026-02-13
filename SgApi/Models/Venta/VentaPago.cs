namespace SgApi.Models.Venta
{
    public class VentaPago
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int MedioPago { get; set; }
        public decimal Importe { get; set; }

        public Venta Venta { get; set; }
        //public MedioPago MedioPago { get; set; }
    }
}
