using Dapper;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ToolboxDataManager.Library.Internal.DataAccess
{
	internal class SqlDataAccess
	{
		public string GetConnectionString(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}

		/// <summary>
		/// Execute a stored procedure that will SELECT records using parameters of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="storedProcedure">The name of the stored procedure to perform the desired operation.</param>
		/// <param name="parameters">The data model to insert into the stored procedure.</param>
		/// <param name="connectionStringName">The name of the connection string to use for the operation.</param>
		/// <returns>A List of type T</returns>
		public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

				return rows;
			}
		}

		/// <summary>
		/// Execute a stored procedure that will INSERT or UPDATE records using parameters of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="storedProcedure">The name of the stored procedure to perform the desired operation.</param>
		/// <param name="parameters">The data model to insert into the stored procedure.</param>
		/// <param name="connectionStringName">The name of the connection string to use for the operation.</param>
		public void SaveData<T, U>(string storedProcedure, T parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection connection = new SqlConnection(connectionString))
			{
				connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Insert a list of type T into its target data table.>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="targetTable">The table where data should inserted.</param>
		/// <param name="parameters">The IList<T> values that will be inserted. </param>
		/// <param name="connectionStringName">The name of the connection string to use for the operation.</param>
		public void SaveBulkData<T,U>(string targetTable, IList<T> list, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (var copy = new SqlBulkCopy(connectionString))
			{
				copy.BatchSize = list.Count;
				copy.DestinationTableName = targetTable;

				DataTable table = new DataTable();
				var properties = TypeDescriptor.GetProperties(typeof(T))
					.Cast<PropertyDescriptor>()
					.Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
					.ToArray();

				foreach(var propertyInfo in properties)
				{
					copy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
					table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
				}

				var values = new object[properties.Length];
				foreach(var item in list)
				{
					for(var i = 0; i < properties.Length; i++)
					{
						values[i] = properties[i].GetValue(item);
					}

					table.Rows.Add(values);
				}

				copy.WriteToServer(table);
			}
		}
	}
}
