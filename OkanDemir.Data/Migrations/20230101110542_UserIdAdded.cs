using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OkanDemir.Data.Migrations
{
    public partial class UserIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoTables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoListTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TodoImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SubscriptionTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "InvoiceTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "IncomeTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CodeNotes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CodeCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeTypeId",
                table: "Incomes",
                column: "IncomeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_IncomeTypes_IncomeTypeId",
                table: "Incomes",
                column: "IncomeTypeId",
                principalTable: "IncomeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_IncomeTypes_IncomeTypeId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_IncomeTypeId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoTables");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoProjects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoListTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SubscriptionTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InvoiceTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncomeTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CodeNotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CodeCategories");
        }
    }
}
