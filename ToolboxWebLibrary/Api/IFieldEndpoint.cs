namespace ToolboxWebLibrary.Api
{
    public interface IFieldEndpoint
    {
        Task<List<FieldModel>> GetAllFields();
        Task<List<FieldModel>> GetAllFieldsByTableId(int tableId);
    }
}