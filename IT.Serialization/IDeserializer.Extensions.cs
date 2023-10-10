using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public static class xIDeserializer
{
    #region Generic

    public static T? Deserialize<T>(this IDeserializer deserializer, Stream stream, CancellationToken cancellationToken = default)
    {
        T? value = default;
        deserializer.Deserialize(stream, ref value, cancellationToken);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, ReadOnlySpan<byte> span)
    {
        T? value = default;
        deserializer.Deserialize(span, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, ReadOnlyMemory<byte> memory)
    {
        T? value = default;
        deserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, byte[] array)
    {
        T? value = default;
        deserializer.Deserialize(new ReadOnlyMemory<byte>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, in ReadOnlySequence<byte> sequence)
    {
        T? value = default;
        deserializer.Deserialize(in sequence, ref value);
        return value;
    }

    #endregion Generic

    #region NonGeneric

    public static object? Deserialize(this IDeserializer deserializer, Type type, Stream stream, CancellationToken cancellationToken = default)
    {
        object? value = default;
        deserializer.Deserialize(type, stream, ref value, cancellationToken);
        return value;
    }

    public static object? Deserialize(this IDeserializer deserializer, Type type, ReadOnlySpan<byte> span)
    {
        object? value = default;
        deserializer.Deserialize(type, span, ref value);
        return value;
    }

    public static object? Deserialize(this IDeserializer deserializer, Type type, ReadOnlyMemory<byte> memory)
    {
        object? value = default;
        deserializer.Deserialize(type, memory, ref value);
        return value;
    }

    public static object? Deserialize(this IDeserializer deserializer, Type type, byte[] array)
    {
        object? value = default;
        deserializer.Deserialize(type, new ReadOnlyMemory<byte>(array), ref value);
        return value;
    }

    public static object? Deserialize(this IDeserializer deserializer, Type type, in ReadOnlySequence<byte> sequence)
    {
        object? value = default;
        deserializer.Deserialize(type, in sequence, ref value);
        return value;
    }

    #endregion NonGeneric
}