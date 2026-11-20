using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShoeStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "manufacturer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    country = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_manufacturer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "receipt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    receipt_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shoe",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manufacturer_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    category = table.Column<short>(type: "smallint", nullable: false),
                    seasons = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shoe", x => x.id);
                    table.CheckConstraint("ck_shoe_price_positive", "price > 0");
                    table.ForeignKey(
                        name: "FK_shoe_manufacturer_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "receipt_item",
                columns: table => new
                {
                    receipt_id = table.Column<int>(type: "integer", nullable: false),
                    shoe_id = table.Column<int>(type: "integer", nullable: false),
                    size = table.Column<decimal>(type: "numeric(3,1)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_receipt_item", x => new { x.receipt_id, x.shoe_id, x.size });
                    table.CheckConstraint("ck_receipt_item_quantity_positive", "quantity > 0");
                    table.ForeignKey(
                        name: "FK_receipt_item_receipt_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "receipt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_receipt_item_shoe_shoe_id",
                        column: x => x.shoe_id,
                        principalTable: "shoe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sale_item",
                columns: table => new
                {
                    sale_id = table.Column<int>(type: "integer", nullable: false),
                    shoe_id = table.Column<int>(type: "integer", nullable: false),
                    size = table.Column<decimal>(type: "numeric(3,1)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    sale_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_item", x => new { x.sale_id, x.shoe_id, x.size });
                    table.CheckConstraint("ck_sale_item_price_positive", "sale_price > 0");
                    table.CheckConstraint("ck_sale_item_quantity_positive", "quantity > 0");
                    table.ForeignKey(
                        name: "FK_sale_item_sale_sale_id",
                        column: x => x.sale_id,
                        principalTable: "sale",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_item_shoe_shoe_id",
                        column: x => x.shoe_id,
                        principalTable: "shoe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_manufacturer_name",
                table: "manufacturer",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_receipt_item_shoe_id",
                table: "receipt_item",
                column: "shoe_id");

            migrationBuilder.CreateIndex(
                name: "IX_sale_item_shoe_id",
                table: "sale_item",
                column: "shoe_id");

            migrationBuilder.CreateIndex(
                name: "IX_shoe_manufacturer_id",
                table: "shoe",
                column: "manufacturer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "receipt_item");

            migrationBuilder.DropTable(
                name: "sale_item");

            migrationBuilder.DropTable(
                name: "receipt");

            migrationBuilder.DropTable(
                name: "sale");

            migrationBuilder.DropTable(
                name: "shoe");

            migrationBuilder.DropTable(
                name: "manufacturer");
        }
    }
}
