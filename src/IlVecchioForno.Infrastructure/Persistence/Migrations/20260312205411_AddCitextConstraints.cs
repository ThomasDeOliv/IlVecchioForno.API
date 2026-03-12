using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlVecchioForno.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCitextConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "ck_quantity_types_name_maxlength",
                schema: "pizzas_schema",
                table: "quantity_types",
                sql: "LENGTH(name) <= 256");

            migrationBuilder.AddCheckConstraint(
                name: "ck_quantity_types_unit_maxlength",
                schema: "pizzas_schema",
                table: "quantity_types",
                sql: "LENGTH(unit) <= 4");

            migrationBuilder.AddCheckConstraint(
                name: "ck_pizzas_description_maxlength",
                schema: "pizzas_schema",
                table: "pizzas",
                sql: "LENGTH(description) <= 1024");

            migrationBuilder.AddCheckConstraint(
                name: "ck_pizzas_name_maxlength",
                schema: "pizzas_schema",
                table: "pizzas",
                sql: "LENGTH(name) <= 256");

            migrationBuilder.AddCheckConstraint(
                name: "ck_ingredients_name_maxlength",
                schema: "pizzas_schema",
                table: "ingredients",
                sql: "LENGTH(name) <= 256");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_quantity_types_name_maxlength",
                schema: "pizzas_schema",
                table: "quantity_types");

            migrationBuilder.DropCheckConstraint(
                name: "ck_quantity_types_unit_maxlength",
                schema: "pizzas_schema",
                table: "quantity_types");

            migrationBuilder.DropCheckConstraint(
                name: "ck_pizzas_description_maxlength",
                schema: "pizzas_schema",
                table: "pizzas");

            migrationBuilder.DropCheckConstraint(
                name: "ck_pizzas_name_maxlength",
                schema: "pizzas_schema",
                table: "pizzas");

            migrationBuilder.DropCheckConstraint(
                name: "ck_ingredients_name_maxlength",
                schema: "pizzas_schema",
                table: "ingredients");
        }
    }
}
