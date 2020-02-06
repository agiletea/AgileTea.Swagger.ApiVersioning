using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace AgileTea.Swagger.ApiVersioning
{
    /// <summary>
    /// Combines multiple options from api versioning and api version explorer to be applied to Swagger Gen
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerGenOptionsExtended
    {
        /// <summary>
        /// Gets or sets the type of Api Version Reader
        /// </summary>
        public ApiVersionReaderType ApiVersionReaderType { get; set; }

        /// <summary>
        /// Gets or sets the application title, defaulting to the entry assembly name
        /// </summary>
        public string? ApplicationTitle { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        /// <summary>
        /// Gets or sets whether to report api versions. Defaults to <c>true</c>
        /// </summary>
        public bool ReportApiVersions { get; set; } = true;

        /// <summary>
        /// Gets or sets the api version string format. Defaults to "'v'VV" (v1.0)
        /// </summary>
        public string GroupNameFormat { get; set; } = "'v'VV";

        /// <summary>
        /// Gets or sets the default api version
        /// </summary>
        public ApiVersion? DefaultApiVersion { get; set; }

        /// <summary>
        /// Gets or sets whether to assume the default version is non is specified
        /// </summary>
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }

        /// <summary>
        /// Gets or sets the name of the query parameter to use when in such mode. Defaults to "api-version"
        /// </summary>
        public string QueryParameterName { get; set; } = "api-version";

        /// <summary>
        /// Gets or sets the name of the header parameter to use when in such mode. Defaults to "x-api-version"
        /// </summary>
        public string HeaderParameterName { get; set; } = "x-api-version";
    }
}
