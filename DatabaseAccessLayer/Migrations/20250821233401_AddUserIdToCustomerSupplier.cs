using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCustomerSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCredentials_Email",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserCredentials");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "UserCredentials",
                newName: "UpdatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserCredentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserCredentials",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductsAndServices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ProductsAndServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CustomersSuppliers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsAndServices_UserId",
                table: "ProductsAndServices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersSuppliers_UserId",
                table: "CustomersSuppliers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersSuppliers_Users_UserId",
                table: "CustomersSuppliers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsAndServices_Users_UserId",
                table: "ProductsAndServices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersSuppliers_Users_UserId",
                table: "CustomersSuppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsAndServices_Users_UserId",
                table: "ProductsAndServices");

            migrationBuilder.DropIndex(
                name: "IX_ProductsAndServices_UserId",
                table: "ProductsAndServices");

            migrationBuilder.DropIndex(
                name: "IX_CustomersSuppliers_UserId",
                table: "CustomersSuppliers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserCredentials");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductsAndServices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomersSuppliers");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "UserCredentials",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserCredentials",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserCredentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductsAndServices",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_Email",
                table: "UserCredentials",
                column: "Email");
        }
    }
}
