using Microsoft.EntityFrameworkCore.Migrations;

namespace ScouTeams.Migrations
{
    public partial class InitialDruzynas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Hufiecs",
               columns: table => new
               {
                   DruzynaId = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name = table.Column<string>(nullable: true),
                   HufiecId = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Druzynas", x => x.DruzynaId);
                   table.ForeignKey(
                       name: "FK_Druzynas_Hufiecs_HufiecId",
                       column: x => x.HufiecId,
                       principalTable: "Hufiecs",
                       principalColumn: "HufiecId",
                       onDelete: ReferentialAction.Cascade);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Druzynas");
        }
    }
}
