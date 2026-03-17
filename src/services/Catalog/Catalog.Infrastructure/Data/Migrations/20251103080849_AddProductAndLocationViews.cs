using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndLocationViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW ""vw_ValidProducts"" AS
                SELECT ""Id"" FROM ""Products""
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW ""vw_ValidLocations"" AS
                SELECT ""Id"" FROM ""Locations""
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW ""vw_ValidProducts""");
            migrationBuilder.Sql(@"DROP VIEW ""vw_ValidLocations""");
        }
    }
}
