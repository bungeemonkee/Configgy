# Documentation

[README](../README.md)

1. [Introduction](Introduction.md)
2. [Command Line Values](CommandLine.md)
3. [Custom Sources Including Databases](CustomSources.md)
4. [Validators and Coercers](ValidatorsAndCoercers.md)
5. [Dependency Injection](DependencyInjection.md)


## Custom Sources

Configgy is designed so custom sources can be added easily. To do this you'll need to implement [`IValueSource`](../Configgy/Source/IValueSource.cs) and add it to the sources in your config class. That would look something like this:

```csharp

using System;
using Configgy;
using Configgy.Cache;
using Configgy.Coercion;
using Configgy.Source;
using Configgy.Validation;

public class MyConfig: Config, IMyConfig
{

    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    public MyConfig(string[] commandLine)
        : base (
            new DictionaryCache(),
            new AggregateSource(
              new CommandLineSource(commandLine),
              new EnvironmentVariableSource(),
              new MyCustomSource(),
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

Notice how similar this is to the customized command line source we added in the advanced section of the [CommandLine](CommandLine.md) documentation.

### Advanced: Databases

Databases (or webservices, or cache services, etc.) as configuration sources are where this gets a little tricky. That's because with databases you generally need some configuration info to connect to them. Which has the potential to introduce a circular dependency when dealing with getting the configuration from the database. So we need to bootstrap an initial configuration before building the full configuration.

We'll need a simple interface for our bootstrap configuration. This will only contain the config values we need to connect to the database like so:

```csharp

public interface IMyBootstrapConfig
{
    string DatabaseConectionString { get; }
}


```
We need to implement out database configuration source:

```csharp

public class MyDatabaseConfig : IValueSource
{
    public MyDatabaseConfig(IMyBootstrapConfig bootstrapConfig)
    {
        // TODO: Implement this constructor
    }
    
    public string GetRawValue(string valueName, PropertyInfo property)
    {
        // TODO: Implement getting the config value from the database
    }
}


```


We'll also need an interface for our full config object. This will inherit from our bootstrap config like this:

```csharp

public interface IMyConfig : IMyBootstrapConfig
{
    int MaxThingCount { get; }       
    DateTime WhenToShutdown { get; }
}


```

Now our config object will implement the main config interface. But we're going to have two constructors, one that builds the boostrap config and one that takes the bootstrap config and uses it to build the full config with database support. It'll look like this: (I'm not including command line support just to simplify this example but you should see where it would fit in.)

```csharp

public class MyConfig: IMyConfig
{

    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    public MyConfig()
    {
    }
    
    public MyConfig(IMyBootstrapConfig bootstrapConfig)
        : base (
            new DictionaryCache(),
            new AggregateSource(
              new EnvironmentVariableSource(),
              new MyDatabaseConfig(bootstrapConfig),
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

Now our program will have to build the bootstrap configuration then use that to build the full configuration. We only want to validate the full configuration though. It would look like this: (Again, command line support has been excluded to simplify the example.)


```csharp

public void Main(string[] args)
{
    var bootstrap = new MyConfig();
    var config = new MyConfig(bootstrap);
    config.Validate();
    
    var logic = new MyLogicClass(config);
    logic.DoSomething();
}

```