using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingSystem.EFDataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBankAccounts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBankAccounts_UserId",
                table: "UserBankAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankAccounts_Users_UserId",
                table: "UserBankAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBankAccounts_Users_UserId",
                table: "UserBankAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserBankAccounts_UserId",
                table: "UserBankAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserBankAccounts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
