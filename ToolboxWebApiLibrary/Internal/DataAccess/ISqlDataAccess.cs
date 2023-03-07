
namespace ToolboxWebApiLibrary.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        string GetConnectionString(string ServerName);
        string GetConnectionString(string ServerName, string DatabaseName);
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<List<T>> QueryData<T>(string sqlScript, string connectionStringName);
        Task SaveBulkData<T, U>(string targetTable, IList<T> list, string connectionStringName);
        Task SaveData<T, U>(string storedProcedure, T parameters, string connectionStringName);
    }
}