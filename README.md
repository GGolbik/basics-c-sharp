# Basics C Sharp

- [Example 1](example-1)
  - A .Net 6 console application.
- [Example 2](example-2)
  - A .Net 6 console application with unit tests and code coverage.
- Example 3 (Not implemented)
  - A .NET 6 library as NuGet package.
- [Example 4](example-4)
  - An ASP.NET 6 Web-API with versionized API, OpenAPI Swagger documentation and localization.
- [Snippets](snippets)
  - [Download a file](snippets/download)

# Docker

A `Dockerfile` for runtime:
~~~
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