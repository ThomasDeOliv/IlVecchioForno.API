using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using IlVecchioForno.Infrastructure.Tests.Utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.Database;

public sealed class DatabaseTests : EmptyInfrastructureTestsBase
{
    public DatabaseTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
    }

    [Fact]
    public async Task Migrations_ApplySuccessfully()
    {
        // Arrange & Act
        IEnumerable<string> pendingMigrations = await this._ctx.Database.GetPendingMigrationsAsync(TestContext.Current.CancellationToken);
        IEnumerable<string> appliedMigrations = await this._ctx.Database.GetAppliedMigrationsAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Empty(pendingMigrations);
        Assert.NotEmpty(appliedMigrations);
    }

    [Fact]
    public async Task Schema_ExistsAfterMigration()
    {
        // Arrange & Act
        List<string> schemas = await this._ctx.Database
            .SqlQuery<string>(
                $"""
                  SELECT schema_name
                  FROM information_schema.schemata
                 """
            )
            .ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotEmpty(schemas);
        Assert.Contains(
            DbDescriptionTestsData.PizzasDbSchema,
            schemas
        );
    }

    [Fact]
    public async Task Tables_ExistAfterMigration()
    {
        // Arrange
        List<string> expectedTables = new List<string>
        {
            DbDescriptionTestsData.PizzasTable,
            DbDescriptionTestsData.PizzasIngredientsTable,
            DbDescriptionTestsData.IngredientsTable,
            DbDescriptionTestsData.QuantityTypesTable
        };
        // Act
        List<string> tables = await this._ctx.Database
            .SqlQuery<string>(
                $"""
                 SELECT table_name
                 FROM information_schema.tables
                 WHERE table_schema = {DbDescriptionTestsData.PizzasDbSchema}
                 """
            )
            .ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotEmpty(tables);
        Assert.Equivalent(
            expectedTables,
            tables
        );
    }

    [Theory]
    [MemberData(nameof(DbDescriptionTestsData.TablesAndRelatedColumns), MemberType = typeof(DbDescriptionTestsData))]
    public async Task Table_ContainsExpectedFields(
        string tableName,
        ColumnInfo[] expectedColumns
    )
    {
        // Arrange & Act
        List<ColumnInfo> columns = await this._ctx.Database
            .SqlQuery<ColumnInfo>(
                $"""
                  SELECT 
                      column_name AS "ColumnName",
                      data_type AS "DataType",
                      is_nullable = 'YES' AS "IsNullable"
                  FROM information_schema.columns
                  WHERE table_schema = {DbDescriptionTestsData.PizzasDbSchema}
                      AND table_name = {tableName}
                 """
            )
            .ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotEmpty(columns);
        Assert.Equal(expectedColumns.Length, columns.Count);
        Assert.Equivalent(expectedColumns, columns);
    }

    [Theory]
    [MemberData(nameof(DbDescriptionTestsData.TablesAndRelatedVarcharColumnsLength), MemberType = typeof(DbDescriptionTestsData))]
    public async Task Table_VarcharColumns_HaveExpectedLength(
        string tableName,
        string columnName,
        int expectedLength
    )
    {
        // Arrange
        await this._ctx.Database.MigrateAsync(TestContext.Current.CancellationToken);
        // Act
        VarcharColumnInfo? info = await this._ctx.Database
            .SqlQuery<VarcharColumnInfo>(
                $"""
                  SELECT 
                      character_maximum_length AS "MaxLength"
                  FROM information_schema.columns
                  WHERE table_schema = {DbDescriptionTestsData.PizzasDbSchema}
                      AND table_name = {tableName}
                      AND column_name = {columnName}
                 """
            )
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedLength, info.MaxLength);
    }

    [Theory]
    [MemberData(nameof(DbDescriptionTestsData.TablesAndNumericColumnsPrecision), MemberType = typeof(DbDescriptionTestsData))]
    public async Task Table_NumericColumns_HaveExpectedPrecisionAndScale(
        string tableName,
        string columnName,
        int expectedLength,
        int expectedScale
    )
    {
        // Arrange & Act
        NumericColumnInfo? info = await this._ctx.Database
            .SqlQuery<NumericColumnInfo>(
                $"""
                  SELECT 
                      numeric_precision AS "Precision", 
                      numeric_scale AS "Scale"
                  FROM information_schema.columns
                  WHERE table_schema = {DbDescriptionTestsData.PizzasDbSchema}
                      AND table_name = {tableName}
                      AND column_name = {columnName}
                 """
            )
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedLength, info.Precision);
        Assert.Equal(expectedScale, info.Scale);
    }

    [Theory]
    [MemberData(nameof(DbDescriptionTestsData.TablesAndExpectedPrimaryKeys), MemberType = typeof(DbDescriptionTestsData))]
    public async Task Table_PrimaryKey_HasExpectedConstraint(
        string tableName,
        string expectedConstraintName,
        string[] expectedColumnNames
    )
    {
        // Arrange
        string expectedColumnNamesString = string.Join(", ", expectedColumnNames);
        // Act
        PrimaryKeyConstraintColumnsInfo? info = await this._ctx.Database
            .SqlQuery<PrimaryKeyConstraintColumnsInfo>(
                $"""
                 SELECT                                                                                                                                                                                                                                                                                                                                                                                                                          
                     tc.constraint_name AS "ConstraintName",                                                                                                                                                                                                                                                                                                                                                                                     
                     STRING_AGG(kcu.column_name, ', ') AS "ColumnNames"                                                                                                                                                                                                                                                                                                                                                                          
                 FROM information_schema.table_constraints tc                                                                                                                                                                                                                                                                                                                                                                                    
                 JOIN information_schema.key_column_usage kcu                                                                                                                                                                                                                                                                                                                                                                                    
                     ON tc.constraint_name = kcu.constraint_name                                                                                                                                                                                                                                                                                                                                                                                 
                 WHERE tc.table_schema = {DbDescriptionTestsData.PizzasDbSchema}                                                                                                                                                                                                                                                                                                                                                                                        
                     AND tc.table_name = {tableName}                                                                                                                                                                                                                                                                                                                                                                                             
                     AND tc.constraint_type = 'PRIMARY KEY'                                                                                                                                                                                                                                                                                                                                                                                      
                 GROUP BY tc.constraint_name               
                 """
            )
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedConstraintName, info.ConstraintName);
        Assert.Equal(expectedColumnNamesString, info.ColumnNames);
    }

    [Theory]
    [MemberData(nameof(DbDescriptionTestsData.TablesAndExpectedForeignKeys), MemberType = typeof(DbDescriptionTestsData))]
    public async Task Table_ForeignKeyColumns_HaveExpectedConstraint(
        string tableName,
        string columnName,
        string expectedConstraintName,
        string expectedReferencedTableName,
        string expectedReferencedColumnName,
        string expectedReferencedDeleteRule
    )
    {
        // Arrange & Act
        ForeignKeyConstraintColumnInfo? info = await this._ctx.Database
            .SqlQuery<ForeignKeyConstraintColumnInfo>(
                $"""
                 SELECT                                                                                                                                                                                                                                                                                                                                                                                                                          
                      tc.constraint_name AS "ConstraintName",                                                                                                                                                                                                                                                                                                                                                                                     
                      ccu.table_name AS "ReferencedTable",                                                                                                                                                                                                                                                                                                                                                                                        
                      ccu.column_name AS "ReferencedColumn",
                      rc.delete_rule AS "DeleteRule" 
                  FROM information_schema.table_constraints tc                                                                                                                                                                                                                                                                                                                                                                                    
                  JOIN information_schema.key_column_usage kcu                                                                                                                                                                                                                                                                                                                                                                                    
                      ON tc.constraint_name = kcu.constraint_name                                                                                                                                                                                                                                                                                                                                                                                 
                  JOIN information_schema.constraint_column_usage ccu                                                                                                                                                                                                                                                                                                                                                                             
                      ON tc.constraint_name = ccu.constraint_name      
                  JOIN information_schema.referential_constraints rc                                                                                                                                                                                                                                                                                                                                                                              
                      ON tc.constraint_name = rc.constraint_name  
                  WHERE tc.table_schema = {DbDescriptionTestsData.PizzasDbSchema}                                                                                                                                                                                                                                                                                                                                                                                        
                      AND tc.table_name = {tableName}                                                                                                                                                                                                                                                                                                                                                                                             
                      AND kcu.column_name = {columnName}                                                                                                                                                                                                                                                                                                                                                                                          
                      AND tc.constraint_type = 'FOREIGN KEY'               
                 """
            )
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.NotNull(info);
        Assert.Equal(expectedConstraintName, info.ConstraintName);
        Assert.Equal(expectedReferencedTableName, info.ReferencedTable);
        Assert.Equal(expectedReferencedColumnName, info.ReferencedColumn);
        Assert.Equal(expectedReferencedDeleteRule, info.DeleteRule);
    }
}