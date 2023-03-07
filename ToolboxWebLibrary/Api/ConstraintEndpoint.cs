
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

using Newtonsoft.Json;

namespace ToolboxWebLibrary.Api;

public class ConstraintEndpoint : IConstraintEndpoint
{
    private IApiHelper _apiHelper;

    public ConstraintEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<ConstraintModel>> GetAllConstraints()
    {
        Console.WriteLine("Entering ConstraintEndpoint.GetAllConstraints()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Constraint/GetAllConstraints"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ConstraintModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ConstraintModel>> GetAllConstraintsByTableId(int tableId)
    {
        Console.WriteLine("Entering ConstraintEndpoint.GetAllConstraintsByTableId(int tableId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Constraint/GetAllConstraintsByTableId/{tableId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ConstraintModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task AddConstraint(ConstraintModel constraint)
    {
        Console.WriteLine("Entering ConstraintEndpoint.AddConstraint(ConstraintModel constraint)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ConstraintModel>($"/api/Constraint/AddConstraint", constraint))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task DeleteConstraint(ConstraintModel constraint)
    {
        Console.WriteLine("Entering ConstraintEndpoint.DeleteConstraint(ConstraintModel constraint)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ConstraintModel>($"/api/Constraint/DeleteConstraint", constraint))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task UpdateConstraint(ConstraintModel constraint)
    {
        Console.WriteLine("Entering ConstraintEndpoint.UpdateConstraint(ConstraintModel constraint)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ConstraintModel>($"/api/Constraint/UpdateConstraint", constraint))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ConstraintFieldModel>> GetAllConstraintFields()
    {
        Console.WriteLine("Entering ConstraintEndpoint.GetAllConstraintFields()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Constraint/GetAllConstraintFields"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ConstraintFieldModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ConstraintFieldModel>> GetConstraintFieldsByConstraintId(int constraintId)
    {
        Console.WriteLine("Entering ConstraintEndpoint.GetAllConstraintFields(int constraintId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Constraint/GetConstraintFieldsByConstraintId/{constraintId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<ConstraintFieldModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task AddConstraintField(ConstraintFieldModel constraintField)
    {
        Console.WriteLine("Entering ConstraintEndpoint.AddConstraintField(ConstraintFieldModel constraintField)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ConstraintFieldModel>($"/api/Constraint/AddConstraintField", constraintField))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task DeleteConstraintField(ConstraintFieldModel constraintField)
    {
        Console.WriteLine("Entering ConstraintEndpoint.DeleteConstraintField(ConstraintFieldModel constraintField)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ConstraintFieldModel>($"/api/Constraint/DeleteConstraintField", constraintField))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task DeleteConstraintFields(int constraintId)
    {
        Console.WriteLine("Entering ConstraintEndpoint.UpdateConstraintField(int constraintId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<int>($"/api/Constraint/UpdateConstraintField", constraintId))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
