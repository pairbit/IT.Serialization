using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface IDeserializer<T> : IAsyncDeserializer<T>
{
    T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default);

    T? Deserialize(Stream stream, CancellationToken cancellationToken = default);
}