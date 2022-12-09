# IT.Serialization.MessagePack
[![NuGet version (IT.Serialization.MessagePack)](https://img.shields.io/nuget/v/IT.Serialization.MessagePack.svg)](https://www.nuget.org/packages/IT.Serialization.MessagePack)
[![NuGet pre version (IT.Serialization.MessagePack)](https://img.shields.io/nuget/vpre/IT.Serialization.MessagePack.svg)](https://www.nuget.org/packages/IT.Serialization.MessagePack)

Implementation of binary serialization via MessagePack

## ISerializer

```csharp
    private static IT.Serialization.ISerializer GetMessagePackSerializer()
    {
        return new IT.Serialization.MessagePack.Serializer(MessagePackSerializerOptions.Standard);
    }
```