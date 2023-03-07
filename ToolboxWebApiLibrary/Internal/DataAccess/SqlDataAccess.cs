using System.ComponentModel;

using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using Dapper;

namespace ToolboxWebApiLibrary.Internal.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    public string GetConnectionString(string ServerName)
    {
        Uri KeyVaultEndpoint = new (Environment.GetEnvironmentVariable("VaultUri"));

        SecretClientOptions secretOptions = new SecretClientOptions()
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };
        var client = new SecretClient(KeyVaultEndpoint, new DefaultAzureCredential(), secretOptions);
        KeyVaultSecret toolboxUserName = client.GetSecret("EDWToolboxUserName");
        KeyVaultSecret toolboxPassword = client.GetSecret("EDWToolboxPwd");
        KeyVaultSecret edwDatabaseUserName = client.GetSecret("EdwDatabasesUserName");
        KeyVaultSecret edwDatabasePassword = client.GetSecret("EdwDatabasesPassword");

        switch (ServerName)
        {
            case "mncportaldev-sql.database.windows.net":
            case "mncportalprod-sql.database.windows.net":
            case "mncportalstage-sql.database.windows.net":
            case "mncportalstage2.database.windows.net":
            case "secondary-mncportalprod.database.windows.net":
                return $"Server={ServerName}; Authentication=Active Directory Password; Database=ToolboxApp; User Id={toolboxUserName.Value}; Password={toolboxPassword.Value}";
            case @"AZE1DSQLME01.dev.corp.mynexuscare.com\DEV":
            case "AZE1PSQLME01":
            case "AZE1SSQLME01":
            case "AZE1SSQLME02":
                return $"Server={ServerName}; User ID={edwDatabaseUserName.Value}; Password={edwDatabasePassword.Value}; Encrypt=false;";
            case "ToolboxData":
            default:
                return $"Server=AZE1PSQLME01; Database=ToolboxApp; User ID={edwDatabaseUserName.Value}; Password={edwDatabasePassword.Value}; Encrypt=false;";
        }
    }


    public string GetConnectionString(string ServerName, string DatabaseName)
    {
        Uri KeyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));

        SecretClientOptions secretOptions = new SecretClientOptions()
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };
        var client = new SecretClient(KeyVaultEndpoint, new DefaultAzureCredential(), secretOptions);
        KeyVaultSecret toolboxUserName = client.GetSecret("EDWToolboxUserName");
        KeyVaultSecret toolboxPassword = client.GetSecret("EDWToolboxPwd");
        KeyVaultSecret edwDatabaseUserName = client.GetSecret("EdwDatabasesUserName");
        KeyVaultSecret edwDatabasePassword = client.GetSecret("EdwDatabasesPassword");

        switch (ServerName)
        {
            case "mncportaldev-sql.database.windows.net":
            case "mncportalprod-sql.database.windows.net":
            case "mncportalstage-sql.database.windows.net":
            case "mncportalstage2.database.windows.net":
            case "secondary-mncportalprod.database.windows.net":
                return $"Server={ServerName}; Authentication=Active Directory Password; Database={DatabaseName}; User Id={toolboxUserName.Value}; Password={toolboxPassword.Value}";
            case @"AZE1DSQLME01.dev.corp.mynexuscare.com\DEV":
            case "AZE1PSQLME01":
            case "AZE1SSQLME01":
            case "AZE1SSQLME02":
                return $"Server={ServerName}; Database={DatabaseName}; User ID={edwDatabaseUserName.Value}; Password={edwDatabasePassword.Value}; Encrypt=false;";
            case "ToolboxData":
            default:
                return $"Server=AZE1PSQLME01; Database=ToolboxApp; User ID={edwDatabaseUserName.Value}; Password={edwDatabasePassword.Value}; Encrypt=false;";
        }
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
    public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

            return rows;
        }
    }

    /// <summary>
    /// Execute a stored procedure that will SELECT records using a SQL script.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlScript">The SQL script to perform the desired operation.</param>
    /// <param name="connectionStringName">The name of the connection string to use for the operation.</param>
    /// <returns>A List of type T</returns>
    public async Task<List<T>> QueryData<T>(string sqlScript, string connectionStringName)
    {
        string connectionString = GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(sqlScript, commandType: CommandType.Text).ToList();

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
    public async Task SaveData<T, U>(string storedProcedure, T parameters, string connectionStringName)
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
    public async Task SaveBulkData<T, U>(string targetTable, IList<T> list, string connectionStringName)
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

            foreach (var propertyInfo in properties)
            {
                copy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }

            var values = new object[properties.Length];
            foreach (var item in list)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            copy.WriteToServer(table);
        }
    }
}
