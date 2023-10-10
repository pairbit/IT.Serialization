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

    #region Generic

    public ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => new(MessagePackSerializer.SerializeAsync(stream, value, _options, cancellationToken));

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(stream, value, _options, cancellationToken);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MessagePackSerializer.Serialize(writer, value, _options);

    public byte[] Serialize<T>(in T? value)
        => MessagePackSerializer.Serialize(value, _options);

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);

    public int Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = MessagePackSerializer.Deserialize<T>(stream, _options, cancellationToken);

        return (int)stream.Length;
    }

    public int Deserialize<T>(ReadOnlySpan<byte> span, ref T? value)
    {
        var array = new byte[span.Length];

        span.CopyTo(array);

        var reader = new MessagePackReader(array);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (int)reader.Consumed;
    }

    public int Deserialize<T>(ReadOnlyMemory<byte> memory, ref T? value)
    {
        var reader = new MessagePackReader(memory);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (int)reader.Consumed;
    }

    public int Deserialize<T>(in ReadOnlySequence<byte> sequence, ref T? value)
    {
        var reader = new MessagePackReader(in sequence);

        value = MessagePackSerializer.Deserialize<T>(ref reader, _options);

        return (int)reader.Consumed;
    }

    #endregion Generic

    #region NonGeneric

    public ValueTask SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => new(MessagePackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken));

    public void Serialize(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.Serialize(type, stream, value, _options, cancellationToken);

    public void Serialize<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MessagePackSerializer.Serialize(type, writer, value, _options);

    public byte[] Serialize(Type type, object? value)
        => MessagePackSerializer.Serialize(type, value, _options);

    public ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => MessagePackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);

    public int Deserialize(Type type, Stream stream, ref object? value, CancellationToken cancellationToken)
    {
        value = MessagePackSerializer.Deserialize(type, stream, _options, cancellationToken);

        return (int)stream.Length;
    }

    public int Deserialize(Type type, ReadOnlySpan<byte> span, ref object? value)
    {
        var array = new byte[span.Length];

        span.CopyTo(array);

        var reader = new MessagePackReader(array);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (int)reader.Consumed;
    }

    public int Deserialize(Type type, ReadOnlyMemory<byte> memory, ref object? value)
    {
        var reader = new MessagePackReader(memory);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (int)reader.Consumed;
    }

    public int Deserialize(Type type, in ReadOnlySequence<byte> sequence, ref object? value)
    {
        var reader = new MessagePackReader(in sequence);

        value = MessagePackSerializer.Deserialize(type, ref reader, _options);

        return (int)reader.Consumed;
    }

    #endregion NonGeneric
}