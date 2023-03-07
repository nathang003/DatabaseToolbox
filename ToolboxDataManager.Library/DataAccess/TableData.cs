using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class TableData
    {
        /// <summary>
        /// This method gets a list of all known existing tables from the ToolboxApp database.
        /// </summary>
        /// <returns>A list of TableModel objects.</returns>
        public List<TableModel> GetAllTables()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<TableModel, dynamic>("dbo.spTables_GetAll", new { }, "ToolboxData");

            return output;
        }

        public List<TableModel> GetAllTablesBySchemaId(int schemaId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<TableModel, dynamic>("dbo.spTables_GetBySchemaId", schemaId, "ToolboxData");

            return output;
        }

        /// <summary>
        /// Gets a table model from the ToolboxApp database based on matching entered parameters.
        /// </summary>
        /// <param name="serverName">The table's server name.</param>
        /// <param name="databaseName">The table's database name.</param>
        /// <param name="schemaName">The table's schema name.</param>
        /// <param name="tableName">The table's name.</param>
        /// <returns>A TableModel if the table is known and it exists.</returns>
        public TableModel GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
		{
            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { ServerName = serverName, DatabaseName = databaseName, SchemaName = schemaName, TableName = tableName };
            var output = sql.LoadData<TableModel, dynamic>("dbo.spTables_GetByNames", parameters, "ToolboxData");

            return output.FirstOrDefault();
		}
    }
}
