
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class DatabaseObjectData : IDatabaseObjectData
{
    private ISqlDataAccess _sql;
    private Task<List<DatabaseObjectModel>> results;

    public DatabaseObjectData(ISqlDataAccess sql)
    {
        _sql = sql;
        results = _sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetAll", new { }, "ToolboxData");
    }

    public Task<List<DatabaseObjectModel>> GetAllDatabaseObjects()
    {
        //var output = _sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetAll", new { }, "ToolboxData");
        //return output;
        return results;
    }

    public Task<List<DatabaseObjectModel>> GetExactMatches(string searchValue)
    {
        //var output = _sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetExactMatches", new { }, "ToolboxData");
        //return output;
        return Task.FromResult(results.Result.Where(o => o.DatabaseObjectName.ToLower().Equals(searchValue.ToLower())
                                                            || o.DatabaseFullAddress.ToLower().Equals(searchValue.ToLower())
                                                            || o.Purpose.ToLower().Equals(searchValue.ToLower())).Take(100).ToList<DatabaseObjectModel>());
    }

    public Task<List<DatabaseObjectModel>> GetPartialMatches(string searchValue)
    {
        //var output = _sql.LoadData<DatabaseObjectModel, dynamic>("dbo.spDatabaseObjects_GetPartialMatches", new { }, "ToolboxData");
        //return output;
        return Task.FromResult(results.Result.Where(o => o.DatabaseObjectName.ToLower().Contains(searchValue.ToLower())
                                                            || o.DatabaseFullAddress.ToLower().Contains(searchValue.ToLower())
                                                            || o.Purpose.ToLower().Contains(searchValue.ToLower())).Take(100).ToList<DatabaseObjectModel>());
    }
}
