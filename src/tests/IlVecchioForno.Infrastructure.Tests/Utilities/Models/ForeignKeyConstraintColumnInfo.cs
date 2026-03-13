namespace IlVecchioForno.Infrastructure.Tests.Utilities.Models;

public sealed record ForeignKeyConstraintColumnInfo(
    string ConstraintName,
    string ReferencedTable,
    string ReferencedColumn,
    string DeleteRule
);