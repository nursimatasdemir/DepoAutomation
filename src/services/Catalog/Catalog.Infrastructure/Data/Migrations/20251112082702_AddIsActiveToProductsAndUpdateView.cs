using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToProductsAndUpdateView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: true);
            
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS ""vw_ValidProducts""");

            migrationBuilder.Sql(@"
                CREATE VIEW ""vw_ValidProducts"" AS
                SELECT ""Id"" FROM ""Products""
                WHERE ""IsActive"" = true
            ");
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS""vw_ValidProducts""");
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.Sql(@"
                CREATE VIEW ""vw_ValidProducts"" AS
                SELECT ""Id"" FROM ""Products""
            ");
        }
    }
}
