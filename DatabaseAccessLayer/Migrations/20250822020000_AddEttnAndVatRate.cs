using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddEttnAndVatRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ettn",
                table: "Invoices",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VatRate",
                table: "LineOfInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ettn",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "VatRate",
                table: "LineOfInvoices");
        }
    }
}