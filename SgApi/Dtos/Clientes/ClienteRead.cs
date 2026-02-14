namespace SgApi.Dtos.Clientes
{
    public record ClienteReadDto(
        int IdCliente,
        string RazonSocial,
        string? Celular,
        string? DniCuit,
        string? Email,
        string? Direccion,
        string? Localidad,
        int CondicionIva,
        decimal DescuentoPorc,
        decimal Saldo,
        decimal CreditoLimite,
        bool Activo,
        string? Observaciones
    );
}
