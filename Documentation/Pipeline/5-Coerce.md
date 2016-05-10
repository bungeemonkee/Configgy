# Configgy
 
[README](../../README.md)

1. [Overview](../1-Overview.md)
    1. [Cache](1-Cache.md)
    2. [Source](2-Source.md)
    3. [Transform](3-Transform.md)
    4. [Validate](4-Validate.md)
    5. [Coerce](5-Coerce.md)

## Pipeline - Coerce

The fifth and final pipeline step is coercion. This is the step where a raw, transformed, validated value is converted to a usable object.

The interface for all coercers is `Configgy.Coercion.IValueCoercer`.

The default coercer is `Configgy.Coercion.AggregateCoerver`. This coercer uses three coercers by default:

1. One can create instances of `System.Text.RegularExpressions.Regex` (by parsing the string using the regular Regex constructor)
2. One can create instances of `System.Type` (by looking up a type using a raw string value representing the type name)
3. The other can parse all primitive types and nearly every other type built into .NET by using the [`TypeConverter`](https://msdn.microsoft.com/en-us/library/system.componentmodel.typeconverter%28v=vs.110%29.aspx) instances provided by [`TypeDescriptor.GetConverter()`](https://msdn.microsoft.com/en-us/library/w202c8fy%28v=vs.110%29.aspx). In order to allow this coercer to successfully parse any custom types you can [implement a custom `TypeDescriptor`](https://msdn.microsoft.com/en-us/library/ms171819.aspx?f=255&MSPPError=-2147217396) for that type.

## CSV Properties

For properties that are simple arrays of objects there is a coercer that can parse these lists: `Configgy.Coercion.CsvCoercerAttribute`. This coercer returns an array of the parsed objects of the given type. Simply apply the attribute to the array property. The property could also be of type `IEnumerable`, `IEnumerable<T>
    `, `IList`, or `IList<T>
        ` as arrays can be cast to all these types.

        ## Json/Xml Properties

        For properties that are complex objects Configgy provides coercers that can take raw values in either json or xml and deserialize complex objects from them. __Json is highly recommended over xml__ due to the facts that json is simpler, more compact, and the `System.Runtime.Serialization.DataContractSerializer` class which is used internally has many (often undocumented) idiosyncrasies.
