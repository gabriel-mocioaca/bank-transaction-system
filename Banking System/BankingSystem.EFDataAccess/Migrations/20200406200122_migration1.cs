using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingSystem.EFDataAccess.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBankAccounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBankAccounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "UserTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromAccountId = table.Column<int>(nullable: false),
                    ToAccountId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    CurrencyRate = table.Column<decimal>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_UserTransactions_UserBankAccounts_FromAccountId",
                        column: x => x.FromAccountId,
                        principalTable: "UserBankAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTransactions_UserBankAccounts_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "UserBankAccounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_FromAccountId",
                table: "UserTransactions",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_ToAccountId",
                table: "UserTransactions",
                column: "ToAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTransactions");

            migrationBuilder.DropTable(
                name: "UserBankAccounts");
        }
    }
}
