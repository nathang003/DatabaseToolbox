

namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IFieldData
    {
        Task<List<FieldModel>> GetAllFields();
        Task<List<FieldModel>> GetAllFieldsByTableId(int tableId);
        Task<FieldDetailedModel> GetFieldDetailsByFieldId(int FieldId);
        Task<List<FieldModel>> FindAllFields(string ServerName, string DatabaseName, string SchemaName, string TableName);
        Task<FieldDetailedModel> FindAllValues(FieldDetailedModel Field);
        Task FieldUpdate(FieldDetailedModel Field);
        Task FieldsUpsert(List<FieldModel> Fields, int TableId);
        Task<int> GetFieldIdByNames(string ServerName, string DatabaseName, string SchemaName, string TableName, string FieldName);
    }
}