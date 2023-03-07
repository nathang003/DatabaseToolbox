namespace ToolboxWebLibrary.Api
{
    public interface IDataTableLineageEndpoint
    {
        Task<List<DataTableLineageModel>> GetAllDataTableLineages();
    }
}