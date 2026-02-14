namespace SgApi.Dtos.Clientes
{
    public record ClienteSaldoAjusteDto(
       decimal Importe,   // puede ser + o -
       string? Motivo
   );
}

