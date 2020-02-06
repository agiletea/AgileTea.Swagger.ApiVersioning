# AgileTea.Swagger.ApiVersioning

[![Build Status](https://agiletea.visualstudio.com/Agile%20Tea%20Swagger%20Api%20Versioning/_apis/build/status/agiletea.AgileTea.Swagger.ApiVersioning?branchName=master)]()
[![NuGet Version](https://img.shields.io/nuget/v/AgileTea.AspNetCore.Swagger.ApiVersioning)](https://www.nuget.org/packages/AgileTea.AspNetCore.Swagger.ApiVersioning/)

Combines Api Versioning with Api Versioning Discovery and Swagger Generation to provide a self-discovering api versioning configuration fronted with a Swashbuckle Swagger UI Frontend

## Installation

AgileTea.AspNetCore.Swagger.ApiVersioning installs through [NuGet][1] and requires [.NET Core 3.1][2]

```
PS> Install-Package AgileTea.AspNetCore.Swagger.ApiVersioning
```

## Configuration (in Startup.cs)

```csharp

public void ConfigureServices(IServiceCollection services)
{
  // service configuration etc.
  // ...
  
    services.AddSwaggerGenDiscovery(
        options =>
        {
            options.ApplicationTitle = "Agile Tea Learning Web Api";
            options.ApiVersionReaderType = ApiVersionReaderType.UrlSegment;
        });

  // ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
{
    // other code removed from brevity
    app.UseSwaggerWithSelfDiscovery(provider);

    // ...
}
```

## Self Discovery

Decorate your controllers with your api version in whichever approach you prefer:

### Header/ Query Parameter versioning:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class SomeClassController : ControllerBase
{
  // etc
}
```

### Url Segment versioning:

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SomeClassController : ControllerBase
{
  // etc
}
```

Configuration will the pick up the versions and apply to middleware pipelines to enforce api version as well as allow automatic swagger document generations.

## Swagger UI
Browser to your local swagger end point to view results (i.e. https://localhost:5001/swagger)

[0]: https://swagger.io/tools/swagger-ui/
[1]: https://www.nuget.org/packages/AgileTea.AspNetCore.Swagger.ApiVersioning
[2]: https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-1
[3]: https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/