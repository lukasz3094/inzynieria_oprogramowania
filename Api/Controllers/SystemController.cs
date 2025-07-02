using Contracts.DTOs.DatabaseSchema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Api.Controllers;

[ApiController]
[Route("api/system")]
public class DatabaseSchemaController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DatabaseSchemaController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetDatabaseSchema()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        var result = new List<TableInfo>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var tablesCommand = new SqlCommand(
                @"SELECT TABLE_SCHEMA, TABLE_NAME 
                  FROM INFORMATION_SCHEMA.TABLES 
                  WHERE TABLE_TYPE = 'BASE TABLE'", connection);

            using (var tablesReader = tablesCommand.ExecuteReader())
            {
                while (tablesReader.Read())
                {
                    var schema = tablesReader.GetString(0);
                    var table = tablesReader.GetString(1);

                    result.Add(new TableInfo
                    {
                        Schema = schema,
                        TableName = table,
                        Columns = new List<ColumnInfo>()
                    });
                }
            }

            foreach (var table in result)
            {
                var columnsCommand = new SqlCommand(
                    @"SELECT COLUMN_NAME, DATA_TYPE 
                      FROM INFORMATION_SCHEMA.COLUMNS 
                      WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table", connection);

                columnsCommand.Parameters.AddWithValue("@schema", table.Schema);
                columnsCommand.Parameters.AddWithValue("@table", table.TableName);

                using (var columnsReader = columnsCommand.ExecuteReader())
                {
                    while (columnsReader.Read())
                    {
                        var columnName = columnsReader.GetString(0);
                        var dataType = columnsReader.GetString(1);

                        table.Columns.Add(new ColumnInfo
                        {
                            ColumnName = columnName,
                            DataType = dataType
                        });
                    }
                }
            }
        }

        return Ok(result);
    }
}
