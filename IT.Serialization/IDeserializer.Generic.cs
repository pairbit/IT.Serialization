using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface IDeserializer<T> : IAsyncDeserializer<T>
{
    Int32 Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default);

    Int32 Deserialize(ReadOnlySpan<Byte> span, ref T? value);

    Int32 Deserialize(ReadOnlyMemory<Byte> memory, ref T? value);

    Int32 Deserialize(in ReadOnlySequence<Byte> sequence, ref T? value);
}