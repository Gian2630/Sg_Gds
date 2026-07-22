namespace SgApi.Dtos.Proveedores
{
    public class ProveedorCreateDto
    {
        public string RazonSocial { get; set; } = null!;
        public string? Cuit { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
    }
}
