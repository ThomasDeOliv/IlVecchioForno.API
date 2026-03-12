using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlVecchioForno.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnsToUseCitextType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "unit",
                schema: "pizzas_schema",
                table: "quantity_types",
                type: "CITEXT",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(4)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "quantity_types",
                type: "CITEXT",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(256)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "pizzas",
                type: "CITEXT",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(256)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "pizzas_schema",
                table: "pizzas",
                type: "CITEXT",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(1024)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "ingredients",
                type: "CITEXT",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(256)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "unit",
                schema: "pizzas_schema",
                table: "quantity_types",
                type: "VARCHAR(4)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CITEXT",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "quantity_types",
                type: "VARCHAR(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CITEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "pizzas",
                type: "VARCHAR(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CITEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "pizzas_schema",
                table: "pizzas",
                type: "VARCHAR(1024)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "CITEXT",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "pizzas_schema",
                table: "ingredients",
                type: "VARCHAR(256)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CITEXT",
                oldMaxLength: 256);
        }
    }
}
