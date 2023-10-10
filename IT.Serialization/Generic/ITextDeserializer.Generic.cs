using System;
using System.Buffers;

namespace IT.Serialization.Generic;

public interface ITextDeserializer<T> : IDeserializer<T>
{
    int Deserialize(ReadOnlySpan<char> span, ref T? value);

    int Deserialize(ReadOnlyMemory<char> memory, ref T? value);

    int Deserialize(in ReadOnlySequence<char> sequence, ref T? value);
}