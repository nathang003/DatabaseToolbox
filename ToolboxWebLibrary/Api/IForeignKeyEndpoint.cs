namespace ToolboxWebLibrary.Api
{
    public interface IForeignKeyEndpoint
    {
        Task<List<ForeignKeyModel>> GetAllForeignKeys();
    }
}