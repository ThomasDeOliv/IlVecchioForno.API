namespace IlVecchioForno.Infrastructure.Tests.Utilities;

public record ForeignKeyConstraintColumnInfo(
    string ConstraintName,
    string ReferencedTable,
    string ReferencedColumn,
    string DeleteRule
);