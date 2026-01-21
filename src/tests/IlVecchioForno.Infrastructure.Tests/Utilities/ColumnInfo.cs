using Xunit.Abstractions;

namespace IlVecchioForno.Infrastructure.Tests.Utilities;

public record ColumnInfo : IXunitSerializable
{
    public ColumnInfo()
    {
        this.ColumnName = string.Empty;
        this.DataType = string.Empty;
        this.IsNullable = true;
    }

    public ColumnInfo(string columnName, string dataType, bool isNullable) : this()
    {
        this.ColumnName = columnName;
        this.DataType = dataType;
        this.IsNullable = isNullable;
    }

    public string ColumnName { get; private set; }
    public string DataType { get; private set; }
    public bool IsNullable { get; private set; }

    public void Deserialize(IXunitSerializationInfo info)
    {
        this.ColumnName = info.GetValue<string>(nameof(this.ColumnName));
        this.DataType = info.GetValue<string>(nameof(this.DataType));
        this.IsNullable = info.GetValue<bool>(nameof(this.IsNullable));
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(this.ColumnName), this.ColumnName);
        info.AddValue(nameof(this.DataType), this.DataType);
        info.AddValue(nameof(this.IsNullable), this.IsNullable);
    }
}