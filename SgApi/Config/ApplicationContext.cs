
using SgApi.Models;
using Microsoft.EntityFrameworkCore;
using SgApi.Models.Venta;

namespace SgApi.Config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<VentaPago> VentaPagos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(e =>
            {
                e.HasKey(c => c.IdCliente);

                e.Property(c => c.RazonSocial)
                    .IsRequired()
                    .HasMaxLength(150);

                e.Property(c => c.Celular).HasMaxLength(30);
                e.Property(c => c.DniCuit).HasMaxLength(20);
                e.Property(c => c.Email).HasMaxLength(150);
                e.Property(c => c.Direccion).HasMaxLength(200);
                e.Property(c => c.Localidad).HasMaxLength(120);

                // Dinero / porcentajes
                e.Property(c => c.Saldo).HasPrecision(18, 2);
                e.Property(c => c.CreditoLimite).HasPrecision(18, 2);
                e.Property(c => c.DescuentoPorc).HasPrecision(5, 2);

                // Índice único si se carga DniCuit (SQL Server permite varios NULL)
                e.HasIndex(c => c.DniCuit).IsUnique();

                // Defaults (opcional)
                e.Property(c => c.Activo).HasDefaultValue(true);
                e.Property(c => c.DescuentoPorc).HasDefaultValue(0m);
                e.Property(c => c.Saldo).HasDefaultValue(0m);
                e.Property(c => c.CreditoLimite).HasDefaultValue(0m);
            });

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.IdCliente);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithOne(d => d.Venta)
                .HasForeignKey(d => d.IdVenta);

            modelBuilder.Entity<VentaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.IdProducto);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Pagos)
                .WithOne(p => p.Venta)
                .HasForeignKey(p => p.VentaId);

            modelBuilder.Entity<Producto>()
            .HasOne(p => p.Proveedor)
              .WithMany()
             .HasForeignKey(p => p.IdProveedor);
        
            base.OnModelCreating(modelBuilder);
        }
    }
}
