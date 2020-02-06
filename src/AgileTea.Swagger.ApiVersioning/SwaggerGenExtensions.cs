using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AgileTea.Swagger.ApiVersioning
{
    /// <summary>
    /// Extension methods for <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection" />
    /// Include Configuration Options classes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SwaggerGenExtensions
    {
        /// <summary>
        /// Adds api versioning, api version discovery and swagger generation for url segment, header or query parameter api versioning
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="options">Api versioning options</param>
        /// <exception cref="ArgumentNullException">Thrown if options are null</exception>
        public static void AddSwaggerGenDiscovery(
            this IServiceCollection services,
            Action<SwaggerGenOptionsExtended> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.Configure(options);
            services.ConfigureOptions<ConfigureApiVersioningOptions>();
            services.AddApiVersioning();
            services.ConfigureOptions<ConfigureApiExplorerOptions>();
            services.AddVersionedApiExplorer();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfigurationOptions>();
            services.AddSwaggerGen();
        }

        /// <summary>
        /// Helper method to apply the Swagger UI with self discovery of all available versions
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="provider">Api Version description provider to gain access to the discovered api versions</param>
        public static void UseSwaggerWithSelfDiscovery(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        [ExcludeFromCodeCoverage]
        private class ConfigureApiVersioningOptions : IConfigureOptions<ApiVersioningOptions>
        {
            private readonly IOptionsMonitor<SwaggerGenOptionsExtended> extendedOptions;

            public ConfigureApiVersioningOptions(IOptionsMonitor<SwaggerGenOptionsExtended> extendedOptions)
            {
                this.extendedOptions = extendedOptions;
            }

            [SuppressMessage(
                "Major Code Smell",
                "S3928:Parameter names used into ArgumentException constructors should match an existing one ",
                Justification = "Only able to access the options through injection so parameter will always be embedded and not a named one")]
            public void Configure(ApiVersioningOptions options)
            {
                options.ReportApiVersions = extendedOptions.CurrentValue.ReportApiVersions;
                options.DefaultApiVersion = extendedOptions.CurrentValue.DefaultApiVersion ?? ApiVersion.Default;
                options.AssumeDefaultVersionWhenUnspecified = extendedOptions.CurrentValue.AssumeDefaultVersionWhenUnspecified;

                options.ApiVersionReader = extendedOptions.CurrentValue.ApiVersionReaderType switch
                {
                    ApiVersionReaderType.Header => new HeaderApiVersionReader(extendedOptions.CurrentValue.HeaderParameterName),
                    ApiVersionReaderType.QueryParameter => new QueryStringApiVersionReader(extendedOptions.CurrentValue.QueryParameterName),
                    ApiVersionReaderType.UrlSegment => new UrlSegmentApiVersionReader(),
                    _ => throw new ArgumentOutOfRangeException(nameof(extendedOptions.CurrentValue.ApiVersionReaderType))
                };
            }
        }

        [ExcludeFromCodeCoverage]
        private class ConfigureApiExplorerOptions : IConfigureOptions<ApiExplorerOptions>
        {
            private readonly IOptions<SwaggerGenOptionsExtended> extendedOptions;

            public ConfigureApiExplorerOptions(IOptions<SwaggerGenOptionsExtended> extendedOptions)
            {
                this.extendedOptions = extendedOptions;
            }

            public void Configure(ApiExplorerOptions options)
            {
                options.GroupNameFormat = extendedOptions.Value.GroupNameFormat;
                options.DefaultApiVersion = extendedOptions.Value.DefaultApiVersion ?? ApiVersion.Default;
                options.AssumeDefaultVersionWhenUnspecified = extendedOptions.Value.AssumeDefaultVersionWhenUnspecified;
                options.SubstituteApiVersionInUrl = extendedOptions.Value.ApiVersionReaderType == ApiVersionReaderType.UrlSegment;
            }
        }
    }
}