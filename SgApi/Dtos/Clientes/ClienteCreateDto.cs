namespace SgApi.Dtos.Clientes
{
    public record ClienteCreateDto(
        string RazonSocial,
        string? Celular,
        string? DniCuit,
        string? Email,
        string? Direccion,
        string? Localidad,
        int CondicionIva,
        decimal DescuentoPorc,
        decimal CreditoLimite,
        string? Observaciones
    );
}
