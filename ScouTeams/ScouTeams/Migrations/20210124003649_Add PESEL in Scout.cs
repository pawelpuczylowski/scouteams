using Microsoft.EntityFrameworkCore.Migrations;

namespace ScouTeams.Migrations
{
    public partial class AddPESELinScout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChoragiew_Choragiews_ChoragiewId",
                table: "UserChoragiew");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChoragiew_AspNetUsers_ScoutId",
                table: "UserChoragiew");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDruzyna_Druzynas_DruzynaId",
                table: "UserDruzyna");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDruzyna_AspNetUsers_ScoutId",
                table: "UserDruzyna");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHufiec_Hufiecs_HufiecId",
                table: "UserHufiec");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHufiec_AspNetUsers_ScoutId",
                table: "UserHufiec");

            migrationBuilder.DropForeignKey(
                name: "FK_UserZastep_AspNetUsers_ScoutId",
                table: "UserZastep");

            migrationBuilder.DropForeignKey(
                name: "FK_UserZastep_Zastep_ZastepId",
                table: "UserZastep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserZastep",
                table: "UserZastep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHufiec",
                table: "UserHufiec");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDruzyna",
                table: "UserDruzyna");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChoragiew",
                table: "UserChoragiew");

            migrationBuilder.RenameTable(
                name: "UserZastep",
                newName: "UserZasteps");

            migrationBuilder.RenameTable(
                name: "UserHufiec",
                newName: "UserHufiecs");

            migrationBuilder.RenameTable(
                name: "UserDruzyna",
                newName: "UserDruzynas");

            migrationBuilder.RenameTable(
                name: "UserChoragiew",
                newName: "UserChoragiews");

            migrationBuilder.RenameIndex(
                name: "IX_UserZastep_ZastepId",
                table: "UserZasteps",
                newName: "IX_UserZasteps_ZastepId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHufiec_HufiecId",
                table: "UserHufiecs",
                newName: "IX_UserHufiecs_HufiecId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDruzyna_DruzynaId",
                table: "UserDruzynas",
                newName: "IX_UserDruzynas_DruzynaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChoragiew_ChoragiewId",
                table: "UserChoragiews",
                newName: "IX_UserChoragiews_ChoragiewId");

            migrationBuilder.AddColumn<string>(
                name: "PESEL",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserZasteps",
                table: "UserZasteps",
                columns: new[] { "ScoutId", "ZastepId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHufiecs",
                table: "UserHufiecs",
                columns: new[] { "ScoutId", "HufiecId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDruzynas",
                table: "UserDruzynas",
                columns: new[] { "ScoutId", "DruzynaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChoragiews",
                table: "UserChoragiews",
                columns: new[] { "ScoutId", "ChoragiewId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserChoragiews_Choragiews_ChoragiewId",
                table: "UserChoragiews",
                column: "ChoragiewId",
                principalTable: "Choragiews",
                principalColumn: "ChoragiewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChoragiews_AspNetUsers_ScoutId",
                table: "UserChoragiews",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDruzynas_Druzynas_DruzynaId",
                table: "UserDruzynas",
                column: "DruzynaId",
                principalTable: "Druzynas",
                principalColumn: "HufiecId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDruzynas_AspNetUsers_ScoutId",
                table: "UserDruzynas",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHufiecs_Hufiecs_HufiecId",
                table: "UserHufiecs",
                column: "HufiecId",
                principalTable: "Hufiecs",
                principalColumn: "HufiecId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHufiecs_AspNetUsers_ScoutId",
                table: "UserHufiecs",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserZasteps_AspNetUsers_ScoutId",
                table: "UserZasteps",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserZasteps_Zastep_ZastepId",
                table: "UserZasteps",
                column: "ZastepId",
                principalTable: "Zastep",
                principalColumn: "ZastepId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChoragiews_Choragiews_ChoragiewId",
                table: "UserChoragiews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChoragiews_AspNetUsers_ScoutId",
                table: "UserChoragiews");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDruzynas_Druzynas_DruzynaId",
                table: "UserDruzynas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDruzynas_AspNetUsers_ScoutId",
                table: "UserDruzynas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHufiecs_Hufiecs_HufiecId",
                table: "UserHufiecs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHufiecs_AspNetUsers_ScoutId",
                table: "UserHufiecs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserZasteps_AspNetUsers_ScoutId",
                table: "UserZasteps");

            migrationBuilder.DropForeignKey(
                name: "FK_UserZasteps_Zastep_ZastepId",
                table: "UserZasteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserZasteps",
                table: "UserZasteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHufiecs",
                table: "UserHufiecs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDruzynas",
                table: "UserDruzynas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChoragiews",
                table: "UserChoragiews");

            migrationBuilder.DropColumn(
                name: "PESEL",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "UserZasteps",
                newName: "UserZastep");

            migrationBuilder.RenameTable(
                name: "UserHufiecs",
                newName: "UserHufiec");

            migrationBuilder.RenameTable(
                name: "UserDruzynas",
                newName: "UserDruzyna");

            migrationBuilder.RenameTable(
                name: "UserChoragiews",
                newName: "UserChoragiew");

            migrationBuilder.RenameIndex(
                name: "IX_UserZasteps_ZastepId",
                table: "UserZastep",
                newName: "IX_UserZastep_ZastepId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHufiecs_HufiecId",
                table: "UserHufiec",
                newName: "IX_UserHufiec_HufiecId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDruzynas_DruzynaId",
                table: "UserDruzyna",
                newName: "IX_UserDruzyna_DruzynaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChoragiews_ChoragiewId",
                table: "UserChoragiew",
                newName: "IX_UserChoragiew_ChoragiewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserZastep",
                table: "UserZastep",
                columns: new[] { "ScoutId", "ZastepId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHufiec",
                table: "UserHufiec",
                columns: new[] { "ScoutId", "HufiecId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDruzyna",
                table: "UserDruzyna",
                columns: new[] { "ScoutId", "DruzynaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChoragiew",
                table: "UserChoragiew",
                columns: new[] { "ScoutId", "ChoragiewId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserChoragiew_Choragiews_ChoragiewId",
                table: "UserChoragiew",
                column: "ChoragiewId",
                principalTable: "Choragiews",
                principalColumn: "ChoragiewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChoragiew_AspNetUsers_ScoutId",
                table: "UserChoragiew",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDruzyna_Druzynas_DruzynaId",
                table: "UserDruzyna",
                column: "DruzynaId",
                principalTable: "Druzynas",
                principalColumn: "HufiecId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDruzyna_AspNetUsers_ScoutId",
                table: "UserDruzyna",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHufiec_Hufiecs_HufiecId",
                table: "UserHufiec",
                column: "HufiecId",
                principalTable: "Hufiecs",
                principalColumn: "HufiecId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHufiec_AspNetUsers_ScoutId",
                table: "UserHufiec",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserZastep_AspNetUsers_ScoutId",
                table: "UserZastep",
                column: "ScoutId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserZastep_Zastep_ZastepId",
                table: "UserZastep",
                column: "ZastepId",
                principalTable: "Zastep",
                principalColumn: "ZastepId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
