# Configgy

[README](../../README.md)

1. [Overview](../1-Overview.md)
    1. [Cache](1-Cache.md)
    2. [Source](2-Source.md)
    3. [Transform](3-Transform.md)
    4. [Validate](4-Validate.md)
    5. [Coerce](5-Coerce.md)
2. [Other Features](../2-Other.md)
3. [Advanced Usage](../3-Advanced.md)

## Pipeline - Validate

The fourth pipeline step is validation. This step takes raw values and allows them to be validated. It is possible that as a result of validation the actual coerced value may be produced. In this case this step may also provide a coerced value to optimize away the coercion step (step #5).

The interface that all validators must implement is `Configgy.Validation.IValueValidator`.

The default validator is `Configgy.Validation.AggregateValidator`. If one of its available validators matches the type of the expected result then that validator is used, otherwise no validation is performed. It also applies any validators that exist as property attributes on the property requesting the value. It contains default validators for the following types:

* `byte`
* `char`
* `System.DateTime`
* `decimal`
* `double`
* `float`
* `long`
* `sbyte`
* `short`
* `System.TimeSpan`
* `uint`
* `ulong`
* `ushort`

### Range Validation

When dealing with any type that is a number (or number like: DateTime, TimeSpan) you can apply a validator attribute to ensure that the value falls within an expected range or an expected list of values. For example to validate that an integer is in a certain range just use [`IntValidatorAttribute`](../Configgy/Validation/IntValidatorAttribute.cs) like so:

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

### Regex Validation

There is also a validator built in to validate any value by matching it to a regular expression (`Configgy.Validation.RegexValidatorAttribute`). Simply apply this attribute to the property to be validated.

### Custom Validators

Any custom validator simply need be a property attribute that implements [`IValueValidator`](../Configgy/Validation/IValueValidator.cs). It is recommended that you inherit from [`ValueValidatorAttributeBase`](../Configgy/Validation/ValueValidatorAttributeBase.cs). Then just apply your custom validator attribute to the property.
