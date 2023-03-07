namespace ToolboxWebLibrary.Api
{
    public interface IConstraintEndpoint
    {
        Task AddConstraint(ConstraintModel constraint);
        Task AddConstraintField(ConstraintFieldModel constraintField);
        Task DeleteConstraint(ConstraintModel constraint);
        Task DeleteConstraintField(ConstraintFieldModel constraintField);
        Task DeleteConstraintFields(int constraintId);
        Task<List<ConstraintFieldModel>> GetAllConstraintFields();
        Task<List<ConstraintModel>> GetAllConstraints();
        Task<List<ConstraintModel>> GetAllConstraintsByTableId(int tableId);
        Task<List<ConstraintFieldModel>> GetConstraintFieldsByConstraintId(int constraintId);
        Task UpdateConstraint(ConstraintModel constraint);
    }
}