using System;
using System.Buffers;

namespace IT.Serialization;

public static class xITextDeserializer
{
    #region ITextDeserializer<T>

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, ReadOnlySpan<Char> span)
    {
        T? value = default;
        textDeserializer.Deserialize(span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, String str)
    {
        T? value = default;
        textDeserializer.Deserialize(str.AsSpan(), ref value);
        return value;
    }

#endif

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, ReadOnlyMemory<Char> memory)
    {
        T? value = default;
        textDeserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, Char[] array)
    {
        T? value = default;
        textDeserializer.Deserialize(new Memory<Char>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, in ReadOnlySequence<Char> sequence)
    {
        T? value = default;
        textDeserializer.Deserialize(sequence, ref value);
        return value;
    }

    #endregion ITextDeserializer<T>

    #region ITextDeserializer

    #region Generic

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, ReadOnlySpan<Char> span)
    {
        T? value = default;
        textDeserializer.Deserialize(span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, String str)
    {
        T? value = default;
        textDeserializer.Deserialize(str.AsSpan(), ref value);
        return value;
    }

#endif

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, ReadOnlyMemory<Char> memory)
    {
        T? value = default;
        textDeserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, Char[] array)
    {
        T? value = default;
        textDeserializer.Deserialize(new Memory<Char>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer textDeserializer, in ReadOnlySequence<Char> sequence)
    {
        T? value = default;
        textDeserializer.Deserialize(sequence, ref value);
        return value;
    }

    #endregion Generic

    #region NonGeneric

    public static Object? Deserialize(this ITextDeserializer textDeserializer, Type type, ReadOnlySpan<Char> span)
    {
        Object? value = default;
        textDeserializer.Deserialize(type, span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static Object? Deserialize(this ITextDeserializer textDeserializer, Type type, String str)
    {
        Object? value = default;
        textDeserializer.Deserialize(type, str.AsSpan(), ref value);
        return value;
    }

#endif

    public static Object? Deserialize(this ITextDeserializer textDeserializer, Type type, ReadOnlyMemory<Char> memory)
    {
        Object? value = default;
        textDeserializer.Deserialize(type, memory, ref value);
        return value;
    }

    public static Object? Deserialize(this ITextDeserializer textDeserializer, Type type, Char[] array)
    {
        Object? value = default;
        textDeserializer.Deserialize(type, new Memory<Char>(array), ref value);
        return value;
    }

    public static Object? Deserialize(this ITextDeserializer textDeserializer, Type type, in ReadOnlySequence<Char> sequence)
    {
        Object? value = default;
        textDeserializer.Deserialize(type, sequence, ref value);
        return value;
    }

    #endregion NonGeneric

    #endregion ITextDeserializer
}