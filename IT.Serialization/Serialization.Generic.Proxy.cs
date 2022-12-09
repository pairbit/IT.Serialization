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

    public Task SerializeAsync(Stream stream, T value, CancellationToken cancellationToken = default)
        => _serialization.SerializeAsync(stream, value, cancellationToken);

    public ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
        => _serialization.DeserializeAsync<T>(stream, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    public void Serialize(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default)
        => _serialization.Serialize(writer, value, cancellationToken);

    public void Serialize(Stream stream, T value, CancellationToken cancellationToken = default)
        => _serialization.Serialize(stream, value, cancellationToken);

    public Byte[] Serialize(T value, CancellationToken cancellationToken = default)
         => _serialization.Serialize(value, cancellationToken);

    public T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default)
        => _serialization.Deserialize<T>(memory, cancellationToken);

    public T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default)
        => _serialization.Deserialize<T>(sequence, cancellationToken);

    public T? Deserialize(Stream stream, CancellationToken cancellationToken = default)
        => _serialization.Deserialize<T>(stream, cancellationToken);

    #endregion ISerializer
}