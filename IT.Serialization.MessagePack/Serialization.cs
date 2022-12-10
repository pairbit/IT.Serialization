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

    public ValueTask SerializeAsync<T>(T? value, Stream stream, CancellationToken cancellationToken)
        => new(MessagePackSerializer.SerializeAsync(stream, value, _options, cancellationToken));

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);

    public ValueTask SerializeAsync(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => new(MessagePackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken));

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(stream, value, _options, cancellationToken);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MessagePackSerializer.Serialize(writer, value, _options);

    public Byte[] Serialize<T>(in T? value)
        => MessagePackSerializer.Serialize(value, _options);

    public Int32 Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = MessagePackSerializer.Deserialize<T>(stream, _options, cancellationToken);

        return (Int32)stream.Length;
    }

    public Int32 Deserialize<T>(ReadOnlySpan<Byte> span, ref T? value)
    {
        var array = new byte[span.Length];

        span.CopyTo(array);

        var reader = new MessagePackReader(array);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (Int32)reader.Consumed;
    }

    public Int32 Deserialize<T>(ReadOnlyMemory<Byte> memory, ref T? value)
    {
        var reader = new MessagePackReader(memory);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (Int32)reader.Consumed;
    }

    public Int32 Deserialize<T>(in ReadOnlySequence<Byte> sequence, ref T? value)
    {
        var reader = new MessagePackReader(in sequence);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (Int32)reader.Consumed;
    }

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(type, stream, value, _options, cancellationToken);

    public void Serialize<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MessagePackSerializer.Serialize(type, writer, value, _options);

    public Byte[] Serialize(Type type, Object? value)
        => MessagePackSerializer.Serialize(type, value, _options);

    public Int32 Deserialize(Type type, Stream stream, ref Object? value, CancellationToken cancellationToken)
    {
        value = MessagePackSerializer.Deserialize(type, stream, _options, cancellationToken);

        return (Int32)stream.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlySpan<Byte> span, ref Object? value)
    {
        var array = new byte[span.Length];

        span.CopyTo(array);

        var reader = new MessagePackReader(array);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (Int32)reader.Consumed;
    }

    public Int32 Deserialize(Type type, ReadOnlyMemory<Byte> memory, ref Object? value)
    {
        var reader = new MessagePackReader(memory);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (Int32)reader.Consumed;
    }

    public Int32 Deserialize(Type type, in ReadOnlySequence<Byte> sequence, ref Object? value)
    {
        var reader = new MessagePackReader(in sequence);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (Int32)reader.Consumed;
    }

    #endregion NonGeneric

    #endregion ISerializer
}