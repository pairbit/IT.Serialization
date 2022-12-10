using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface IDeserializer : IAsyncDeserializer
{
    #region Generic

    Int32 Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken = default);

    Int32 Deserialize<T>(ReadOnlySpan<Byte> span, ref T? value);

    Int32 Deserialize<T>(ReadOnlyMemory<Byte> memory, ref T? value);

    Int32 Deserialize<T>(in ReadOnlySequence<Byte> sequence, ref T? value);

    #endregion Generic

    #region NonGeneric

    Int32 Deserialize(Type type, Stream stream, ref Object? value, CancellationToken cancellationToken = default);

    Int32 Deserialize(Type type, ReadOnlySpan<Byte> span, ref Object? value);

    Int32 Deserialize(Type type, ReadOnlyMemory<Byte> memory, ref Object? value);

    Int32 Deserialize(Type type, in ReadOnlySequence<Byte> sequence, ref Object? value);

    #endregion NonGeneric
}