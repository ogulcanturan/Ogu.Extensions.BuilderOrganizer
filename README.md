# <img src="logo/ogu-logo.png" alt="Header" width="24"/> Ogu.Extensions.BuilderOrganizer

[![.NET](https://github.com/ogulcanturan/Ogu.Extensions.BuilderOrganizer/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/ogulcanturan/Ogu.Extensions.BuilderOrganizer/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/Ogu.Extensions.BuilderOrganizer.svg?color=1ecf18)](https://nuget.org/packages/Ogu.Extensions.BuilderOrganizer)
[![Nuget](https://img.shields.io/nuget/dt/Ogu.Extensions.BuilderOrganizer.svg?logo=nuget)](https://nuget.org/packages/Ogu.Extensions.BuilderOrganizer)

## Introduction

Ogu.Extensions.BuilderOrganizer is a library helps to organize and manage the registration of multiple builders.

## Features

- Organize builders in a specific order for object construction.
- Support for sorting builders in ascending or descending order.

## Installation

You can install the library via NuGet Package Manager:

```bash
dotnet add package Ogu.Extensions.BuilderOrganizer
```

## Usage

```csharp
public static BuilderOrganizer<IApplicationBuilder> Applications { get; internal set; } = new BuilderOrganizer<IApplicationBuilder>(ascending: false);

public void ConfigureServices(IServiceCollection services)
{
    Applications.Add(4, app => app.UseRouting(), "UseRouting");
    Applications.Add(0.9, app => app.UseAuthentication(), "UseAuthentication");
    Applications.Add(0.8, app => app.UseAuthorization(), "UseAuthorization");
    Applications.Add(0.7, app => app.UseEndpoints(endpoints => endpoints.MapControllers()), "UseEndpoints");
}

public void Configure(IApplicationBuilder app)
{
    Applications.Build(app);
}
```
