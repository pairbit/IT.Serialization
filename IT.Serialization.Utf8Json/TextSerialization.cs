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

    #region ISerializer

    #region Generic

    public ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, _resolver));

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, _resolver);

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => throw new NotImplementedException();

    public byte[] Serialize<T>(in T? value)
        => JsonSerializer.Serialize(value, _resolver);

    public async ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => await JsonSerializer.DeserializeAsync<T>(stream, _resolver).ConfigureAwait(false);

    public int Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize<T>(stream, _resolver);
        return (int)stream.Length;
    }

    public int Deserialize<T>(ReadOnlySpan<byte> span, ref T? value)
    {
        var len = span.Length;
        var pool = ArrayPool<byte>.Shared;
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

    public int Deserialize<T>(ReadOnlyMemory<byte> memory, ref T? value)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;

            if (array == null) throw new InvalidOperationException();

            var offset = segment.Offset;

            var consumed = segment.Count;

            if (array.Length - offset == consumed)
            {
                value = JsonSerializer.Deserialize<T>(array, offset, _resolver);
                return consumed;
            }
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<byte>.Shared;
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

    public int Deserialize<T>(in ReadOnlySequence<byte> sequence, ref T? value)
        => throw new NotImplementedException();

    #endregion Generic

    #region NonGeneric

    public ValueTask SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.NonGeneric.SerializeAsync(type, stream, value, _resolver));

    public void Serialize(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.Serialize(type, stream, value, _resolver);

    public void Serialize<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => throw new NotImplementedException();

    public byte[] Serialize(Type type, object? value)
        => JsonSerializer.NonGeneric.Serialize(type, value, _resolver);

    public async ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => await JsonSerializer.NonGeneric.DeserializeAsync(type, stream, _resolver).ConfigureAwait(false);

    public int Deserialize(Type type, Stream stream, ref object? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.NonGeneric.Deserialize(type, stream, _resolver);
        return (int)stream.Length;
    }

    public int Deserialize(Type type, ReadOnlySpan<byte> span, ref object? value)
    {
        var len = span.Length;
        var pool = ArrayPool<byte>.Shared;
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

    public int Deserialize(Type type, ReadOnlyMemory<byte> memory, ref object? value)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;

            if (array == null) throw new InvalidOperationException();

            var offset = segment.Offset;
            var consumed = segment.Count;

            if (array.Length - offset == consumed)
            {
                value = JsonSerializer.NonGeneric.Deserialize(type, array, offset, _resolver);
                return consumed;
            }
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<byte>.Shared;
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

    public int Deserialize(Type type, in ReadOnlySequence<byte> sequence, ref object? value)
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

    public string SerializeToText<T>(in T? value)
        => JsonSerializer.ToJsonString(value, _resolver);

    public int Deserialize<T>(ReadOnlySpan<char> span, ref T? value)
    {
        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<byte>.Shared;

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

    public int Deserialize<T>(ReadOnlyMemory<char> memory, ref T? value)
        => Deserialize(memory.Span, ref value);

    public int Deserialize<T>(in ReadOnlySequence<char> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion Generic

    #region NonGeneric

    public void SerializeToText<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(JsonSerializer.NonGeneric.ToJsonString(type, value, _resolver).AsSpan());

    public string SerializeToText(Type type, object? value)
        => JsonSerializer.NonGeneric.ToJsonString(type, value, _resolver);

    public int Deserialize(Type type, ReadOnlySpan<char> span, ref object? value)
    {
        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<byte>.Shared;

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

    public int Deserialize(Type type, ReadOnlyMemory<char> memory, ref object? value)
        => Deserialize(type, memory.Span, ref value);

    public int Deserialize(Type type, in ReadOnlySequence<char> sequence, ref object? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(type, span, ref value);
    }

    #endregion NonGeneric

    #endregion ITextSerializer
}