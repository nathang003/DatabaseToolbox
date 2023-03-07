using System.Collections.Generic;

namespace ToolboxDataManager.Library.Internal.DataAccess
{
	internal interface ISqlDataAccess
	{
		string GetConnectionString(string name);

		List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);

		void SaveData<T, U>(string storedProcedure, T parameters, string connectionStringName);
	}
}