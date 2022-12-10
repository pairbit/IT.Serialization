using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utf8Json;

namespace IT.Serialization.Utf8Json;

public class TextSerialization : ITextSerialization
{
    private readonly IJsonFormatterResolver? _resolver;
    private static readonly Encoding _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    public TextSerialization(IJsonFormatterResolver? resolver = null)
    {
        _resolver = resolver;
    }

    #region IAsyncSerializer

    public ValueTask SerializeAsync<T>(T? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, _resolver));

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await JsonSerializer.DeserializeAsync<T>(stream, _resolver).ConfigureAwait(false);

    public ValueTask SerializeAsync(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.NonGeneric.SerializeAsync(type, stream, value, _resolver));

    public async ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => await JsonSerializer.NonGeneric.DeserializeAsync(type, stream, _resolver).ConfigureAwait(false);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, _resolver);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => throw new NotImplementedException();

    public Byte[] Serialize<T>(in T? value)
        => JsonSerializer.Serialize(value, _resolver);

    public Int32 Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize<T>(stream, _resolver);
        return (Int32)stream.Length;
    }

    public Int32 Deserialize<T>(ReadOnlySpan<Byte> span, ref T? value)
    {
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            value = JsonSerializer.Deserialize<T>(rented, _resolver);
            return len;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize<T>(ReadOnlyMemory<Byte> memory, ref T? value)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;

            if (array == null) throw new InvalidOperationException();

            var offset = segment.Offset;

            var consumed = segment.Count;

            if ((array.Length - offset) == consumed)
            {
                value = JsonSerializer.Deserialize<T>(array, offset, _resolver);
                return consumed;
            }
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            value = JsonSerializer.Deserialize<T>(rented, _resolver);
            return len;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize<T>(in ReadOnlySequence<Byte> sequence, ref T? value)
        => throw new NotImplementedException();

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.Serialize(type, stream, value, _resolver);

    public void Serialize<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => throw new NotImplementedException();

    public Byte[] Serialize(Type type, Object? value)
        => JsonSerializer.NonGeneric.Serialize(type, value, _resolver);

    public Int32 Deserialize(Type type, Stream stream, ref Object? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.NonGeneric.Deserialize(type, stream, _resolver);
        return (Int32)stream.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlySpan<Byte> span, ref Object? value)
    {
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            value = JsonSerializer.NonGeneric.Deserialize(type, rented, _resolver);
            return len;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize(Type type, ReadOnlyMemory<Byte> memory, ref Object? value)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;

            if (array == null) throw new InvalidOperationException();

            var offset = segment.Offset;
            var consumed = segment.Count;

            if ((array.Length - offset) == consumed)
            {
                value = JsonSerializer.NonGeneric.Deserialize(type, array, offset, _resolver);
                return consumed;
            }
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            value = JsonSerializer.NonGeneric.Deserialize(type, rented, _resolver);
            return len;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize(Type type, in ReadOnlySequence<Byte> sequence, ref Object? value)
        => throw new NotImplementedException();

    #endregion NonGeneric

    #endregion ISerializer

    #region ITextSerializer

    #region Generic

    public void SerializeToText<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(JsonSerializer.ToJsonString(value, _resolver).AsSpan());

    public String SerializeToText<T>(in T? value)
        => JsonSerializer.ToJsonString(value, _resolver);

    public Int32 Deserialize<T>(ReadOnlySpan<Char> span, ref T? value)
    {
        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);

        try
        {
            _encoding.GetBytes(span, rented);

            value = JsonSerializer.Deserialize<T>(rented, _resolver);

            return len;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize<T>(ReadOnlyMemory<Char> memory, ref T? value)
        => Deserialize(memory.Span, ref value);

    public Int32 Deserialize<T>(in ReadOnlySequence<Char> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<Char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion Generic

    #region NonGeneric

    public void SerializeToText<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(JsonSerializer.NonGeneric.ToJsonString(type, value, _resolver).AsSpan());

    public String SerializeToText(Type type, Object? value)
        => JsonSerializer.NonGeneric.ToJsonString(type, value, _resolver);

    public Int32 Deserialize(Type type, ReadOnlySpan<Char> span, ref Object? value)
    {
        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);

        try
        {
            _encoding.GetBytes(span, rented);

            value = JsonSerializer.NonGeneric.Deserialize(type, rented, _resolver);

            return span.Length;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Int32 Deserialize(Type type, ReadOnlyMemory<Char> memory, ref Object? value)
        => Deserialize(type, memory.Span, ref value);

    public Int32 Deserialize(Type type, in ReadOnlySequence<Char> sequence, ref Object? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<Char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(type, span, ref value);
    }

    #endregion NonGeneric

    #endregion ITextSerializer
}