using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserValidacaoUnifor.Migrations
{
    public partial class StatusChamado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Chamados");
        }
    }
}
