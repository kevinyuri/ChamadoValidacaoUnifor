using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserValidacaoUnifor.Migrations
{
    public partial class AddChamado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeSolicitante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Setor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamados", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chamados");
        }
    }
}
