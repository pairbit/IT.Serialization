# IT.Serialization.Utf8Json
[![NuGet version (IT.Serialization.Utf8Json)](https://img.shields.io/nuget/v/IT.Serialization.Utf8Json.svg)](https://www.nuget.org/packages/IT.Serialization.Utf8Json)
[![NuGet pre version (IT.Serialization.Utf8Json)](https://img.shields.io/nuget/vpre/IT.Serialization.Utf8Json.svg)](https://www.nuget.org/packages/IT.Serialization.Utf8Json)

Implementation of json serialization via Utf8Json from Cryptisk

## ITextSerializer

```csharp
    private static IT.Serialization.ITextSerializer GetUtf8JsonSerializer()
    {
        return new IT.Serialization.Utf8Json.TextSerializer(global::Utf8Json.Resolvers.StandardResolver.Default);
    }
```