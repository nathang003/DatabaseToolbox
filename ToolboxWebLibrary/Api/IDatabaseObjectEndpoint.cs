namespace ToolboxWebLibrary.Api
{
    public interface IDatabaseObjectEndpoint
    {
        Task<List<DatabaseObjectModel>> GetAllDatabaseObjects();
        Task<List<DatabaseObjectModel>> GetExactMatches(string searchValue);
        Task<List<DatabaseObjectModel>> GetPartialMatches(string searchValue);
    }
}