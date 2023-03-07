
namespace ToolboxWebApiLibrary.Models
{
    public class ForeignKeyDetailedModel : ForeignKeyModel
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ConstraintSchemaName { get; set; }
        public string ConstraintTableName { get; set; }
        public string ConstraintFieldName { get; set; }
        public string ReferencedSchemaName { get; set; }
        public string ReferencedTableName { get; set; }
        public string ReferencedFieldName { get; set; }

        public ForeignKeyDetailedModel()
        {
        
        }

        public ForeignKeyDetailedModel(ForeignKeyModel foreignKeyModel)
        {        
            ServerId = foreignKeyModel.ServerId;
            DatabaseId = foreignKeyModel.DatabaseId;
            SchemaId = foreignKeyModel.SchemaId;
            TableId = foreignKeyModel.TableId;
            ConstraintSchemaId = foreignKeyModel.ConstraintSchemaId;
            ConstraintTableId = foreignKeyModel.ConstraintTableId;
            ConstraintFieldId = foreignKeyModel.ConstraintFieldId;
            ReferencedSchemaId = foreignKeyModel.ConstraintSchemaId;
            ReferencedTableId = foreignKeyModel.ReferencedTableId;
            ReferencedFieldId = foreignKeyModel.ReferencedFieldId;
        }


        public ForeignKeyModel ToForeignKeyModel()
        {
            return (ForeignKeyModel)this;
        }
    }
}
