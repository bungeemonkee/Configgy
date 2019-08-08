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

## Pipeline - Source

The second stage of the pipeline is the value source. It is the job of the value source to provide the raw configuration values as strings. If the source does not provide any value then an exception will be thrown.

All source classes must implement `Configgy.Source.IValueSource`.

The default source is an instance of `Configgy.Source.AggregateSource` which uses several other source implementations to pull config values from the following locations (in order of precedence):

1. Command line (if a command line is provided in the constructor)
2. Environment variables
3. Files
4. Net Core appsettings.json files
5. Connection strings (in an app/web config)
6. App settings (in an app/web config)
7. Embedded resources
8. Default value attributes

These sources (or others) can be excluded from consideration on a per-property basis using by applying `Configgy.PreventSourceAttribute` to the property. This source takes a [`System.Type`](https://msdn.microsoft.com/en-us/library/system.type(v=vs.110).aspx) reference which will be ignored by `Configgy.Source.AggregateSource` when considering potential values sources. This property can be applied multiple times to ignore multiple sources. This is useful to prevent sensitive values (which must come from a secure environment) from being retrieved accidentally from a command line or config file.

### Command Line

To make your config aware of the command line you need to pass the command line into the config object through the constructor.

Then you can override any config value on the command line by specifying it as a long-form switch.
For example: `MyProgram.exe --MaxThingCount=25`.

Using this syntax you can also specify any boolean properties with no value and the value "True" will be assumed.
For example: `MyProgram.exe --DoSomething=True` and `MyProgram.exe --DoSomething` are equivalent.

To set a boolean value to false you must explicitly specify the value "False".
Ie. `MyProgram.exe --DoSomething=False`.

Additionally a second syntax is supported by using the `/c` or `/config` command line switch.
For example: `MyProgram.exe /config:MaxThingCount=25`.

This way you could have a default value of 50 for MaxThingCount in the app.config, a value configured in the environment of 15, and still override both of those values on the command line.

If you'd like the command line variable name to be different than that of the actual property name decorate the property with an instance of `Configgy.Source.CommandLineNameAttribute` like this `[CommandLineNmae("some_other_name")]` note that the double dashes are not included in the substitute name.

### Environment Variables

Environment variables as a config source are generally easy except that after you set the variable you may need to restart the application for the change to take effect. If your application is running inside another service (such as a web application running inside IIS) you may even need to restart the computer itself for the new variable to propagate all the way into your process.

### Files

To use separate files for each config value simply create a text file with the same name as the config value and one of the following extensions: '.conf', '.json', '.xml'. Note that the file extension does not change the coercer that is used. So a configuration file with a '.json' extension may still contain xml that is parsed using the xml coercer if that coercer attribute is applied to the configuration property.

### Net Core appsettings.json Files

These work almost exactly like the default setup from the new Net Core MVC project template. The files are searched for in the current directory. If there is a file named appsettings.<environment>.json it will override settings in app.json. In this case <environment> will be the value of the ASPNETCORE_ENVIRONMENT environment variable, or if that is not found the ENVIRONMENT environment variable.

### Connection Strings

Nearly every .NET app has an App.config or Web.Config file which may contain a section with connection strings. Any connection strings with names that match a configuration property will be pulled in a a raw value for that configuration property.

### App Settings

Just as configuration strings from config files may be used any settings in the app settings section of these files may be used as well. If the name of the setting matches the desired configuration property then the value of the setting will be used as a raw value for that property.

### Embedded Resources

Embedded resources can be used to contain configuration information in much the same way as the files above. Any embedded resource with a file extension of '.conf', '.config', '.txt', '.json', or '.xml' will be used as as raw source for any configuration property matching the file name. Embedded resources must also be in a namespace (which the compiler determines form the folder it is in) of 'Resources.Settings' or they will not be considered. So for a setting called 'MaxThingCount' an embedded resource in the folder '[ProjectFolder]/Resources/Settings/MaxThingCount.config' would be a potential raw value.

### Default Value Attributes

Instances of [`System.ComponentModel.DefaultValueAttribute`](https://msdn.microsoft.com/en-us/library/system.componentmodel.defaultvalueattribute(v=vs.110).aspx) may be placed on configuration properties to give them a default value if no other value is provided.