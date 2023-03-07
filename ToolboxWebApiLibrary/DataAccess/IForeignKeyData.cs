
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IForeignKeyData
    {
        Task<List<ForeignKeyModel>> GetAllForeignKeys();
        Task<List<ForeignKeyDetailedModel>> FindAllForeignKeys(TableDetailedModel table);
        Task ForeignKeyUpsert(List<ForeignKeyDetailedModel> ForeignKeys, int TableId);
    }
}