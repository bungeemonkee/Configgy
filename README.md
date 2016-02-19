# Configgy

A simple, powerful, extensible, testable .NET configuration library.

## Intro

Configgy is the last .NET configuration library you'll ever need. It is designed to support configuration values coming from any source, with any kind of validation, and then expsoe it all as strong types - even complex types like lists, dictionaries, or general .NET objects.

## Design

The basic design of Configgy boils down to a few key points:

  * Get configuration values from anywhere. (Easily extensible configuration sources.)
  * Validate any configuration value in any way. (Easily extensible configuration validators.)
  * Support any strongly typed configuration value, including complex objects! (Easily extensible configuration type coercers.)
  * Test all the things! (Modular/testable design.)

## Features

Here are a bunch of things supported by Configgy out of the box:

  * Strongly typed configurations including complex objects
      * Any value type you can imagine including enums, numerics, DateTime, TimeSpan and strings
      * JSON for complex objects (preferred)
      * XML for complex objects (not reccomended but it works)
  * Configuration sources
      * App setting entries in a web/app config
      * Connection string entries in a web/app config
      * Command line switches (trust me this bit is swanky)
      * System.ComponentModel.DefaultValueAttribute
      * Environment variables
  * Validation
      * Automatic validation for all numeric types, DateTime, TimeSpan, and by regex
      * Validation of any numeric, DateTime, or TimeSpan configuration properties by min/max or valid value arrays
      * Validation of any configuration property by regular expression
  * Caching
      * In-memory caching of post-coercion values to reduce reflection by default and make most config value lookups lightning fast
  * Optional preemptive validation - know that a config value is bad when your app starts up (or any other time you choose) instead of hours or days later when you get some inscrutable null reference exception or an unexplained int.Parse error.

## Extensibility

Here are a bunch of things that are really easy to do because of the Configgy design

  * Pull your configuration values from any database, web service or other source you can imagine
  * Add your own validators to prevent/allow only certain enum values, strings, or complex object values
  * Write your own value source to change the command line configuration option syntax if you don't like the kick-ass one I came up with
  * Cache your config values to somewhere besides memory, maybe Redis or MemCached, or an instance of System.Runtime.Caching.MemoryCache
  * Coerce values into the wrong types to annoy your coworkers!

## TODO

So far this is like really really pre-alpha and mostly untested but it is tehcnically feature-complete per the above feature list. That being said here is what's missing:

  * Unit tests
  * Inline documentation
  * Reference documentation and code samples
  * Add support for Type objects as configuration values