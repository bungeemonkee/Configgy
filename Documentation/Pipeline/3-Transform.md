# Configgy

[README](../../README.md)

1. [Overview](../1-Overview.md)
    1. [Cache](1-Cache.md)
    2. [Source](2-Source.md)
    3. [Transform](3-Transform.md)
    4. [Validate](4-Validate.md)
    5. [Coerce](5-Coerce.md)

## Pipeline - Transform

The third pipeline step is the transform step. This step allows raw configuration values to be altered before being validated and coerced into the correct type.

All value transformers must implement `Configgy.Transformers.IValueTransformer`.

The default transformer is `Configgy.Transformers.AggregateTransformer`. By default this transformer does nothing, however it can be given one or more transformers to run on every value in its constructor. Additionally, it will search for any transformer attribute on the config property being accessed and will run those transformations.

### A Note Of Explanation

The primary use case for this behavior is implemented in `Configgy.Transformers.DecryptionTransformerAttribute`. This class allows values to be stored in the value source as base-64 encoded, RSA encrypted, UTF-8 strings. The transformer will decode and decrypt the results and return the original string. In this way secure information can safely be stored in any configuration source and still accessed easily (and cached, and validated, and coerced) using Configgy but only accessed on any machine which contains the appropriate certificate for the necessary decryption.