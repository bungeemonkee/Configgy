# Configgy

[README](../README.md)

1. [Overview](1-Overview.md)
    1. [Cache](Pipeline/1-Cache.md)
    2. [Source](Pipeline/2-Source.md)
    3. [Transform](Pipeline/3-Transform.md)
    4. [Validate](Pipeline/4-Validate.md)
    5. [Coerce](Pipeline/5-Coerce.md)

## Overview

When any configuration property is accessed the the main `Configgy.Config` class executes a 5 stage pipeline. The stages are each represented by a single interface and are as follows:

1. [Cache](Pipeline/1-Cache.md) (`Configgy.Cache.IValueCache`)
2. [Source](Pipeline/2-Source.md) (`Configgy.Source.IValueSource`)
3. [Transform](Pipeline/3-Transform.md) (`Configgy.Transformers.IValueTransformer`)
4. [Validate](Pipeline/4-Validate.md) (`Configgy.Validation.IValueValidation`)
5. [Coerce](Pipeline/5-Coerce.md) (`Configgy.Coercion.IValueCoercer`)

The main `Configgy.Config` class either uses a default implementation of these interfaces or it uses the one you provide depending on the constructor you use. With the exception of the cache the default implementations are actually aggregators that utilize multiple other implementations of the same interface. The default cache implementation is a simple thread safe dictionary.

