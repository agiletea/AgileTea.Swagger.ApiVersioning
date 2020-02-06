using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AgileTea.Swagger.ApiVersioning
{
    [ExcludeFromCodeCoverage]
    internal class AddApiVersionExampleValueOperationFilter : IOperationFilter
    {
        private readonly IOptions<SwaggerGenOptionsExtended> extendedOptions;
        private readonly string apiVersionParameterName;
        private readonly ILogger<AddApiVersionExampleValueOperationFilter> logger;

        [SuppressMessage(
            "Major Code Smell",
            "S3928:Parameter names used into ArgumentException constructors should match an existing one ",
            Justification = "Only able to access the options through injection so parameter will always be embedded and not a named one")]
        public AddApiVersionExampleValueOperationFilter(
            ILoggerFactory loggerFactory,
            IOptions<SwaggerGenOptionsExtended> extendedOptions)
        {
            this.extendedOptions = extendedOptions;

            apiVersionParameterName = extendedOptions.Value.ApiVersionReaderType switch
            {
                ApiVersionReaderType.QueryParameter => extendedOptions.Value.QueryParameterName,
                ApiVersionReaderType.Header => extendedOptions.Value.HeaderParameterName,
                _ => throw new ArgumentOutOfRangeException(nameof(extendedOptions.Value.ApiVersionReaderType))
            };

            logger = loggerFactory.CreateLogger<AddApiVersionExampleValueOperationFilter>();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersionParameter = operation.Parameters.SingleOrDefault(p => p.Name == apiVersionParameterName);

            if (apiVersionParameter == null)
            {
                logger.LogWarning("Api Version Parameter Example filter used without any api version parameter located");
                return;
            }

            var attribute = context?.MethodInfo?.DeclaringType?
                .GetCustomAttributes(typeof(ApiVersionAttribute), false)
                .Cast<ApiVersionAttribute>()
                .SingleOrDefault();

            var version = attribute?.Versions?.SingleOrDefault()?.ToString();

            if (version != null)
            {
                apiVersionParameter.Example = new OpenApiString(version);
                apiVersionParameter.Schema.Example = new OpenApiString(version);
            }
            else
            {
                logger.LogWarning($"Web Api Operation found without an api version declared: {context?.MethodInfo?.DeclaringType?.FullName}");
            }
        }
    }
}