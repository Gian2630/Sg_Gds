using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SgApi.Config;
using SgApi.Dtos.Venta.Create;
using SgApi.Dtos.Venta.Read;
using SgApi.Enums;
using SgApi.Models.Venta;

namespace SgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly AppDbContext _context;

            public VentaController(AppDbContext context)
            {
                _context = context;
            }

        // POST: api/venta
        // Registra una venta, descuenta el stock y deja en cuenta corriente el importe no pagado.
        [HttpPost]
        public async Task<ActionResult<VentaReadDto>> CreateVenta([FromBody] VentaCreateDto dto)
        {
            if (dto.IdCliente <= 0)
                return BadRequest(new { mensaje = "Debe indicar un cliente." });

            if (!Enum.IsDefined(dto.TipoComprobante))
                return BadRequest(new { mensaje = "El tipo de comprobante no es válido." });

            if (dto.Detalles is null || dto.Detalles.Count == 0)
                return BadRequest(new { mensaje = "La venta debe tener al menos un producto." });

            if (dto.Pagos is null || dto.Pagos.Count == 0)
                return BadRequest(new { mensaje = "La venta debe incluir al menos un pago." });

            if (dto.Detalles.Any(d => d.ProductoId <= 0 || d.Cantidad <= 0))
                return BadRequest(new { mensaje = "Cada detalle debe tener un producto y una cantidad mayor a cero." });

            if (dto.Pagos.Any(p => !Enum.IsDefined(p.Metodo) || p.Importe <= 0))
                return BadRequest(new { mensaje = "Cada pago debe tener un método válido y un importe mayor a cero." });

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == dto.IdCliente);
            if (cliente is null || !cliente.Activo)
                return BadRequest(new { mensaje = "El cliente indicado no existe o está inactivo." });

            var cantidadesPorProducto = dto.Detalles
                .GroupBy(d => d.ProductoId)
                .ToDictionary(g => g.Key, g => g.Sum(d => d.Cantidad));

            var productos = await _context.Productos
                .Where(p => cantidadesPorProducto.Keys.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            if (productos.Count != cantidadesPorProducto.Count)
                return BadRequest(new { mensaje = "Uno o más productos no existen." });

            foreach (var (productoId, cantidad) in cantidadesPorProducto)
            {
                var producto = productos[productoId];
                if (!producto.Activo)
                    return BadRequest(new { mensaje = $"El producto '{producto.Nombre}' está inactivo." });

                if (producto.Stock < cantidad)
                    return BadRequest(new
                    {
                        mensaje = $"Stock insuficiente para '{producto.Nombre}'.",
                        stockDisponible = producto.Stock,
                        cantidadSolicitada = cantidad
                    });
            }

            var detalles = dto.Detalles.Select(d =>
            {
                var producto = productos[d.ProductoId];
                return new VentaDetalle
                {
                    IdProducto = producto.Id,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = producto.PrecioVenta,
                    Subtotal = decimal.Round(producto.PrecioVenta * d.Cantidad, 2, MidpointRounding.AwayFromZero)
                };
            }).ToList();

            var total = detalles.Sum(d => d.Subtotal);
            var totalPagado = dto.Pagos.Sum(p => p.Importe);
            if (totalPagado > total)
                return BadRequest(new { mensaje = "El total de pagos no puede superar el total de la venta." });

            var saldoACuentaCorriente = total - totalPagado;
            if (saldoACuentaCorriente > 0 &&
                (cliente.CreditoLimite <= 0 || cliente.Saldo + saldoACuentaCorriente > cliente.CreditoLimite))
            {
                return BadRequest(new { mensaje = "El importe pendiente supera el límite de crédito del cliente." });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venta = new Venta
                {
                    IdCliente = cliente.IdCliente,
                    TipoComprobante = dto.TipoComprobante,
                    Fecha = DateTime.UtcNow,
                    Total = total,
                    Detalles = detalles,
                    Pagos = dto.Pagos.Select(p => new VentaPago
                    {
                        MedioPago = (int)p.Metodo,
                        Importe = p.Importe
                    }).ToList()
                };

                foreach (var (productoId, cantidad) in cantidadesPorProducto)
                {
                    var producto = productos[productoId];
                    producto.Stock -= cantidad;
                    producto.FechaActualizacion = DateTime.UtcNow;
                }

                if (saldoACuentaCorriente > 0)
                {
                    cliente.Saldo += saldoACuentaCorriente;
                    cliente.UpdatedAt = DateTime.UtcNow;
                }

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, ToReadDto(venta, cliente, productos));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaReadDto>> GetVenta(int id)
        {
            var venta = await _context.Ventas
                .AsNoTracking()
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(v => v.Pagos)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound(new { mensaje = "Venta no encontrada" });


            return Ok(ToReadDto(venta));
        }

        private static VentaReadDto ToReadDto(
            Venta venta,
            SgApi.Models.Cliente? cliente = null,
            IReadOnlyDictionary<int, SgApi.Models.Producto>? productos = null)
        {
            return new VentaReadDto
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                IdCliente = venta.IdCliente,
                ClienteNombre = cliente?.RazonSocial ?? venta.Cliente?.RazonSocial ?? "(Consumidor final)",
                TipoComprobante = venta.TipoComprobante,
                Total = venta.Total,
                Detalles = venta.Detalles.Select(d => new VentaDetalleReadDto
                {
                    IdProducto = d.IdProducto,
                    ProductoNombre = productos?[d.IdProducto].Nombre ?? d.Producto?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList(),
                Pagos = venta.Pagos.Select(p => new VentaPagoReadDto
                {
                    Metodo = (MetodoPago)p.MedioPago,
                    Importe = p.Importe,
                    Referencia = null
                }).ToList()
            };
        }
        

    }
}
