using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class RemovePrecision : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "Price_Amount",
            schema: "eShop",
            table: "Products",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(2,2)",
            oldPrecision: 2,
            oldScale: 2);

        migrationBuilder.AlterColumn<decimal>(
            name: "Price_Amount",
            schema: "eShop",
            table: "LineItems",
            type: "decimal(18,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(2,2)",
            oldPrecision: 2,
            oldScale: 2);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "Price_Amount",
            schema: "eShop",
            table: "Products",
            type: "decimal(2,2)",
            precision: 2,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");

        migrationBuilder.AlterColumn<decimal>(
            name: "Price_Amount",
            schema: "eShop",
            table: "LineItems",
            type: "decimal(2,2)",
            precision: 2,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)");
    }
}
