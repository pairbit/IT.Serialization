using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization.Generic;

public static class xIDeserializerGeneric
{
    public static T? Deserialize<T>(this IDeserializer<T> deserializer, Stream stream, CancellationToken cancellationToken = default)
    {
        T? value = default;
        deserializer.Deserialize(stream, ref value, cancellationToken);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, ReadOnlySpan<byte> span)
    {
        T? value = default;
        deserializer.Deserialize(span, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, ReadOnlyMemory<byte> memory)
    {
        T? value = default;
        deserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, byte[] array)
    {
        T? value = default;
        deserializer.Deserialize(new Memory<byte>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, in ReadOnlySequence<byte> sequence)
    {
        T? value = default;
        deserializer.Deserialize(in sequence, ref value);
        return value;
    }
}