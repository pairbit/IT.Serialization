using System;
using System.Buffers;

namespace IT.Serialization;

public interface ITextDeserializer : IDeserializer
{
    #region Generic

    int Deserialize<T>(ReadOnlySpan<char> span, ref T? value);

    int Deserialize<T>(ReadOnlyMemory<char> memory, ref T? value);

    int Deserialize<T>(in ReadOnlySequence<char> sequence, ref T? value);

    #endregion Generic

    #region NonGeneric

    int Deserialize(Type type, ReadOnlySpan<char> span, ref object? value);

    int Deserialize(Type type, ReadOnlyMemory<char> memory, ref object? value);

    int Deserialize(Type type, in ReadOnlySequence<char> sequence, ref object? value);

    #endregion NonGeneric
}