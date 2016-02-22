# Documentation

[README](../README.md)

1. [Introduction](Introduction.md)
2. [Command Line Values](CommandLine.md)
3. [Custom Sources Including Databases](CustomSources.md)
4. [Validators and Coercers](ValidatorsAndCoercers.md)

## Validators and Coercers

While many default validators and coercers exist there are also optional ones. Nearly all validators and coercers are implemented as attributes you can apply to a property to change the validation and coercion behavior.

### Validators

When dealing with any type that is a number (or number like: DateTime, TimeSpan) you can apply a validator attribute to ensure that the value falls within an expected range or an expected list of values. For example to validate that an integer is in a certain range just use [`IntValidatorAttribute`](../Configgy/Source/Validation/IntValidatorAttribute.cs) like so:

```csharp

public class MyConfig: Config, IMyConfig
{   
    [IntValidator(10, 25)]
    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
}


```

This would force the value of MaxThingCount to be between 10 and 25 inclusive.

### Coercers

A coercer attribute will change the way the raw value is converted to the type you expect. The best example would be when you want a complex type to be generated from a json string. Just apply the [`JsonCoercerAttribute`](../Configgy/Source/Coercion/JsonCoercerAttribute.cs) to the property like this:

```csharp

public class MyConfig: Config, IMyConfig
{   
    public int MaxThingCount { get { return Get<int>(); } }        
    public string DatabaseConectionString { get { return Get<string>(); } }        
    public DateTime WhenToShutdown { get { return Get<DateTime>(); } }
    
    [JsonCoercer]
    public MyConfigValueType ConfigValue { get return Get<MyConfigValueType>(); } }
}

```

## Custom Validators and Coercers

Any custom validator or coercer simply need be a property attribute that implements [`IValueValidator`](../Configgy/Validation/IValueValidator.cs) or [`IValueCoercer`](../Configgy/Coercion/IValueCoercer.cs) respectively. It is reccomended that you inherit from [`ValueValidatorAttributeBase`](../Configgy/Validation/ValueValidatorAttributeBase.cs) or [`ValueCoercerAttributeBase`](../Configgy/Coercion/ValueCoercerAttributeBase.cs) respectively. Then just apply your custom validator or coercer attribute to the property.