namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IDataTableLineageData
    {
        Task<List<DataTableLineageModel>> GetAllDataTableLineages();
    }
}