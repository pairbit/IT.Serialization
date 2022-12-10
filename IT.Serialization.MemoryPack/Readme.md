# IT.Serialization.Json
[![NuGet version (IT.Serialization.Json)](https://img.shields.io/nuget/v/IT.Serialization.Json.svg)](https://www.nuget.org/packages/IT.Serialization.Json)
[![NuGet pre version (IT.Serialization.Json)](https://img.shields.io/nuget/vpre/IT.Serialization.Json.svg)](https://www.nuget.org/packages/IT.Serialization.Json)

Implementation of json serialization via System.Text.Json

## ITextSerializer

```csharp
    private static IT.Serialization.ITextSerializer GetJsonSerializer(Microsoft.Extensions.Options.IOptions<System.Text.Json.JsonSerializerOptions> options)
    {
        return new IT.Serialization.Json.TextSerializer(() => options.Value);
    }
```