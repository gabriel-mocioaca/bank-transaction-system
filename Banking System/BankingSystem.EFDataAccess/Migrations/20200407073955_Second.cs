using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingSystem.EFDataAccess.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "UserBankAccounts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBankAccounts",
                newName: "CurrencyId");

            migrationBuilder.CreateTable(
                name: "CurrencyType",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Currency = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyType", x => x.CurrencyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBankAccounts_CurrencyId",
                table: "UserBankAccounts",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBankAccounts_CurrencyType_CurrencyId",
                table: "UserBankAccounts",
                column: "CurrencyId",
                principalTable: "CurrencyType",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBankAccounts_CurrencyType_CurrencyId",
                table: "UserBankAccounts");

            migrationBuilder.DropTable(
                name: "CurrencyType");

            migrationBuilder.DropIndex(
                name: "IX_UserBankAccounts_CurrencyId",
                table: "UserBankAccounts");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "UserBankAccounts",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "UserBankAccounts",
                nullable: true);
        }
    }
}
