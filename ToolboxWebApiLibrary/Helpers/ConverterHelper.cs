
using System.Reflection;

namespace ToolboxWebApiLibrary.Helpers;

public class ConverterHelper : IConverterHelper
{
    /// <summary>
    /// Converts a list of data types or models into a datatable.
    /// </summary>
    /// <typeparam name="T">The data type or model type that the list contains.</typeparam>
    /// <param name="list">The List<T> object that needs to be converted to a datatable.</param>
    /// <returns>DataTable</returns>
    public DataTable ConvertModelToDataTable<T>(IList<T> list)
    {
        var properties = typeof(T).GetProperties();
        DataTable dt = new();
        int i = 0;

        // Add columns to data table based on the model's properties.
        foreach (PropertyInfo prop in properties)
        {
            Console.WriteLine($"Adding {++i} of {properties.Length}: {prop.Name}, {prop.PropertyType.ToString()}");
            dt.Columns.Add(prop.Name, prop.PropertyType);
        }

        // Add rows to the data tbale based on the List of models.
        foreach (T record in list)
        {
            var values = new object[properties.Length];
            for (int j = 0; j < properties.Length; j++)
            {
                values[j] = properties[j].GetValue(record, null);
            }
            dt.Rows.Add(values);
        }

        return dt;
    }
}
