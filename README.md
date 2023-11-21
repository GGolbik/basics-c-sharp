# Basics C Sharp

- [Example 1](example-1)
  - A .Net 6 console application.
- [Example 2](example-2)
  - A .Net 6 console application with unit tests and code coverage.
- [Example 3](example-3)
  - A .NET 6 library as NuGet package.
- [Example 4](example-4)
  - An ASP.NET 6 Web-API with Swagger documentation.
- [Example 5](example-5)
  - An ASP.NET 6 Web-API with Angular UI.
- [Example 6](example-6)
  - An ASP.NET 6 Web-API with localization.
- [Example 7](example-7)
  - An ASP.NET 6 Web-API with versionized API and extended Swagger documentation. It provides an example of an exception during a request.
- [Example 8](example-8)
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
CMD ["dotnet", "/app/example-1.dll"]
~~~

# Useful Packages

- [CommandLineParser](https://www.nuget.org/packages/CommandLineParser): Terse syntax C# command line parser for .NET. 
- [Npgsql](https://www.nuget.org/packages/Npgsql/): Npgsql is the open source .NET data provider for PostgreSQL. It allows you to connect and interact with PostgreSQL server using .NET.
- [Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore): Serilog is a diagnostic logging library for .NET applications. It is easy to set up, has a clean API, and runs on all recent .NET platforms. While it's useful even in the simplest applications, Serilog's support for structured logging shines when instrumenting complex, distributed, and asynchronous applications and systems.
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/): the capability to auto-generate the Swagger spec for your RESTful application.
- [JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer): ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token.
