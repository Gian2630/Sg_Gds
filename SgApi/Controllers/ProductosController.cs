using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SgApi.Config;
using SgApi.Dtos.Productos;
using SgApi.Models;


namespace SgApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // Get api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _context.Productos       
            .Select(p => new ProductoReadDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Familia = p.Familia,
            Tipo = p.Tipo.ToString(),
            PrecioVenta = p.PrecioVenta,
            Stock = p.Stock,
            StockMinimo = p.StockMinimo,
           // IdProveedor = p.IdProveedor
        })
        .ToListAsync();

            return Ok(productos);
        }

        // Get api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        // Post
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // Put
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest();

            producto.FechaActualizacion = DateTime.Now;

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE (Borrado logico, desactivación del producto)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            producto.Activo = false;
   
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
