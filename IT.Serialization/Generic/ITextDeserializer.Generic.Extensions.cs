using System;
using System.Buffers;

namespace IT.Serialization.Generic;

public static class xITextDeserializerGeneric
{
    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, ReadOnlySpan<char> span)
    {
        T? value = default;
        textDeserializer.Deserialize(span, ref value);
        return value;
    }

#if NETSTANDARD2_0

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, string str)
    {
        T? value = default;
        textDeserializer.Deserialize(str.AsSpan(), ref value);
        return value;
    }

#endif

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, ReadOnlyMemory<char> memory)
    {
        T? value = default;
        textDeserializer.Deserialize(memory, ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, char[] array)
    {
        T? value = default;
        textDeserializer.Deserialize(new ReadOnlyMemory<char>(array), ref value);
        return value;
    }

    public static T? Deserialize<T>(this ITextDeserializer<T> textDeserializer, in ReadOnlySequence<char> sequence)
    {
        T? value = default;
        textDeserializer.Deserialize(sequence, ref value);
        return value;
    }
}