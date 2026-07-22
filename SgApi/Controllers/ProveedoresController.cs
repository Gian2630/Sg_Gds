using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SgApi.Config;
using SgApi.Dtos.Proveedores;
using SgApi.Models;


namespace SgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorReadDto>>> GetProveedores()
        {
            var query = _context.Proveedores.AsQueryable();
            var proveedores = await query.Select(p => new ProveedorReadDto
            {
                Id = p.Id,
                RazonSocial = p.RazonSocial,
                Cuit = p.Cuit,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Activo = p.Activo
            }).ToListAsync();

            return Ok(proveedores);
        }

        //GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorReadDto>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            var proveedorDto = new ProveedorReadDto
            {
                Id = proveedor.Id,
                RazonSocial = proveedor.RazonSocial,
                Cuit = proveedor.Cuit,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Direccion = proveedor.Direccion,
                Activo = proveedor.Activo
            };
            return Ok(proveedorDto);
        }

        //POST: api/Proveedores
        [HttpPost]
        public async Task<ActionResult<ProveedorReadDto>> CreateProveedor(ProveedorCreateDto dto)
        {
            var existe = await _context.Proveedores.AnyAsync(p => p.RazonSocial == dto.RazonSocial);
            if (existe)
                return BadRequest("Ya existe un proveedor con esa Razón Social.");

            var proveedor = new Proveedor
            {
                RazonSocial = dto.RazonSocial,
                Cuit = dto.Cuit,
                Telefono = dto.Telefono,
                Email = dto.Email,
                Direccion = dto.Direccion,
                Activo = true
            };
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            var ReadDto = new ProveedorReadDto
            {
                Id = proveedor.Id,
                RazonSocial = proveedor.RazonSocial,
                Cuit = proveedor.Cuit,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Direccion = proveedor.Direccion,
                Activo = proveedor.Activo
            };
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, ReadDto);
        }

        // PUT: api/proveedores/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProveedor(int id, ProveedorUpdateDto dto)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            // (Opcional) evitar duplicados por RazonSocial cuando cambia
            var existe = await _context.Proveedores.AnyAsync(p => p.Id != id && p.RazonSocial == dto.RazonSocial);
            if (existe)
                return BadRequest("Ya existe otro proveedor con esa Razón Social.");

            proveedor.RazonSocial = dto.RazonSocial;
            proveedor.Cuit = dto.Cuit;
            proveedor.Telefono = dto.Telefono;
            proveedor.Email = dto.Email;
            proveedor.Direccion = dto.Direccion;
            proveedor.Activo = dto.Activo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/proveedores/5 (borrado lógico)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            proveedor.Activo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/proveedores/5/reactivar
        [HttpPatch("{id:int}/reactivar")]
        public async Task<IActionResult> ReactivarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            proveedor.Activo = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    } }
