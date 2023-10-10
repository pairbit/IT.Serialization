using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public class SerializationProxy<T> : ISerialization<T>
{
    private readonly ISerialization _serialization;

    public SerializationProxy(ISerialization serialization)
    {
        _serialization = serialization;
    }

    #region IAsyncSerializer

    public ValueTask SerializeAsync(in T? value, Stream stream, CancellationToken cancellationToken = default)
        => _serialization.SerializeAsync(value, stream, cancellationToken);

    public ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
        => _serialization.DeserializeAsync<T>(stream, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    public void Serialize(in T? value, Stream stream, CancellationToken cancellationToken = default)
        => _serialization.Serialize(value, stream, cancellationToken);

    public void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => _serialization.Serialize(in value, in writer);

    public Byte[] Serialize(in T? value)
         => _serialization.Serialize(in value);

    public Int32 Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default)
        => _serialization.Deserialize(stream, ref value, cancellationToken);

    public Int32 Deserialize(ReadOnlySpan<Byte> span, ref T? value)
        => _serialization.Deserialize(span, ref value);

    public Int32 Deserialize(ReadOnlyMemory<Byte> memory, ref T? value)
        => _serialization.Deserialize(memory, ref value);

    public Int32 Deserialize(in ReadOnlySequence<Byte> sequence, ref T? value)
        => _serialization.Deserialize(in sequence, ref value);

    #endregion ISerializer
}