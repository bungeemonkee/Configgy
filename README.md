# Configgy

![Configgy: The Last Configuration Library for .NET](https://raw.githubusercontent.com/bungeemonkee/Configgy/master/icon.png)

A simple, powerful, extensible, testable .NET configuration library.

[![Build Status](https://ci.appveyor.com/api/projects/status/64w2omp3rf0sa1hx?svg=true)](https://ci.appveyor.com/project/bungeemonkee/configgy) [![Coverage Status](https://coveralls.io/repos/github/bungeemonkee/Configgy/badge.svg?branch=master)](https://coveralls.io/github/bungeemonkee/Configgy?branch=master) [![NuGet Version](https://img.shields.io/nuget/v/Configgy.svg?maxAge=3600)](https://www.nuget.org/packages/Configgy)

## Documentation

[README](README.md)

1. [Overview](Documentation/1-Overview.md)
    1. [Cache](Documentation/Pipeline/1-Cache.md)
    2. [Source](Documentation/Pipeline/2-Source.md)
    3. [Transform](Documentation/Pipeline/3-Transform.md)
    4. [Validate](Documentation/Pipeline/4-Validate.md)
    5. [Coerce](Documentation/Pipeline/5-Coerce.md)


## Description

Configgy is the last .NET configuration library you'll ever need. It is designed to support configuration values coming from any source, with any kind of validation, and then expose it all as strong types - even complex types like lists, dictionaries, or general .NET objects.

## Usage

The simplest usage of Configgy is to inherit from `Configgy.Config` and add the configuration properties you want like this:

```csharp

using System;
using Configgy;

public class MyConfig: Config, IMyConfig
{   
    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
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

* Strongly typed configurations including complex objects
    * Any value type you can imagine including enums, numerics, DateTime, TimeSpan and strings
    * JSON for complex objects (preferred)
    * XML for complex objects (not recommended but it works)
* Configuration sources
    * Command line switches (trust me this bit is swanky)
    * Environment variables
    * Files (named like the conf value you're looking for)
    * Connection string entries in a web/app config
    * App setting entries in a web/app config
    * System.ComponentModel.DefaultValueAttribute
* Value transformers
    * Convert encrypted strings (RSA encrypted then base-64 encoded) into plaintext.
* Validation
    * Automatic validation for all numeric types, DateTime, TimeSpan
    * Validation of any numeric, DateTime, or TimeSpan configuration properties by min/max or valid value arrays
    * Validation of any configuration property by regular expression
* Caching
    * In-memory caching of post-coercion values to reduce reflection by default and make most config value lookups lightning fast
* Optional preemptive validation - know that a config value is bad when your app starts up (or any other time you choose) instead of hours or days later when you get some inscrutable null reference exception or an unexplained int.Parse error.

## Extensibility

Here are a bunch of things that are really easy to do because of the Configgy design

* Encrypt sensitive settings such as api keys, connection strings, passwords, etc.
* Pull your configuration values from any database, web service or other source you can imagine.
* Add your own validators to prevent/allow only certain enum values, strings, or complex object values.
* Write your own value source to change the command line configuration option syntax if you don't like the kick-ass one I came up with.
* Cache your config values to somewhere besides memory, maybe Redis or MemCached, or an instance of `System.Runtime.Caching.MemoryCache`.
* Coerce values into the wrong types to annoy your coworkers!

## Bugs And Feature Requests

Any TODO items, feature requests, bugs, etc. will be tracked as GitHub issues here:
[https://github.com/bungeemonkee/Configgy/issues?utf8=%E2%9C%93&q=is%3Aopen+](https://github.com/bungeemonkee/Configgy/issues?utf8=%E2%9C%93&q=is%3Aopen+)

## Thanks

Thanks to Alex Bielen for the awesome logo!