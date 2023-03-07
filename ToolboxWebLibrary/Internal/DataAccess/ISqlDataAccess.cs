
namespace ToolboxWebLibrary.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string ServerName);
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task SaveBulkData<T, U>(string targetTable, IList<T> list, string connectionStringName);
        Task SaveData<T, U>(string storedProcedure, T parameters, string connectionStringName);
    }
}