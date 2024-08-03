# 004-web-api-swagger - ASP .Net Web-API

# Table of Contents

* [Create](#create)
  * [http](#http)
  * [https](#https)
* [VSCode](#vscode)

# Create

Create a new console application project. This example provides the [http](#http).

## http

~~~
dotnet new webapi --no-https --framework net6.0 --name 004-web-api-swagger --output ./src
~~~

Change in `src/Properties/launchSettings.json` the applicationUrl in `example_4` to http://0.0.0.0:5080
~~~json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:13154",
      "sslPort": 0
    }
  },
  "profiles": {
    "example_4": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5117",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
~~~

~~~
dotnet run --urls "http://$(hostname -I | awk '{print $1}'):5080"
~~~

## https

~~~
dotnet new webapi --framework net6.0 --name 004-web-api-swagger --output ./src
~~~

Change in `src/Properties/launchSettings.json` the applicationUrl in `example 4` to https://0.0.0.0:5443;http://0.0.0.0:5080
~~~json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:50317",
      "sslPort": 44312
    }
  },
  "profiles": {
    "example_4": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://0.0.0.0:5443;http://0.0.0.0:5080",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
~~~

~~~
dotnet run --urls "https://$(hostname -I | awk '{print $1}'):5443;http://$(hostname -I | awk '{print $1}'):5080"
~~~

# VSCode

Create a `launch.json` file in the `.vscode` folder if it does not exist.
This file will contain the ASP.NET Core configuration for VSCode.

~~~json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Example 4 Debug",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build-004-web-api-swagger",
      "program": "${workspaceFolder}/004-web-api-swagger/build/bin/004-web-api-swagger/Debug/net6.0/004-web-api-swagger.dll",
      "args": [],
      "cwd": "${workspaceFolder}/004-web-api-swagger/src",
      "stopAtEntry": false,
      "logging": {
        "moduleLoad": false
      },
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "%s/swagger"
      },
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "sourceFileMap": {
        "/Views": "${workspaceFolder}/Views"
      }
    }
  ]
}
~~~

Create a `tasks.json` file in the `.vscode` folder if it does not exist.
This file will contain the ASP.NET Core build configuration for VSCode.
~~~json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build-004-web-api-swagger",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/004-web-api-swagger/src/004-web-api-swagger.csproj",
        "/property:GenerateFullPaths=true",
        "/consoleloggerparameters:NoSummary"
      ],
      "problemMatcher": "$msCompile"
    }
  ]
}
~~~

The resource will be available at
- http://localhost:5080/swagger
- https://localhost:5080/swagger
- http://localhost:5080/WeatherForecast
- https://localhost:5080/WeatherForecast
