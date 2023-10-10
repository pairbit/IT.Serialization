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

    #region IAsyncSerializer

    public ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(stream, value, _options, cancellationToken);

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await MemoryPackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);

    public ValueTask SerializeAsync(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken);

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.DeserializeAsync(type, stream, _options, cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(stream, value, _options, cancellationToken).AsTask().Wait(cancellationToken);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MemoryPackSerializer.Serialize(in writer, in value, _options);

    public Byte[] Serialize<T>(in T? value)
        => MemoryPackSerializer.Serialize(in value, _options);

    public Int32 Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = MemoryPackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).AsTask().Result;

        return (Int32)stream.Length;
    }

    public Int32 Deserialize<T>(ReadOnlySpan<Byte> bytes, ref T? value)
        => MemoryPackSerializer.Deserialize(bytes, ref value, _options);

    public Int32 Deserialize<T>(ReadOnlyMemory<Byte> memory, ref T? value)
        => MemoryPackSerializer.Deserialize(memory.Span, ref value, _options);

    public Int32 Deserialize<T>(in ReadOnlySequence<Byte> sequence, ref T? value)
        => MemoryPackSerializer.Deserialize(in sequence, ref value, _options);

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => MemoryPackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken).AsTask().Wait(cancellationToken);

    public void Serialize<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => MemoryPackSerializer.Serialize(type, in writer, value, _options);

    public Byte[] Serialize(Type type, Object? value)
        => MemoryPackSerializer.Serialize(type, value, _options);

    public Int32 Deserialize(Type type, Stream stream, ref Object? value, CancellationToken cancellationToken)
    {
        value = MemoryPackSerializer.DeserializeAsync(type, stream, _options, cancellationToken).AsTask().Result;

        return (Int32)stream.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlySpan<Byte> span, ref Object? value)
        => MemoryPackSerializer.Deserialize(type, span, ref value, _options);

    public Int32 Deserialize(Type type, ReadOnlyMemory<Byte> memory, ref Object? value)
        => MemoryPackSerializer.Deserialize(type, memory.Span, ref value, _options);

    public Int32 Deserialize(Type type, in ReadOnlySequence<Byte> sequence, ref Object? value)
        => MemoryPackSerializer.Deserialize(type, in sequence, ref value, _options);

    #endregion NonGeneric

    #endregion ISerializer
}