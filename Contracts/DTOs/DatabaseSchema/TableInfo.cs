namespace Contracts.DTOs.DatabaseSchema;

public class TableInfo
{
    public string Schema { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public List<ColumnInfo> Columns { get; set; } = new();
}
