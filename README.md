# .NetCommon

Common utilities for .NET Core projects.

![CI](https://github.com/alex75it/.NetCommon/workflows/CI/badge.svg)


## Alex75.Common.MongoDB package
[![NuGet](https://img.shields.io/nuget/v/Alex75.Common.MongoDB.svg)](https://www.nuget.org/packages/Alex75.Common.MongoDB) 

Utility to manage MongoDB collections.

## Alex75.Common.WebApiHosting package
[![NuGet](https://img.shields.io/nuget/v/Alex75.Common.WebApiHosting.svg)](https://www.nuget.org/packages/Alex75.Common.WebApiHosting) 

Allows you to easily add a self hosted web service and API controllers to a service.  

Usage: 
In ```Host.CreateDefaultBuilder().ConfigureServices()```:

```fsharp
Alex75.Common.WebApiHosting.HostCreator.CreateApiHost("http://localhost:5001", services:IServiceCollection)
```