using System;
using System.Buffers;

namespace IT.Serialization;

public interface ITextDeserializer<T> : IDeserializer<T>
{
    Int32 Deserialize(ReadOnlySpan<Char> span, ref T? value);

    Int32 Deserialize(ReadOnlyMemory<Char> memory, ref T? value);

    Int32 Deserialize(in ReadOnlySequence<Char> sequence, ref T? value);
}