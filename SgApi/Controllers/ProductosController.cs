using SgApi.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) { return NotFound(); };
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto (int id, Producto productoact)
        {
            var producto = await _context.Productos.FindAsync (id);
            if(producto == null) { return NotFound(); }

            producto.nombre = productoact.nombre;
            producto.preciocompra = productoact.preciocompra;
            producto.precioventa = productoact.precioventa;
            producto.FechaActualizacion = productoact.FechaActualizacion;
            producto.Tipo = productoact.Tipo;
            producto.Familia = productoact.Familia;
            producto.IdProveedor = productoact.IdProveedor;

            await _context.SaveChangesAsync();
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id, Producto producto)
        {
            var existente = await _context.Productos.FindAsync(id);
            if (existente == null) { return NotFound(); }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(existente);
        }
    }
}
