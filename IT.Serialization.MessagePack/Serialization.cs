using MessagePack;
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.MessagePack;

public class Serialization : ISerialization
{
    private readonly MessagePackSerializerOptions? _options;

    public Serialization(MessagePackSerializerOptions? options = null)
    {
        _options = options;
    }

    #region IAsyncSerializer

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken)
        => MessagePackSerializer.SerializeAsync(stream, value, _options, cancellationToken);

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);

    public Task SerializeAsync(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => MessagePackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken);

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(writer, value, _options, cancellationToken);

    public void Serialize<T>(Stream stream, T value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(stream, value, _options, cancellationToken);

    public Byte[] Serialize<T>(T value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(value, _options, cancellationToken);

    public T? Deserialize<T>(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize<T>(memory, _options, cancellationToken);

    public T? Deserialize<T>(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize<T>(sequence, _options, cancellationToken);

    public T? Deserialize<T>(Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize<T>(stream, _options, cancellationToken);

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, IBufferWriter<Byte> writer, Object value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(type, writer, value, _options, cancellationToken);

    public void Serialize(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(type, stream, value, _options, cancellationToken);

    public Byte[] Serialize(Type type, Object value, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(type, value, _options, cancellationToken);

    public Object? Deserialize(Type type, ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize(type, memory, _options, cancellationToken);

    public Object? Deserialize(Type type, ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize(type, sequence, _options, cancellationToken);

    public Object? Deserialize(Type type, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Deserialize(type, stream, _options, cancellationToken);

    #endregion NonGeneric

    #endregion ISerializer
}