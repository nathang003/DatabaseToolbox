namespace ToolboxWebApiLibrary.Helpers
{
    public interface IConverterHelper
    {
        DataTable ConvertModelToDataTable<T>(IList<T> list);
    }
}