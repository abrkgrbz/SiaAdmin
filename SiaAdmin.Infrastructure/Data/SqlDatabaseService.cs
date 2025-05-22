using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SiaAdmin.Application.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Infrastructure.Data
{
    public class SqlDatabaseService: IDatabaseService
    {
        private readonly IConfiguration _configuration;

        public SqlDatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            string? connectionString = _configuration.GetConnectionString("SiaAdminSql");
            connectionString = connectionString?.Replace("&amp;", "&");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandTimeout = 300; // 5 dakika timeout
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}
