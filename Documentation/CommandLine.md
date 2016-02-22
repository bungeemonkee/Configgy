# Documentation

[README](../README.md)

1. [Introduction](Introduction.md)
2. [Command Line Values](CommandLine.md)
3. [Custom Sources Including Databases](CustomSources.md)

## Command Line Values

Configgy allows the inclusion of command line values as a source for configuration settings. Values given on the command line will override the value given in any other source. In fact Configgy by default allows any configuration source to override any other. The order of precedence is as follows:

  * Command line
  * Environment variables
  * Connection strings (in web/app config)
  * App settings (in web/app config)
  * DefaultValueAttribute

So if you include a connection string in your app.config file you can override it by setting a different value for the same name in the environment. But you can then override that for a one-off execution by using the command line.

To make your config aware of the command line you need to pass the command line into the config object through the constructor. Lets extend our example from the [Introduction](Introduction.md) to include getting values form the command line:

```csharp

using System;
using Configgy;

public class MyConfig: Config, IMyConfig
{
    public int MaxThingCount { Get<int>(); }        
    public string DatabaseConectionString { Get<string>(); }        
    public DateTime WhenToShutdown { Get<DateTime>(); }
    
    public MyConfig(string[] commandLine)
        : base (commandLine)
    {
    }
}

```

And then we change the program like this:

```csharp

public void Main(string[] args)
{
    var config = new MyConfig(args);
    config.Validate();
    var logic = new MyLogicClass(config);
    logic.DoSomething();
}

```

Easy peasy!

So we've added a new constructor to `MyConfig` that simply passes the command line to the appropriate constructor in the `Config` base class.

Now we can override any config value on the command line by using the `/c` or `/config` command line switch like this: `MyProgram.exe /config:MaxThingCount=25`. This way we could have a default value of 50 for MaxThingCount in the app.config, a value configured in the environment of 15, and still override both of those values on the command line.

Generally speaking this should not be used as the sole command line parser for programs which require user input with every run. This should be used in the exceptional case where  configured value needs to be overridden.

### Advanced: Restricting Command Line Overriding

The intended use here is for developers writing back-end processes or tools for their own use (that's my primary use case). In which case being able to override any config value is really useful. But if you want to distribute a program that allows command line overrides to users but restricts which values they can override you should have a look at the source for [`CommandLineSource`](../Configgy/Source/CommandLineSource.cs). You can add a list of allowed override properties to this class in several ways:

  * A hard coded list :(
  * A constructor parameter :|
  * An attribute you can apply to the properties you want to be override-able :)

Once you have your own implementation of [`IValueCoercer`](../Configgy/Coercion/IValueCoercer.cs) you'll have to change your configuration class to use the new source like this:

```csharp

using System;
using Configgy;
using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Validation;

public class MyConfig: Config, IMyConfig
{

    public int MaxThingCount { get { return Get<int>(); }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    public MyConfig(string[] commandLine)
        : base (
            new DictionaryCache(),
            new AggregateSource(
              new MyCommandLineSource(commandLine),
              new EnvironmentVariableSource(),
              new ConectionStringsSource(),
              new AppSettingSource(),
              new DefaultValueAttributeSource()
            ),
            new AggregateValidator(),
            new AggregateCoercer()
        )
    {
    }
}

```

Really all this is doing is overriding the default value source for your config class to a new one that uses your overridden command line source class. To really understand this you should look at the constructors for [`Config`](../Configgy/Config.cs) and [`AggregateSource`](../Configgy/Source/AggregateSource.cs).