# Documentation

[README](../README.md)

1. [Introduction](Introduction.md)
2. [Command Line Values](CommandLine.md)
3. [Custom Sources Including Databases](CustomSources.md)
4. [Validators and Coercers](ValidatorsAndCoercers.md)

## Introduction

Configgy is designed to be easy to use (and extend). At its core there is only one class (`Configgy.Config`) which contains one main method (`Get<T>(string)`). The `Config` class is an abstract base class intended to be sub-classed to contain all the properties used as configuration inputs for your program. That would look something like this:

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

And that's it!

Using this in a console program might look like this:

```csharp

public void Main(string[] args)
{
    var config = new MyConfig();
    config.Validate();
    
    var logic = new MyLogicClass(config);
    logic.DoSomething();
}

```

First, note the use of an interface (`IMyConfig`). This is just highly recommended so you can test anything that uses these properties such as the `MyLogicClass` in the above example.

The neat bit is inside the `Get<T>(string)` function. The type parameter tells it what type to parse the value into. This should probably be the same as the type of the property. But notice that we're not actually passing a string to this function - it has a default parameter that sets it to null. But it also uses `CallerMemberNameAttribute` to know the name of the thing that is calling it. This is set by the compiler at compile time and involves no reflection. You should pretty much always leave this blank.

A note on reflection: there is some reflection that is done initially when the class constructor is called (but not a lot) to pre-load some properties. Then there may be some more reflection the first time each property is accessed to coerce values into the correct types. But after the first call to each property these values are cached in memory so there is no ongoing cost to performance.

