using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SgApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProveedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // RazonSocial es NOT NULL en tu modelo, entonces ponemos un default para filas existentes
            migrationBuilder.AddColumn<string>(
                name: "RazonSocial",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cuit",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Proveedores",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "Proveedores",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FechaAlta", table: "Proveedores");
            migrationBuilder.DropColumn(name: "Activo", table: "Proveedores");
            migrationBuilder.DropColumn(name: "Direccion", table: "Proveedores");
            migrationBuilder.DropColumn(name: "Email", table: "Proveedores");
            migrationBuilder.DropColumn(name: "Telefono", table: "Proveedores");
            migrationBuilder.DropColumn(name: "Cuit", table: "Proveedores");
            migrationBuilder.DropColumn(name: "RazonSocial", table: "Proveedores");
        }
    }
}
