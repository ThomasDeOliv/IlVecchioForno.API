using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Tests.Utilities.Models;

namespace IlVecchioForno.Infrastructure.Tests.Utilities.Data;

public static class DbDescriptionTestsData
{
    public const string PizzasDbSchema = "pizzas_schema";

    public const string PizzasTable = "pizzas";
    public const string PizzasIngredientsTable = "pizzas_ingredients";
    public const string IngredientsTable = "ingredients";
    public const string QuantityTypesTable = "quantity_types";

    public static TheoryData<string, string, string> TablesAndRelatedVarcharColumnsLength =>
        new TheoryData<string, string, string>
        {
            {
                PizzasTable, "ck_pizzas_name_maxlength", $"((length((name)::text) >= {PizzaInvariant.NameMinLength}) AND (length((name)::text) <= {PizzaInvariant.NameMaxLength}))"
            },
            {
                PizzasTable, "ck_pizzas_description_maxlength", $"((description IS NULL) OR ((length((description)::text) >= {PizzaInvariant.DescriptionMinLength}) AND (length((description)::text) <= {PizzaInvariant.DescriptionMaxLength})))"
            },
            {
                IngredientsTable, "ck_ingredients_name_maxlength", $"((length((name)::text) >= {IngredientInvariant.NameMinLength}) AND (length((name)::text) <= {IngredientInvariant.NameMaxLength}))"
            },
            {
                QuantityTypesTable, "ck_quantity_types_name_maxlength", $"((length((name)::text) >= {QuantityTypeInvariant.NameMinLength}) AND (length((name)::text) <= {QuantityTypeInvariant.NameMaxLength}))"
            },
            {
                QuantityTypesTable, "ck_quantity_types_unit_maxlength", $"((length((unit)::text) >= {QuantityTypeInvariant.UnitMinLength}) AND (length((unit)::text) <= {QuantityTypeInvariant.UnitMaxLength}))"
            }
        };

    public static TheoryData<string, string, int, int> TablesAndNumericColumnsPrecision =>
        new TheoryData<string, string, int, int>
        {
            {
                PizzasTable, "price", 6, 2
            },
            {
                PizzasIngredientsTable, "quantity", 9, 3
            }
        };

    // Test data
    public static TheoryData<string, ColumnInfo[]> TablesAndRelatedColumns =>
        new TheoryData<string, ColumnInfo[]>
        {
            {
                PizzasTable, [
                    new ColumnInfo("id", "integer", false),
                    new ColumnInfo("name", "USER-DEFINED", false),
                    new ColumnInfo("description", "USER-DEFINED", true),
                    new ColumnInfo("price", "numeric", false),
                    new ColumnInfo("archived_at", "timestamp with time zone", true),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                PizzasIngredientsTable, [
                    new ColumnInfo("quantity", "numeric", false),
                    new ColumnInfo("pizza_id", "integer", false),
                    new ColumnInfo("ingredient_id", "integer", false),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                IngredientsTable, [
                    new ColumnInfo("id", "integer", false),
                    new ColumnInfo("name", "USER-DEFINED", false),
                    new ColumnInfo("quantity_type_id", "smallint", true),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                QuantityTypesTable, [
                    new ColumnInfo("id", "smallint", false),
                    new ColumnInfo("name", "USER-DEFINED", false),
                    new ColumnInfo("unit", "USER-DEFINED", false),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            }
        };

    public static TheoryData<string, string, string[]> TablesAndExpectedPrimaryKeys =>
        new TheoryData<string, string, string[]>
        {
            {
                IngredientsTable, "pk_ingredients", ["id"]
            },
            {
                PizzasTable, "pk_pizzas", ["id"]
            },
            {
                QuantityTypesTable, "pk_quantity_types", ["id"]
            },
            {
                PizzasIngredientsTable, "pk_pizzas_ingredients", ["ingredient_id", "pizza_id"]
            }
        };

    public static TheoryData<string, string, string, string, string, string> TablesAndExpectedForeignKeys =>
        new TheoryData<string, string, string, string, string, string>
        {
            {
                IngredientsTable, "quantity_type_id", "fk_ingredients__quantity_types", QuantityTypesTable, "id", "RESTRICT"
            },
            {
                PizzasIngredientsTable, "pizza_id", "fk_pizzas_ingredients__pizzas", PizzasTable, "id", "CASCADE"
            },
            {
                PizzasIngredientsTable, "ingredient_id", "fk_pizzas_ingredients__ingredients", IngredientsTable, "id", "RESTRICT"
            }
        };
}