# Documentation

[README](../README.md)

1. [Introduction](Introduction.md)
2. [Command Line Values](CommandLine.md)
3. [Custom Sources Including Databases](CustomSources.md)
4. [Validators and Coercers](ValidatorsAndCoercers.md)
5. [Dependency Injection](DependencyInjection.md)

## Dependency Injection

The design of Configgy works naturally with dependency injection. This is especially true if you use an interface to define your configuration properties, which is highly recommended. (For the purposes of this example I'm assuming you're using [Autofac](http://docs.autofac.org/en/stable/getting-started/index.html) - a popular IoC container but the principles here will work with any half-decent IoC library.) First you'll have your configuration interface:

```csharp

using System;

public interface IMyConfig
{   
    int MaxThingCount { get; }        
    string DatabaseConectionString { get; }        
    DateTime WhenToShutdown { get; }
}


```

You'll also need your configuration class:

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

Then somewhere you'll have a function like this to set up your IoC container:

```csharp

public IContainer GetContainer()
{
    var builder = new ContainerBuilder();
    
    builder.RegisterType<MyConfig>()
        .As<IMyConfig>()
        .SingleInstance();
       
    return builder.Build();
}

```

Be careful to make sure the config instance is bound as a singleton or you'll get no benefit from the value caching. In the case of AutoFac that is accomplished with the call to `SingleInstance()`;


### Advanced 1: Multiple Configuration Interfaces

One of the great things about this approach is that you can have a single config object that implements multiple configuration interfaces form separate services or objects used by your code. This could be useful if you need to connect to multiple different databases or web services or simply want to isolate the configuration of multiple areas of your application. This approach, combined with dependency injection might look something like this:

You'll have a configuration interface for each service. So one might be for a databse:

```csharp

using System;

public interface IMyDatabaseConfig
{
    int MaxThingCount { get; }
    DateTime WhenToShutdown { get; }
}


```

And you'll have other interfaces for other sections of your code. One could simply be a class that implements your business logic:

```csharp

using System;

public interface IMyLogicConfig
{   
    string DatabaseConectionString { get; } 
}


```

You'll also need your configuration class, which will implement both intefaces like this:

```csharp

using System;
using Configgy;

public class MyConfig: Config, IMyDatabaseConfig, IMyLogicConfig
{   
    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
}

```

Then you'll bind the config class to both interfaces when you set up your IoC container like so:

```csharp

public IContainer GetContainer()
{
    var builder = new ContainerBuilder();
    
    builder.RegisterType<MyConfig>()
        .As<IMyDatabaseConfig>()
        .As<IMyLogicConfig>()
        .SingleInstance();
        
    // TOOD: Wire up everything else
       
    return builder.Build();
}

```

Again, we're careful to bind our configuration object as a singleton, but using this approach that same singleton can provide configuration values to discrete areas of our code without having to provide them configuration values they neither need nor should even know exist.

### Advanced 2: Dependency Injection and Database Configuration Sources

Using dependency injection can simplify the database-as-configuration-source issue covered in the advanced section of [Custom Sources Including Databases](CustomSources.md). It will allow us to encapsulate the logic to do the bootstrapping into a single self-contained function.

We'll still need need a simple interface for our bootstrap configuration:

```csharp

public interface IMyBootstrapConfig
{
    string DatabaseConectionString { get; }
}


```

And we still need to implement out database configuration source:

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

We also need the full configuration interface, but this time we're going use the technique above to isolate the properties in each interface from each other:

```csharp

public interface IMyConfig
{
    int MaxThingCount { get; }
    DateTime WhenToShutdown { get; }
}


```

And we still have basically the same configuration object, except it implements two interfaces now:

```csharp

public class MyConfig: IMyConfig, IMyBootstrapConfig
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

Now our program is going to do nothing but request the config from the container, but the setup of the container will look like this:

```csharp


public void Main(string[] args)
{
    using (var container = GetContainer())
    {
        var config = container.Resolve<IMyConfig>();
        config.Validate();
    
        var logic = container.Resolve<IMyLogic>();
        logic.DoSomething();
    }
    
}

public IContainer GetContainer()
{
    var builder = new ContainerBuilder();
        
    builder.Register<MyConfig>(c => new MyConfig(new MyConfig()))
        .As<IMyConfig>()
        .SingleInstance();
        
    // TOOD: Wire up everything else
       
    return builder.Build();
}

```

This registers a callback that first creates out bootstrapping version of MyConfig using the default controller then passes that into the second constructor of MyConfig to create a full version of our config object. That full version is what is returned. And when the program runs the `Validate()` method and later uses the configuration inside the logic class it is only ever aware of the full configuration object. Note that the logic class here is also being returned from the IoC. In this way it it getting the config instance injected to it automatically.