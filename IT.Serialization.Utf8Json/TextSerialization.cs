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

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeAsync(stream, value, _resolver);

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.DeserializeAsync<T>(stream, _resolver));

    public Task SerializeAsync(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.SerializeAsync(type, stream, value, _resolver);

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.NonGeneric.DeserializeAsync(type, stream, _resolver));

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public void Serialize<T>(Stream stream, T value, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, _resolver);

    public Byte[] Serialize<T>(T value, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(value, _resolver);

    public T? Deserialize<T>(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;
            var offset = segment.Offset;

            if ((array.Length - offset) == segment.Count)
                return JsonSerializer.Deserialize<T>(array, offset, _resolver);
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            return JsonSerializer.Deserialize<T>(rented, _resolver);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public T? Deserialize<T>(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public T? Deserialize<T>(Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize<T>(stream, _resolver);

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, IBufferWriter<Byte> writer, Object value, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public void Serialize(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.Serialize(type, stream, value, _resolver);

    public Byte[] Serialize(Type type, Object value, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.Serialize(type, value, _resolver);

    public Object? Deserialize(Type type, ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
    {
        if (MemoryMarshal.TryGetArray(memory, out ArraySegment<byte> segment))
        {
            var array = segment.Array;
            var offset = segment.Offset;

            if ((array.Length - offset) == segment.Count)
                return JsonSerializer.NonGeneric.Deserialize(type, array, offset, _resolver);
        }
        var span = memory.Span;
        var len = span.Length;
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            span.CopyTo(rented);
            return JsonSerializer.NonGeneric.Deserialize(type, rented, _resolver);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public Object? Deserialize(Type type, ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Object? Deserialize(Type type, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.Deserialize(type, stream, _resolver);

    #endregion NonGeneric

    #endregion ISerializer

    #region ITextSerializer

    #region Generic

    //public void Serialize<T>(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public String SerializeToText<T>(T value, CancellationToken cancellationToken)
        => JsonSerializer.ToJsonString(value, _resolver);

    public T? Deserialize<T>(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
    {
        var span = memory.Span;

        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);

        try
        {
            _encoding.GetBytes(span, rented);

            return JsonSerializer.Deserialize<T>(rented, _resolver);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    //public T? Deserialize<T>(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion Generic

    #region NonGeneric

    //public void Serialize(Type type, IBufferWriter<Char> writer, Object value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public String SerializeToText(Type type, Object value, CancellationToken cancellationToken)
        => JsonSerializer.NonGeneric.ToJsonString(type, value, _resolver);

    public Object? Deserialize(Type type, ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
    {
        var span = memory.Span;

        var len = _encoding.GetByteCount(span);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);

        try
        {
            _encoding.GetBytes(span, rented);

            return JsonSerializer.NonGeneric.Deserialize(type, rented, _resolver);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    //public Object? Deserialize(Type type, ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion NonGeneric

    #endregion ITextSerializer
}