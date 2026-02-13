namespace SgApi.Models.Venta
{
    public class MedioPago
    {
        //Hacer tabla
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<VentaPago> VentaPagos { get; set; }
    }

}
