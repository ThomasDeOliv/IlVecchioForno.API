using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence;
using IlVecchioForno.Infrastructure.Tests.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests;

[Collection("Database")]
public class DbContextTests : IAsyncLifetime
{
    // Schemas
    private const string PizzasDbSchema = "pizzas_schema";

    // Tables
    private const string PizzasTable = "pizzas";
    private const string PizzasIngredientsTable = "pizzas_ingredients";
    private const string IngredientsTable = "ingredients";
    private const string QuantityTypesTable = "quantity_types";

    private readonly DbFixture _fixture;
    private IlVecchioFornoDbContext _context;

    public DbContextTests(DbFixture fixture)
    {
        this._fixture = fixture;
        this._context = null!;
    }

    public static TheoryData<string, string, int> TablesAndRelatedVarcharColumnsLength =>
        new TheoryData<string, string, int>
        {
            {
                PizzasTable,
                "name",
                PizzaInvariant.NameMaxLength
            },
            {
                PizzasTable,
                "description",
                PizzaInvariant.DescriptionMaxLength
            },
            {
                IngredientsTable,
                "name",
                IngredientInvariant.NameMaxLength
            },
            {
                QuantityTypesTable,
                "name",
                QuantityTypeInvariant.NameMaxLength
            },
            {
                QuantityTypesTable,
                "unit",
                QuantityTypeInvariant.UnitMaxLength
            }
        };

    public static TheoryData<string, string, int, int> TablesAndRelatedNumericColumnsType =>
        new TheoryData<string, string, int, int>
        {
            {
                PizzasTable,
                "price",
                6,
                2
            },
            {
                PizzasIngredientsTable,
                "quantity",
                9,
                3
            }
        };

    // Test data
    public static TheoryData<string, ColumnInfo[]> TablesAndRelatedColumns =>
        new TheoryData<string, ColumnInfo[]>
        {
            {
                PizzasTable,
                [
                    new ColumnInfo("id", "integer", false),
                    new ColumnInfo("name", "character varying", false),
                    new ColumnInfo("description", "character varying", true),
                    new ColumnInfo("archived", "timestamp with time zone", true),
                    new ColumnInfo("price", "numeric", false),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                PizzasIngredientsTable,
                [
                    new ColumnInfo("quantity", "numeric", false),
                    new ColumnInfo("pizza_id", "integer", false),
                    new ColumnInfo("ingredient_id", "integer", false),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                IngredientsTable,
                [
                    new ColumnInfo("id", "integer", false),
                    new ColumnInfo("name", "character varying", false),
                    new ColumnInfo("quantity_type_id", "smallint", false),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            },
            {
                QuantityTypesTable,
                [
                    new ColumnInfo("id", "smallint", false),
                    new ColumnInfo("name", "character varying", false),
                    new ColumnInfo("unit", "character varying", true),
                    new ColumnInfo("created_at", "timestamp with time zone", false),
                    new ColumnInfo("updated_at", "timestamp with time zone", false)
                ]
            }
        };

    public Task InitializeAsync()
    {
        string dbName = GetRandomDbName();
        this._context = this._fixture.CreateTestDbContext(dbName);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await this._context.DisposeAsync();
        this._context = null!;
    }

    private static string GetRandomDbName()
    {
        return $"test_{Guid.NewGuid():N}";
    }

    [Fact]
    public async Task Database_CanBeCreated()
    {
        // Arrange & Act
        bool created = await this._context.Database.EnsureCreatedAsync();
        // Assert
        Assert.True(created);
    }

    [Fact]
    public async Task Migrations_ApplySuccessfully()
    {
        // Arrange & Act
        await this._context.Database.MigrateAsync();
        IEnumerable<string> pendingMigrations = await this._context.Database.GetPendingMigrationsAsync();
        IEnumerable<string> appliedMigrations = await this._context.Database.GetAppliedMigrationsAsync();
        // Assert
        Assert.Empty(pendingMigrations);
        Assert.NotEmpty(appliedMigrations);
    }

    [Fact]
    public async Task Schema_ExistAfterMigration()
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        List<string> schemas = await this._context.Database
            .SqlQuery<string>(
                $"""
                  SELECT schema_name
                  FROM information_schema.schemata
                 """
            )
            .ToListAsync();
        // Assert
        Assert.NotEmpty(schemas);
        Assert.Contains(
            PizzasDbSchema,
            schemas
        );
    }

    [Fact]
    public async Task Tables_ExistAfterMigration()
    {
        // Arrange
        List<string> expectedTables = new List<string>
        {
            PizzasTable,
            PizzasIngredientsTable,
            IngredientsTable,
            QuantityTypesTable
        };
        await this._context.Database.MigrateAsync();
        // Act
        List<string> tables = await this._context.Database
            .SqlQuery<string>(
                $"""
                 SELECT table_name
                 FROM information_schema.tables
                 WHERE table_schema = {PizzasDbSchema}
                 """
            )
            .ToListAsync();
        // Assert
        Assert.NotEmpty(tables);
        Assert.Equivalent(
            expectedTables,
            tables
        );
    }

    [Theory]
    [MemberData(nameof(TablesAndRelatedColumns))]
    public async Task Tables_ContainsFields(string tableName, ColumnInfo[] expectedColumns)
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        List<ColumnInfo> columns = await this._context.Database
            .SqlQuery<ColumnInfo>(
                $"""
                  SELECT 
                      column_name AS "ColumnName",
                      data_type AS "DataType",
                      is_nullable = 'YES' AS "IsNullable"
                  FROM information_schema.columns
                  WHERE table_schema = {PizzasDbSchema}
                      AND table_name = {tableName}
                 """
            )
            .ToListAsync();
        // Assert
        Assert.NotEmpty(columns);
        Assert.Equal(expectedColumns.Length, columns.Count);
        Assert.Equivalent(expectedColumns, columns);
    }

    [Theory]
    [MemberData(nameof(TablesAndRelatedVarcharColumnsLength))]
    public async Task Table_VarcharColumns_HaveExpectedLength(string tableName, string columnName, int expectedLength)
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        VarcharColumnInfo? info = await this._context.Database
            .SqlQuery<VarcharColumnInfo>(
                $"""
                  SELECT 
                      character_maximum_length AS "MaxLength"
                  FROM information_schema.columns
                  WHERE table_schema = {PizzasDbSchema}
                      AND table_name = {tableName}
                      AND column_name = {columnName}
                 """
            )
            .SingleOrDefaultAsync();
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedLength, info.MaxLength);
    }

    [Theory]
    [MemberData(nameof(TablesAndRelatedNumericColumnsType))]
    public async Task Table_NumericColumns_HaveExpectedLengthAndScale(string tableName, string columnName,
        int expectedLength, int expectedScale)
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        NumericColumnInfo? info = await this._context.Database
            .SqlQuery<NumericColumnInfo>(
                $"""
                  SELECT 
                      numeric_precision AS "Precision", 
                      numeric_scale AS "Scale"
                  FROM information_schema.columns
                  WHERE table_schema = {PizzasDbSchema}
                      AND table_name = {tableName}
                      AND column_name = {columnName}
                 """
            )
            .SingleOrDefaultAsync();
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedLength, info.Precision);
        Assert.Equal(expectedScale, info.Scale);
    }
}