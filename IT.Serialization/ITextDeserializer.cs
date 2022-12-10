using System;
using System.Buffers;

namespace IT.Serialization;

public interface ITextDeserializer : IDeserializer
{
    #region Generic

    Int32 Deserialize<T>(ReadOnlySpan<Char> span, ref T? value);

    Int32 Deserialize<T>(ReadOnlyMemory<Char> memory, ref T? value);

    Int32 Deserialize<T>(in ReadOnlySequence<Char> sequence, ref T? value);

    #endregion Generic

    #region NonGeneric

    Int32 Deserialize(Type type, ReadOnlySpan<Char> span, ref Object? value);

    Int32 Deserialize(Type type, ReadOnlyMemory<Char> memory, ref Object? value);

    Int32 Deserialize(Type type, in ReadOnlySequence<Char> sequence, ref Object? value);

    #endregion NonGeneric
}