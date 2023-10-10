using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.Generic;

public interface IDeserializer<T>
{
    ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default);

    int Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default);

    int Deserialize(ReadOnlySpan<byte> span, ref T? value);

    int Deserialize(ReadOnlyMemory<byte> memory, ref T? value);

    int Deserialize(in ReadOnlySequence<byte> sequence, ref T? value);
}