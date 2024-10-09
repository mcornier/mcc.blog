using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BlogBackend.Filters
{
    public class HideSwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Clear(); // Efface tous les chemins pour masquer Swagger
        }
    }
}
