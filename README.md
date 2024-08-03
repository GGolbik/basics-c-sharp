# Basics C Sharp

- [001-console-app](001-console-app)
  - A .Net 6 console application.
- [002-tests-and-code-coverage](002-tests-and-code-coverage)
  - A .Net 6 console application with unit tests and code coverage.
- [003-library-nuget](003-library-nuget)
  - A .NET 6 library as NuGet package.
- [004-web-api-swagger](004-web-api-swagger)
  - An ASP.NET 6 Web-API with Swagger documentation.
- [005-web-api-angular](005-web-api-angular)
  - An ASP.NET 6 Web-API with Angular UI.
- [006-web-api-i18n](006-web-api-i18n)
  - An ASP.NET 6 Web-API with localization.
- [007-web-api-versionized-swagger](007-web-api-versionized-swagger)
  - An ASP.NET 6 Web-API with versionized API and extended Swagger documentation. It provides an example of an exception during a request.
- [008-etc-passwd](008-etc-passwd)
  - Example to read `/etc/passwd` file
- [Snippets](snippets)
  - [Download a file](snippets/download)
  - [Unix-Crypt Implementation](snippets/unix-crypt)

# .NET

https://dotnet.microsoft.com/en-us/download/dotnet

# Docker

A `Dockerfile` for runtime:
~~~Docker
# Use .NET Core Runtime as release container - https://hub.docker.com/_/microsoft-dotnet-core-runtime/
FROM mcr.microsoft.com/dotnet/core/runtime:6.0

LABEL \
  # version of the packaged software
  org.opencontainers.image.version="1.0.0" \
  # date and time on which the image was built (string, date-time as defined by RFC 3339).
  org.opencontainers.image.created=${LABEL_CREATED} \
  # Name of the distributing entity, organization or individual.
  org.opencontainers.image.vendor="GGolbik" \
  # Text description of the image.
  org.opencontainers.image.description="A simple .Net Core console application."

COPY build/Release/. /app/
CMD ["dotnet", "/app/001-console-app.dll"]
~~~

# Useful Packages

- [CommandLineParser](https://www.nuget.org/packages/CommandLineParser): Terse syntax C# command line parser for .NET. 
- [Npgsql](https://www.nuget.org/packages/Npgsql/): Npgsql is the open source .NET data provider for PostgreSQL. It allows you to connect and interact with PostgreSQL server using .NET.
- [Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore): Serilog is a diagnostic logging library for .NET applications. It is easy to set up, has a clean API, and runs on all recent .NET platforms. While it's useful even in the simplest applications, Serilog's support for structured logging shines when instrumenting complex, distributed, and asynchronous applications and systems.
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/): the capability to auto-generate the Swagger spec for your RESTful application.
- [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer): ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token.
