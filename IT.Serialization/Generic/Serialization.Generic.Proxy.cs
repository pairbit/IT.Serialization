using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.Generic;

public class SerializationProxy<T> : ISerialization<T>
{
    private readonly ISerialization _serialization;

    public SerializationProxy(ISerialization serialization)
    {
        _serialization = serialization;
    }

    public ValueTask SerializeAsync(in T? value, Stream stream, CancellationToken cancellationToken = default)
        => _serialization.SerializeAsync(value, stream, cancellationToken);

    public void Serialize(in T? value, Stream stream, CancellationToken cancellationToken = default)
        => _serialization.Serialize(value, stream, cancellationToken);

    public void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => _serialization.Serialize(in value, in writer);

    public byte[] Serialize(in T? value)
         => _serialization.Serialize(in value);

    public ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
        => _serialization.DeserializeAsync<T>(stream, cancellationToken);

    public int Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default)
        => _serialization.Deserialize(stream, ref value, cancellationToken);

    public int Deserialize(ReadOnlySpan<byte> span, ref T? value)
        => _serialization.Deserialize(span, ref value);

    public int Deserialize(ReadOnlyMemory<byte> memory, ref T? value)
        => _serialization.Deserialize(memory, ref value);

    public int Deserialize(in ReadOnlySequence<byte> sequence, ref T? value)
        => _serialization.Deserialize(in sequence, ref value);
}