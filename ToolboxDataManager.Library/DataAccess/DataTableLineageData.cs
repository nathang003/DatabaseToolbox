using ToolboxDataManager.Library.Internal.DataAccess;
using ToolboxDataManager.Library.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxDataManager.Library.DataAccess
{
    public class DataTableLineageData
    {

        public List<DataTableLineageModel> FindDataTableLineages()
        {
            Debug.WriteLine("Entering ToolboxDataManager.Library.DataAccess.DataTableLineageData.FindDataTableLineages");

            SqlDataAccess sql = new SqlDataAccess();
            List<DataTableLineageModel> dataTableLineages = new List<DataTableLineageModel>();

            List<PackageDataLineageModel> packageData = sql.LoadData<PackageDataLineageModel, dynamic>("dbo.spEtlSubPkg_GetParsedData", new { }, "MedEconProdStaging");

            Debug.WriteLine($"Found { packageData.Count } records of Data Table Lineage.");

            if (packageData != null && packageData.Count > 0)
            {
                int counter = 1;

                foreach (PackageDataLineageModel package in packageData)
                {
                    Debug.WriteLine($"Working Data Table Lineage record { counter } of { packageData.Count }.");

                    TableData tableData = new TableData();

                    TableModel parentTable = tableData.GetTableByNames(package.ParentServer, package.ParentDatabase, package.ParentSchema, package.ParentTable);
                    TableModel childTable = tableData.GetTableByNames(package.ChildServer, package.ChildDatabase, package.ChildSchema, package.ChildTable);

                    if (parentTable != null && childTable != null && parentTable.Id > 0 && childTable.Id > 0)
                    {
                        dataTableLineages.Add(new DataTableLineageModel() { ParentTableId = parentTable.Id, ChildTableId = childTable.Id, LineageStartDate = DateTime.UtcNow, CreatedBy = "ToolboxApp", CreatedDate = DateTime.UtcNow, UpdatedBy = "ToolboxApp", UpdatedDate = DateTime.UtcNow });
                    }

                    counter++;
                }
            }
            return dataTableLineages;
        }

        public List<DataTableLineageModel> GetAllDataTableLineages()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DataTableLineageModel, dynamic>("dbo.spDataTableLineage_GetAll", new { }, "ToolboxData");

            return output;
        }

        public List<DatabaseModel> GetAllDataTableLineageParentsByTableId(int tableId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseModel, dynamic>("dbo.spDataTableLineage_GetParentsByTableId", new { TableId = tableId }, "ToolboxData");

            return output;
        }

        public List<DatabaseModel> GetAllDataTableLineageChildrenByTableId(int tableId)
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<DatabaseModel, dynamic>("dbo.spDataTableLineage_GetChildrenByTableId", new { TableId = tableId }, "ToolboxData");

            return output;
        }

        public void AddDataTableLineage(DataTableLineageModel dataTableLineageModel)
        {

            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { ParentTableId = dataTableLineageModel.ParentTableId, ChildTableId = dataTableLineageModel.ChildTableId, LineageStartDate = dataTableLineageModel.LineageStartDate, LineageEndDate = dataTableLineageModel.LineageEndDate, UserId = dataTableLineageModel.CreatedBy };

            sql.SaveData<dynamic, DataTableLineageModel>("dbo.spDataTableLineage_Insert", parameters, "ToolboxData");

        }

        public void BulkAddDataTableLineage(List<DataTableLineageModel> dataTableLineageModels)
        {

            SqlDataAccess sql = new SqlDataAccess();

            sql.SaveBulkData<dynamic, DataTableLineageModel>( "dbo.DataTableLineageData", (IList<dynamic>)dataTableLineageModels, "ToolboxData");

        }

        public void UpdateDataTableLineage(DataTableLineageModel dataTableLineageModel)
		{

            SqlDataAccess sql = new SqlDataAccess();
            var parameters = new { ParentTableId = dataTableLineageModel.ParentTableId, ChildTableId = dataTableLineageModel.ChildTableId, LineageStartDate = dataTableLineageModel.LineageStartDate, LineageEndDate = dataTableLineageModel.LineageEndDate };

            sql.SaveData<dynamic, DataTableLineageModel>("dbo.spDataTableLineage_Update", parameters, "ToolboxData");
		}
    }
}
