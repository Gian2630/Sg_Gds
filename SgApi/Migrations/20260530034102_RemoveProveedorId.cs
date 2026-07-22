using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SgApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProveedorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentaDetalles_Ventas_VentaId",
                table: "VentaDetalles");

            migrationBuilder.RenameColumn(
                name: "VentaId",
                table: "VentaDetalles",
                newName: "IdVenta");

            migrationBuilder.RenameIndex(
                name: "IX_VentaDetalles_VentaId",
                table: "VentaDetalles",
                newName: "IX_VentaDetalles_IdVenta");

            migrationBuilder.AlterColumn<int>(
                name: "TipoComprobante",
                table: "Ventas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Ventas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "VentaDetalles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "VentaDetalles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdProveedor",
                table: "Productos",
                column: "IdProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Proveedores_IdProveedor",
                table: "Productos",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VentaDetalles_Ventas_IdVenta",
                table: "VentaDetalles",
                column: "IdVenta",
                principalTable: "Ventas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Proveedores_IdProveedor",
                table: "Productos");

            migrationBuilder.DropForeignKey(
                name: "FK_VentaDetalles_Ventas_IdVenta",
                table: "VentaDetalles");

            migrationBuilder.DropIndex(
                name: "IX_Productos_IdProveedor",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "VentaDetalles");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "VentaDetalles");

            migrationBuilder.RenameColumn(
                name: "IdVenta",
                table: "VentaDetalles",
                newName: "VentaId");

            migrationBuilder.RenameIndex(
                name: "IX_VentaDetalles_IdVenta",
                table: "VentaDetalles",
                newName: "IX_VentaDetalles_VentaId");

            migrationBuilder.AlterColumn<string>(
                name: "TipoComprobante",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VentaDetalles_Ventas_VentaId",
                table: "VentaDetalles",
                column: "VentaId",
                principalTable: "Ventas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
