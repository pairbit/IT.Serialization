using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public static class xIDeserializer
{
    #region IDeserializer<T>

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, Stream stream, CancellationToken cancellationToken = default)
    {
        T? value = default;
        deserializer.Deserialize(stream, ref value, cancellationToken);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, ReadOnlySpan<Byte> span)
    {
        T? value = default;
        deserializer.Deserialize(span, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, ReadOnlyMemory<Byte> memory)
    {
        T? value = default;
        deserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, Byte[] array)
    {
        T? value = default;
        deserializer.Deserialize(new Memory<Byte>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer<T> deserializer, in ReadOnlySequence<Byte> sequence)
    {
        T? value = default;
        deserializer.Deserialize(in sequence, ref value);
        return value;
    }

    #endregion IDeserializer<T>

    #region IDeserializer

    #region Generic

    public static T? Deserialize<T>(this IDeserializer deserializer, Stream stream, CancellationToken cancellationToken = default)
    {
        T? value = default;
        deserializer.Deserialize(stream, ref value, cancellationToken);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, ReadOnlySpan<Byte> span)
    {
        T? value = default;
        deserializer.Deserialize(span, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, ReadOnlyMemory<Byte> memory)
    {
        T? value = default;
        deserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, Byte[] array)
    {
        T? value = default;
        deserializer.Deserialize(new Memory<Byte>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this IDeserializer deserializer, in ReadOnlySequence<Byte> sequence)
    {
        T? value = default;
        deserializer.Deserialize(in sequence, ref value);
        return value;
    }

    #endregion Generic

    #region NonGeneric

    public static Object? Deserialize(this IDeserializer deserializer, Type type, Stream stream, CancellationToken cancellationToken = default)
    {
        Object? value = default;
        deserializer.Deserialize(type, stream, ref value, cancellationToken);
        return value;
    }

    public static Object? Deserialize(this IDeserializer deserializer, Type type, ReadOnlySpan<Byte> span)
    {
        Object? value = default;
        deserializer.Deserialize(type, span, ref value);
        return value;
    }

    public static Object? Deserialize(this IDeserializer deserializer, Type type, ReadOnlyMemory<Byte> memory)
    {
        Object? value = default;
        deserializer.Deserialize(type, memory, ref value);
        return value;
    }

    public static Object? Deserialize(this IDeserializer deserializer, Type type, Byte[] array)
    {
        Object? value = default;
        deserializer.Deserialize(type, new Memory<Byte>(array), ref value);
        return value;
    }

    public static Object? Deserialize(this IDeserializer deserializer, Type type, in ReadOnlySequence<Byte> sequence)
    {
        Object? value = default;
        deserializer.Deserialize(type, in sequence, ref value);
        return value;
    }

    #endregion NonGeneric

    #endregion IDeserializer
}