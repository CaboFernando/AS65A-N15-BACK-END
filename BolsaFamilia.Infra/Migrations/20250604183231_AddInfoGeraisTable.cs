using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolsaFamilia.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoGeraisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoGerais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValorBaseRendaPerCapita = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiposParentescoPermitidos = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoGerais", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoGerais");
        }
    }
}
