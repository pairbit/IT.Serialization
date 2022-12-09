# IT.Serialization
[![NuGet version (IT.Serialization)](https://img.shields.io/nuget/v/IT.Serialization.svg)](https://www.nuget.org/packages/IT.Serialization)
[![NuGet pre version (IT.Serialization)](https://img.shields.io/nuget/vpre/IT.Serialization.svg)](https://www.nuget.org/packages/IT.Serialization)

Interfaces of serialization

## Serialize/Deserialize

```csharp
    private void SerializeDeserialize(ISerializer serializer, Person person)
    {
        var serialized = serializer.Serialize(person);
        var person2 = serializer.Deserialize<Person>(serialized);
        if (!person.Equals(person2)) throw new InvalidOperationException();
    }

    private void SerializeDeserialize(ISerializer<Person> serializer, Person person)
    {
        var serialized = serializer.Serialize(person);
        var person2 = serializer.Deserialize(serialized);
        if (!person.Equals(person2)) throw new InvalidOperationException();
    }

    private void SerializeToTextDeserialize(ITextSerializer serializer, Person person)
    {
        var serialized = serializer.SerializeToText(person);
        var person2 = serializer.Deserialize<Person>(serialized.AsMemory());
        if (!person.Equals(person2)) throw new InvalidOperationException();
    }

    private void SerializeToTextDeserialize(ITextSerializer<Person> serializer, Person person)
    {
        var serialized = serializer.SerializeToText(person);
        var person2 = serializer.Deserialize(serialized.AsMemory());
        if (!person.Equals(person2)) throw new InvalidOperationException();
    }
```