namespace IlVecchioForno.Infrastructure.Tests.Utilities.Models;

public record ForeignKeyConstraintColumnInfo(
    string ConstraintName,
    string ReferencedTable,
    string ReferencedColumn,
    string DeleteRule
);