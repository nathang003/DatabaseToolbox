

namespace ToolboxWebApiLibrary.DataAccess;

public class DataTableLineageData : IDataTableLineageData
{
    private ISqlDataAccess _sqlDataAccess;

    public DataTableLineageData(ISqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }


    /// <summary>
    /// This method gets a list of all known existing data table lineages from the ToolboxApp database.
    /// </summary>
    /// <returns>
    /// A list of DataTableLineageModel objects.
    /// </returns>
    public Task<List<DataTableLineageModel>> GetAllDataTableLineages()
    {
        var output = _sqlDataAccess.LoadData<DataTableLineageModel, dynamic>("dbo.spDataTableLineage_GetAll", new { }, "ToolboxData");

        return output;
    }
}
