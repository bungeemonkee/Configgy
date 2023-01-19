# Configgy

![Configgy: The Last Configuration Library for .NET](https://raw.githubusercontent.com/bungeemonkee/Configgy/master/icon.png)

A simple, powerful, extensible, testable .NET configuration library.

[![Build Status](https://github.com/bungeemonkee/Configgy/actions/workflows/dotnet.yml/badge.svg)](https://github.com/bungeemonkee/Configgy) [![NuGet Version](https://img.shields.io/nuget/v/Configgy.svg?maxAge=3600)](https://www.nuget.org/packages/Configgy)

## Documentation

[README](README.md)

1. [Overview](Documentation/1-Overview.md)
    1. [Cache](Documentation/Pipeline/1-Cache.md)
    2. [Source](Documentation/Pipeline/2-Source.md)
    3. [Transform](Documentation/Pipeline/3-Transform.md)
    4. [Validate](Documentation/Pipeline/4-Validate.md)
    5. [Coerce](Documentation/Pipeline/5-Coerce.md)
2. [Other Features](Documentation/2-Other.md)
3. [Advanced Usage](Documentation/3-Advanced.md)


## Description

Configgy is the last .NET configuration library you'll ever need. It is designed to support configuration values coming from any source, with any kind of validation, and then expose it all as strong types - even complex types like lists, dictionaries, or general .NET objects.

## Usage

The simplest usage of Configgy is to inherit from `Configgy.Config` and add the configuration properties you want like this:

```csharp

using System;
using Configgy;

public class MyConfig: Config, IMyConfig
{   
    [DefaultValue(100)] //assign a default value.
    public int MaxThingCount { get { return Get<int>(); } }
    [DefaultValue("Server=server;Database=db;User Id=usr;Password=pwd;")] //assign a default value.
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    //use expression bodied statements
    public int MinThingCount => Get<int>();
}

```

## Installation

You can build from this source or you can get it from nuget here: [https://www.nuget.org/packages/Configgy](https://www.nuget.org/packages/Configgy)

## Design

The basic design of Configgy boils down to a few key points:

* Get configuration values from anywhere. (Easily extensible configuration sources.)
* Transform raw values before validation or coercion. (Easily extensible value transformers.)
* Validate any configuration value in any way. (Easily extensible configuration validators.)
* Support any strongly typed configuration value, including complex objects! (Easily extensible configuration type coercers.)
* Test all the things! (Modular/testable design. Current unit test coverage above 90%)

## Features

Here are a bunch of things supported by Configgy out of the box:

* Strongly typed configuration properties including complex objects
    * Any value type you can imagine including enums, numerics, [`DateTime`](https://msdn.microsoft.com/en-us/library/system.datetime(v=vs.110).aspx), [`TimeSpan`](https://msdn.microsoft.com/en-us/library/system.timespan(v=vs.110).aspx) and strings
    * JSON for complex objects (preferred)
    * XML for complex objects (not recommended but it works)
* Configuration sources
    * Command line switches (trust me this bit is swanky)
    * Environment variables
    * Files (named like the conf value you're looking for)
    * Net Core appsettings.json files
    * Connection string entries in a web/app config
    * App setting entries in a web/app config
    * Embedded resources (just like the files above but embedded in the app)
    * [`System.ComponentModel.DefaultValueAttribute`](https://msdn.microsoft.com/en-us/library/system.componentmodel.defaultvalueattribute(v=vs.110).aspx)
* Value transformers
    * Convert encrypted strings (RSA encrypted then base-64 encoded) into plaintext.
    * Convert relative paths to absolute ones
    * Convert strings to upper or lower case
* Validation
    * Automatic validation for all numeric types, [`DateTime`](https://msdn.microsoft.com/en-us/library/system.datetime(v=vs.110).aspx), TimeSpan
    * Validation of any numeric, [`DateTime`](https://msdn.microsoft.com/en-us/library/system.datetime(v=vs.110).aspx), or [`TimeSpan`](https://msdn.microsoft.com/en-us/library/system.timespan(v=vs.110).aspx) configuration properties by min/max or valid value arrays
    * Validation of any configuration property by regular expression
* Caching
    * In-memory caching of post-coercion values to reduce reflection by default and make most config value lookups lightning fast
* Optional preemptive validation - know that a config value is bad when your app starts up (or any other time you choose) instead of hours or days later when you get some inscrutable null reference exception or an unexplained int.Parse error.
* Populate any configuration object even ones provided by thridparty libraries using the same `Configgy.ConfigProvider` instance - so just configure it once
* Dependency Injection - an interface for everything and everything has an interface. You can assemble an entire `Configgy.ConfigProvider` instance with DI if you wish

## Extensibility

Here are a bunch of things that are really easy to do because of the Configgy design

* Encrypt sensitive settings such as api keys, connection strings, passwords, etc.
* Pull your configuration values from any database, web service or other source you can imagine.
* Add your own validators to prevent/allow only certain enum values, strings, or complex object values.
* Write your own value source to change the command line configuration option syntax if you don't like the kick-ass one I came up with.
* Cache your config values to somewhere besides memory, maybe Redis or MemCached, or an instance of [`System.Runtime.Caching.MemoryCache`](https://msdn.microsoft.com/en-us/library/system.runtime.caching.memorycache(v=vs.110).aspx).
* Coerce values into the wrong types to annoy your coworkers!

## Bugs And Feature Requests

Any TODO items, feature requests, bugs, etc. will be tracked as GitHub issues here:
[https://github.com/bungeemonkee/Configgy/issues](https://github.com/bungeemonkee/Configgy/issues)

## Thanks

Thanks to Alex Bielen for the awesome logo!
