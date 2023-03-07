
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface IConstraintData
    {
        Task<List<ConstraintModel>> GetAllConstraints();
        Task<List<ConstraintDetailedModel>> GetConstraintsByTableId(int tableId);
        Task AddConstraint(ConstraintModel constraint);
        Task DeleteConstraint(ConstraintModel constraint);
        Task UpdateConstraint(ConstraintModel constraint);
        Task<List<ConstraintFieldModel>> GetAllConstraintFields();
        Task<List<ConstraintFieldModel>> GetConstraintFieldsByConstraintId(int constraintId);
        Task AddConstraintField(ConstraintFieldModel constraintField);
        Task DeleteConstraintField(ConstraintFieldModel constraintField);
        Task DeleteConstraintFields(int constraintId);
        Task ConstraintUpsert(ConstraintModel Constraints, int TableId);
        Task ConstraintUpsert(List<ConstraintModel> Constraints, int TableId);
        Task ConstraintUpsert(List<ConstraintDetailedModel> Constraints, int TableId);
        Task ConstraintFieldsUpsert(List<ConstraintFieldModel> ConstraintFields, int ConstraintId);
        Task<List<ConstraintDetailedModel>> FindAllConstraints(TableDetailedModel table);
        Task<List<ConstraintFieldModel>> FindAllConstraintFields(ConstraintDetailedModel constraint);
    }
}