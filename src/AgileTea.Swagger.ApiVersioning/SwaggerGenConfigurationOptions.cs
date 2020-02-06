using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AgileTea.Swagger.ApiVersioning
{
    [ExcludeFromCodeCoverage]
    internal class SwaggerGenConfigurationOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;
        private readonly IOptions<SwaggerGenOptionsExtended> extendedOptions;

        public SwaggerGenConfigurationOptions(
            IApiVersionDescriptionProvider provider,
            IOptions<SwaggerGenOptionsExtended> extendedOptions)
        {
            this.provider = provider;
            this.extendedOptions = extendedOptions;
        }

        public void Configure(SwaggerGenOptions options)
        {
            if (extendedOptions.Value.ApiVersionReaderType != ApiVersionReaderType.UrlSegment)
            {
                options.OperationFilter<AddApiVersionExampleValueOperationFilter>();
            }

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = extendedOptions.Value!.ApplicationTitle,
                        Version = description.ApiVersion.ToString()
                    });
            }
        }
    }
}