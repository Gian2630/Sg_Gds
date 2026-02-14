using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SgApi.Config;
using SgApi.Dtos.Clientes;
using SgApi.Models;

namespace SgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext db) => _db = db;

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<object>> GetAll(
            [FromQuery] string? q,
            [FromQuery] bool? activo,
            [FromQuery] int? condIva,
            [FromQuery] string? localidad,
            [FromQuery] bool? conSaldo,
            [FromQuery] bool? morosos,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _db.Clientes.AsNoTracking().AsQueryable();

            if (activo.HasValue)
                query = query.Where(x => x.Activo == activo.Value);

            if (condIva.HasValue)
                query = query.Where(x => (int)x.CondicionIva == condIva.Value);

            if (!string.IsNullOrWhiteSpace(localidad))
                query = query.Where(x => x.Localidad != null && x.Localidad.Contains(localidad.Trim()));

            if (conSaldo == true)
                query = query.Where(x => x.Saldo != 0);

            if (morosos == true)
                query = query.Where(x => x.Saldo > 0);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(x =>
                    x.RazonSocial.Contains(q) ||
                    (x.DniCuit != null && x.DniCuit.Contains(q)) ||
                    (x.Celular != null && x.Celular.Contains(q)) ||
                    (x.Email != null && x.Email.Contains(q)));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.RazonSocial)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }

        // GET: api/clientes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            var cliente = await _db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdCliente == id);

            return cliente is null ? NotFound() : Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] ClienteCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RazonSocial))
                return BadRequest(new { message = "RazonSocial es requerida." });

            if (dto.DescuentoPorc < 0 || dto.DescuentoPorc > 100)
                return BadRequest(new { message = "DescuentoPorc debe estar entre 0 y 100." });

            if (dto.CreditoLimite < 0)
                return BadRequest(new { message = "CreditoLimite no puede ser negativo." });

            if (!string.IsNullOrWhiteSpace(dto.DniCuit))
            {
                var exists = await _db.Clientes.AnyAsync(x => x.DniCuit == dto.DniCuit);
                if (exists) return Conflict(new { message = "Ya existe un cliente con ese DNI/CUIT." });
            }

            var cliente = new Cliente
            {
                RazonSocial = dto.RazonSocial.Trim(),
                Celular = dto.Celular?.Trim(),
                DniCuit = dto.DniCuit?.Trim(),
                Email = dto.Email?.Trim(),
                Direccion = dto.Direccion?.Trim(),
                Localidad = dto.Localidad?.Trim(),
                CondicionIva = (CondicionIva)dto.CondicionIva,
                DescuentoPorc = dto.DescuentoPorc,
                CreditoLimite = dto.CreditoLimite,
                Saldo = 0m,
                Activo = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Observaciones = dto.Observaciones?.Trim()
            };

            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cliente.IdCliente }, cliente);
        }

        // PUT: api/clientes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
        {
            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.RazonSocial))
                return BadRequest(new { message = "RazonSocial es requerida." });

            if (dto.DescuentoPorc < 0 || dto.DescuentoPorc > 100)
                return BadRequest(new { message = "DescuentoPorc debe estar entre 0 y 100." });

            if (dto.CreditoLimite < 0)
                return BadRequest(new { message = "CreditoLimite no puede ser negativo." });

            if (!string.IsNullOrWhiteSpace(dto.DniCuit) && dto.DniCuit != cliente.DniCuit)
            {
                var exists = await _db.Clientes.AnyAsync(x => x.DniCuit == dto.DniCuit && x.IdCliente != id);
                if (exists) return Conflict(new { message = "Ya existe un cliente con ese DNI/CUIT." });
            }

            cliente.RazonSocial = dto.RazonSocial.Trim();
            cliente.Celular = dto.Celular?.Trim();
            cliente.DniCuit = dto.DniCuit?.Trim();
            cliente.Email = dto.Email?.Trim();
            cliente.Direccion = dto.Direccion?.Trim();
            cliente.Localidad = dto.Localidad?.Trim();
            cliente.CondicionIva = (CondicionIva)dto.CondicionIva;
            cliente.DescuentoPorc = dto.DescuentoPorc;
            cliente.CreditoLimite = dto.CreditoLimite;
            cliente.Activo = dto.Activo;
            cliente.UpdatedAt = DateTime.UtcNow;
            cliente.Observaciones = dto.Observaciones?.Trim();

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/clientes/5  (baja lógica)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            cliente.Activo = false;
            cliente.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/clientes/5/activar
        [HttpPatch("{id:int}/activar")]
        public async Task<IActionResult> Reactivar(int id)
        {
            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            cliente.Activo = true;
            cliente.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/clientes/5/saldo  (ajuste manual)
        [HttpPatch("{id:int}/saldo")]
        public async Task<IActionResult> AjustarSaldo(int id, [FromBody] ClienteSaldoAjusteDto dto)
        {
            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            // dto.Importe puede ser + o -
            cliente.Saldo += dto.Importe;
            cliente.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { cliente.IdCliente, cliente.Saldo });
        }

        // PATCH: api/clientes/5/credito
        [HttpPatch("{id:int}/credito")]
        public async Task<IActionResult> CambiarCredito(int id, [FromBody] ClienteCreditoDto dto)
        {
            if (dto.CreditoLimite < 0)
                return BadRequest(new { message = "CreditoLimite no puede ser negativo." });

            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            cliente.CreditoLimite = dto.CreditoLimite;
            cliente.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/clientes/5/descuento
        [HttpPatch("{id:int}/descuento")]
        public async Task<IActionResult> CambiarDescuento(int id, [FromBody] ClienteDescuentoDto dto)
        {
            if (dto.DescuentoPorc < 0 || dto.DescuentoPorc > 100)
                return BadRequest(new { message = "DescuentoPorc debe estar entre 0 y 100." });

            var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (cliente is null) return NotFound();

            cliente.DescuentoPorc = dto.DescuentoPorc;
            cliente.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
