
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IIndexData
    {
        Task<List<IndexModel>> GetAllIndexes();
        Task<List<IndexModel>> GetIndexesByTableId(int tableId);
        Task<List<IndexDetailedModel>> FindAllIndexes(TableDetailedModel table);
        Task<List<IndexFieldModel>> FindAllIndexFields(IndexDetailedModel index);
        Task IndexUpsert(IndexModel Indexes, int TableId);
        Task IndexUpsert(List<IndexModel> Indexes, int TableId);
        Task IndexFieldsUpsert(List<IndexFieldModel> IndexFields, int IndexId);
    }
}