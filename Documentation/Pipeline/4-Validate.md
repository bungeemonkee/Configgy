# Configgy

[README](../README.md)

1. [Overview](1-Overview.md)
    1. [Cache](Pipeline/1-Cache.md)
    2. [Source](Pipeline/2-Source.md)
    3. [Transform](Pipeline/3-Transform.md)
    4. [Validate](Pipeline/4-Validate.md)
    5. [Coerce](Pipeline/5-Coerce.md)

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

## Regex Validation

There is also a validator built in to validate any value by matching it to a regular expression (`Configgy.Validation.RegexValidatorAttribute`). Simply apply this attribute to the property to be validated.

## Custom Validators

As with any part of Configgy custom validators can be written. The ideal approach is to create a class that inherits from `Configgy.Validation.ValueValidatorAttributeBase` and apply that validator as an attribute to the property to be validated.