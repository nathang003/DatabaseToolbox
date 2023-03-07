namespace ToolboxWebLibrary.Api
{
    public interface IIndexEndpoint
    {
        Task<List<IndexModel>> GetAllIndexes();
        Task<List<IndexModel>> GetIndexesByTableId(int tableId);
    }
}