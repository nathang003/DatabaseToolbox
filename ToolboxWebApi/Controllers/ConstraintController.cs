using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolboxWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ConstraintController : ControllerBase
{
    private IConstraintData _constraintData;
    public ConstraintController(IConstraintData constraintData)
    {
        _constraintData = constraintData;
    }


    // GET api/Constraint/GetAllConstraints
    [Route("GetAllConstraints", Name = "GetAllConstraints")]
    [HttpGet]
    public async Task<List<ConstraintModel>> GetAllConstraints()
    {
        return await _constraintData.GetAllConstraints();
    }


    // GET api/Constraint/GetConstraintsByTableId/{ tableId }
    [Route("GetConstraintsByTableId", Name = "GetConstraintsByTableId")]
    [HttpGet]
    public async Task<List<ConstraintModel>> GetConstraintsByTableId(int tableId)
    {
        List<ConstraintDetailedModel> detailedConstraints = await _constraintData.GetConstraintsByTableId(tableId);
        List<ConstraintModel> constraints = new List<ConstraintModel>();

        foreach (ConstraintDetailedModel constraint in detailedConstraints)
        {
            constraints.Add(constraint.ToConstraintModel());
        }

        return constraints;
    }


    // POST api/Constraint/AddConstraint
    [Route("AddConstraint", Name = "AddConstraint")]
    [HttpPost]
    public async Task AddConstraint(ConstraintModel constraint)
    {
        await _constraintData.AddConstraint(constraint);
    }


    // POST api/Constraint/DeleteConstraint
    [Route("DeleteConstraint", Name = "DeleteConstraint")]
    [HttpPost]
    public async Task DeleteConstraint(ConstraintModel constraint)
    {
        await _constraintData.DeleteConstraint(constraint);
    }


    // POST api/Constraint/UpdateConstraint
    [Route("UpdateConstraint", Name = "UpdateConstraint")]
    [HttpPost]
    public async Task UpdateConstraint(ConstraintModel constraint)
    {
        await _constraintData.UpdateConstraint(constraint);
    }


    // GET api/Constraint/GetAllConstraintFields
    [Route("GetAllConstraintFields", Name = "GetAllConstraintFields")]
    [HttpGet]
    public async Task<List<ConstraintFieldModel>> GetAllConstraintFields()
    {
        return await _constraintData.GetAllConstraintFields();
    }


    // GET api/Constraint/GetConstraintFieldsByConstraintId/{constraintId}
    [Route("GetConstraintFieldsByConstraintId", Name = "GetConstraintFieldsByConstraintId")]
    [HttpGet]
    public async Task<List<ConstraintFieldModel>> GetConstraintFieldsByConstraintId(int constraintId)
    {
        return await _constraintData.GetConstraintFieldsByConstraintId(constraintId);
    }


    // POST api/Constraint/AddConstraintField
    [Route("AddConstraintField", Name = "AddConstraintField")]
    [HttpPost]
    public async Task AddConstraintField(ConstraintFieldModel constraintField)
    {
        await _constraintData.AddConstraintField(constraintField);
    }


    // POST api/Constraint/DeleteConstraintField
    [Route("DeleteConstraintField", Name = "DeleteConstraintField")]
    [HttpPost]
    public async Task DeleteConstraintField(ConstraintFieldModel constraintField)
    {
        await _constraintData.DeleteConstraintField(constraintField);
    }


    // POST api/Constraint/DeleteConstraintFields
    [Route("DeleteConstraintFields", Name = "DeleteConstraintFields")]
    [HttpPost]
    public async Task DeleteConstraintFields(int constraintId)
    {
        await _constraintData.DeleteConstraintFields(constraintId);
    }
}
