using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddResponseHeadersFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("200"))
        {
            var response = operation.Responses["200"];
            if (response.Headers == null)
                response.Headers = new Dictionary<string, OpenApiHeader>();

            response.Headers.Add("Authorization", new OpenApiHeader
            {
                Description = "JWT Token",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
