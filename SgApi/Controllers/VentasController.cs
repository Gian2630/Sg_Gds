using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SgApi.Dtos;
using SgApi.Models;
using SgApi.Config;
using System;
using SgApi.Models.Venta;

namespace SgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.Detalles)
                .Include(v => v.Pagos)
                .Include(v => v.Cliente)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CrearVenta(VentaCreateDto dto)
        {
            var venta = new Venta
            {
                TipoComprobante = dto.TipoComprobante,
                IdCliente = dto.IdCliente,
                Total = dto.Total,
                Detalles = dto.Detalles
                    .Select(d => new VentaDetalle
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList(),
                Pagos = dto.Pagos
                    .Select(p => new VentaPago
                    {
                        MedioPago = p.MedioPagoId,
                        Importe = p.Importe
                    }).ToList()
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada", id = venta.Id });
        }
    }

}
