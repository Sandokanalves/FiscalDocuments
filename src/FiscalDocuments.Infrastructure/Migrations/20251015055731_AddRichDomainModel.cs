using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiscalDocuments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRichDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientCnpj",
                table: "FiscalDocuments");

            migrationBuilder.AddColumn<int>(
                name: "DocumentNumber",
                table: "FiscalDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IssuerCityCode",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerCityName",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerDistrict",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerName",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerNumber",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerState",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerStreet",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IssuerZipCode",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Model",
                table: "FiscalDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecipientCityCode",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientCityName",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientDistrict",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientDocument",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientNumber",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientState",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientStreet",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientZipCode",
                table: "FiscalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Series",
                table: "FiscalDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalProducts",
                table: "FiscalDocuments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ncm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cfop = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FiscalDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItem_FiscalDocuments_FiscalDocumentId",
                        column: x => x.FiscalDocumentId,
                        principalTable: "FiscalDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItem_FiscalDocumentId",
                table: "ProductItem",
                column: "FiscalDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductItem");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerCityCode",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerCityName",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerDistrict",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerName",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerNumber",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerState",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerStreet",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "IssuerZipCode",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientCityCode",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientCityName",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientDistrict",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientDocument",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientNumber",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientState",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientStreet",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "RecipientZipCode",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "FiscalDocuments");

            migrationBuilder.DropColumn(
                name: "TotalProducts",
                table: "FiscalDocuments");

            migrationBuilder.AddColumn<string>(
                name: "RecipientCnpj",
                table: "FiscalDocuments",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }
    }
}
