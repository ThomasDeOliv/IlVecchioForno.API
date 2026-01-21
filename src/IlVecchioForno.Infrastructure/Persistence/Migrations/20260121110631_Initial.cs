using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IlVecchioForno.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pizzas_schema");

            migrationBuilder.CreateTable(
                name: "pizzas",
                schema: "pizzas_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    description = table.Column<string>(type: "VARCHAR(1024)", nullable: true),
                    price = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    archived = table.Column<DateTimeOffset>(type: "TIMESTAMPTZ", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pizzas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quantity_types",
                schema: "pizzas_schema",
                columns: table => new
                {
                    id = table.Column<short>(type: "SMALLINT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    unit = table.Column<string>(type: "VARCHAR(4)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quantity_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                schema: "pizzas_schema",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "VARCHAR(256)", nullable: false),
                    quantity_type_id = table.Column<short>(type: "SMALLINT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredients", x => x.id);
                    table.ForeignKey(
                        name: "fk_ingredients__quantity_types",
                        column: x => x.quantity_type_id,
                        principalSchema: "pizzas_schema",
                        principalTable: "quantity_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pizzas_ingredients",
                schema: "pizzas_schema",
                columns: table => new
                {
                    pizza_id = table.Column<int>(type: "INTEGER", nullable: false),
                    ingredient_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(9,3)", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pizzas_ingredients", x => new { x.pizza_id, x.ingredient_id });
                    table.ForeignKey(
                        name: "fk_pizzas_ingredients__ingredients",
                        column: x => x.ingredient_id,
                        principalSchema: "pizzas_schema",
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pizzas_ingredients__pizzas",
                        column: x => x.pizza_id,
                        principalSchema: "pizzas_schema",
                        principalTable: "pizzas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_quantity_type_id",
                schema: "pizzas_schema",
                table: "ingredients",
                column: "quantity_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_pizzas_ingredients_ingredient_id",
                schema: "pizzas_schema",
                table: "pizzas_ingredients",
                column: "ingredient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pizzas_ingredients",
                schema: "pizzas_schema");

            migrationBuilder.DropTable(
                name: "ingredients",
                schema: "pizzas_schema");

            migrationBuilder.DropTable(
                name: "pizzas",
                schema: "pizzas_schema");

            migrationBuilder.DropTable(
                name: "quantity_types",
                schema: "pizzas_schema");
        }
    }
}
