using System;
using System.Buffers;

namespace IT.Serialization;

public static class xITextDeserializer
{
    #region Generic

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, ReadOnlySpan<char> span)
    {
        T? value = default;
        textDeserializer.Deserialize(span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, string str)
    {
        T? value = default;
        textDeserializer.Deserialize(str.AsSpan(), ref value);
        return value;
    }

#endif

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, ReadOnlyMemory<char> memory)
    {
        T? value = default;
        textDeserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, char[] array)
    {
        T? value = default;
        textDeserializer.Deserialize(new ReadOnlyMemory<char>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, in ReadOnlySequence<char> sequence)
    {
        T? value = default;
        textDeserializer.Deserialize(sequence, ref value);
        return value;
    }

    #endregion Generic

    #region NonGeneric

    public static object? Deserialize(this ITextDeserializer textDeserializer, Type type, ReadOnlySpan<char> span)
    {
        object? value = default;
        textDeserializer.Deserialize(type, span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static object? Deserialize(this ITextDeserializer textDeserializer, Type type, string str)
    {
        object? value = default;
        textDeserializer.Deserialize(type, str.AsSpan(), ref value);
        return value;
    }

#endif

    public static object? Deserialize(this ITextDeserializer textDeserializer, Type type, ReadOnlyMemory<char> memory)
    {
        object? value = default;
        textDeserializer.Deserialize(type, memory, ref value);
        return value;
    }

    public static object? Deserialize(this ITextDeserializer textDeserializer, Type type, char[] array)
    {
        object? value = default;
        textDeserializer.Deserialize(type, new ReadOnlyMemory<char>(array), ref value);
        return value;
    }

    public static object? Deserialize(this ITextDeserializer textDeserializer, Type type, in ReadOnlySequence<char> sequence)
    {
        object? value = default;
        textDeserializer.Deserialize(type, sequence, ref value);
        return value;
    }

    #endregion NonGeneric
}