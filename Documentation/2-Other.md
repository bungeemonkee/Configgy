# Configgy

[README](../README.md)

1. [Overview](1-Overview.md)
    1. [Cache](Pipeline/1-Cache.md)
    2. [Source](Pipeline/2-Source.md)
    3. [Transform](Pipeline/3-Transform.md)
    4. [Validate](Pipeline/4-Validate.md)
    5. [Coerce](Pipeline/5-Coerce.md)
2. [Other Features](2-Other.md)
3. [Advanced Usage](3-Advanced.md)

## Other Features

Configgy also has a handful of other features.

### Command Line Help Generation

Configgy contains extension methods which can generate human-readable documentation for any configuration properties documented with `Configgy.HelpAttribute`:

* `string Config.GetCommandLineHelp()`
* `string Config.GetCommandLineHelp(bool includeUndocumented)`
* `string Config.GetCommandLineHelp(bool includeUndocumented, int width)`

Options that have a `Configgy.PreventSourceAttribute` with the type value set to `Configgy.Source.DashedCommandLineSource` are never included in the output.

If `includeUndocumented` is `true` then even options that have no `Configgy.HelpAttribute` are included (but without any description).

If `width` is not specified then the value of [`Console.BufferWidth`](https://msdn.microsoft.com/en-us/library/system.console.bufferwidth(v=vs.110).aspx) is used if it can be retrieved. If not then 80 is used.
For a class that looks like this:

```csharp

using System;
using Configgy;

[Help("A program that does things with a database.")]
public class MyConfig: Config, IMyConfig
{   
    [Help("The maximum number of things to use.")]
    public int MaxThingCount { get { return Get<int>(); } }

    public string DatabaseConectionString { get { return Get<string>(); } }

    [Help("The time the service should stop running.")]        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
}

```

The output would look something like this:

```
Program.exe [OPTIONS]

A program that does things with a database.

--MaxThingCount=<int>
    The maximum number of things to use.

--WhenToShutdown=<DateTime>
    The time the service should stop running.

```

