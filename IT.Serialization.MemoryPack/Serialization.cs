using MemoryPack;
using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.MemoryPack;

public class Serialization : ISerialization
{
    private readonly MemoryPackSerializerOptions? _options;

    public Serialization(MemoryPackSerializerOptions? options = null)
    {
        _options = options;
    }

    #region Generic

    public ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(stream, value, _options, cancellationToken);

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(stream, value, _options, cancellationToken).AsTask().Wait(cancellationToken);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MemoryPackSerializer.Serialize(in writer, in value, _options);

    public byte[] Serialize<T>(in T? value)
        => MemoryPackSerializer.Serialize(in value, _options);

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await MemoryPackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);

    public int Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = MemoryPackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).AsTask().Result;

        return (int)stream.Length;
    }

    public int Deserialize<T>(ReadOnlySpan<byte> bytes, ref T? value)
        => MemoryPackSerializer.Deserialize(bytes, ref value, _options);

    public int Deserialize<T>(ReadOnlyMemory<byte> memory, ref T? value)
        => MemoryPackSerializer.Deserialize(memory.Span, ref value, _options);

    public int Deserialize<T>(in ReadOnlySequence<byte> sequence, ref T? value)
        => MemoryPackSerializer.Deserialize(in sequence, ref value, _options);

    #endregion Generic

    #region NonGeneric

    public ValueTask SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken);

    public void Serialize(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken).AsTask().Wait(cancellationToken);

    public void Serialize<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MemoryPackSerializer.Serialize(type, in writer, value, _options);

    public byte[] Serialize(Type type, object? value)
        => MemoryPackSerializer.Serialize(type, value, _options);

    public ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);

    public int Deserialize(Type type, Stream stream, ref object? value, CancellationToken cancellationToken)
    {
        value = MemoryPackSerializer.DeserializeAsync(type, stream, _options, cancellationToken).AsTask().Result;

        return (int)stream.Length;
    }

    public int Deserialize(Type type, ReadOnlySpan<byte> span, ref object? value)
        => MemoryPackSerializer.Deserialize(type, span, ref value, _options);

    public int Deserialize(Type type, ReadOnlyMemory<byte> memory, ref object? value)
        => MemoryPackSerializer.Deserialize(type, memory.Span, ref value, _options);

    public int Deserialize(Type type, in ReadOnlySequence<byte> sequence, ref object? value)
        => MemoryPackSerializer.Deserialize(type, in sequence, ref value, _options);

    #endregion NonGeneric
}