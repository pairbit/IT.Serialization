using System;
using System.Buffers;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.Json;

public class TextSerialization : ITextSerialization
{
    private readonly Func<JsonSerializerOptions>? _getOptions;

    public TextSerialization(Func<JsonSerializerOptions>? getOptions = null)
    {
        _getOptions = getOptions;
    }

    #region IAsyncSerializer

    public ValueTask SerializeAsync<T>(T? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, _getOptions?.Invoke(), cancellationToken));

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync<T>(stream, _getOptions?.Invoke(), cancellationToken);

    public ValueTask SerializeAsync(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, type, _getOptions?.Invoke(), cancellationToken));

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync(stream, type, _getOptions?.Invoke(), cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, _getOptions?.Invoke());

    public void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
    {
        using var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, value, _getOptions?.Invoke());
    }

    public Byte[] Serialize<T>(in T? value)
        => JsonSerializer.SerializeToUtf8Bytes(value, _getOptions?.Invoke());

    public Int32 Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize<T>(stream, _getOptions?.Invoke());
        return (Int32)stream.Length;
    }

    public Int32 Deserialize<T>(ReadOnlySpan<Byte> span, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(span, _getOptions?.Invoke());
        return span.Length;
    }

    public Int32 Deserialize<T>(ReadOnlyMemory<Byte> memory, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());
        return memory.Length;
    }

    public Int32 Deserialize<T>(in ReadOnlySequence<Byte> sequence, ref T? value)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        value = JsonSerializer.Deserialize<T>(ref jsonReader, _getOptions?.Invoke());
        return (Int32)jsonReader.BytesConsumed;
    }

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, Object? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, type, _getOptions?.Invoke());

    public void Serialize<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
    {
        using var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, value, type, _getOptions?.Invoke());
    }

    public Byte[] Serialize(Type type, Object? value)
        => JsonSerializer.SerializeToUtf8Bytes(value, type, _getOptions?.Invoke());

    public Int32 Deserialize(Type type, Stream stream, ref Object? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize(stream, type, _getOptions?.Invoke());

        return (Int32)stream.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlySpan<Byte> span, ref Object? value)
    {
        value = JsonSerializer.Deserialize(span, type, _getOptions?.Invoke());
        return span.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlyMemory<Byte> memory, ref Object? value)
    {
        value = JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());
        return memory.Length;
    }

    public Int32 Deserialize(Type type, in ReadOnlySequence<Byte> sequence, ref Object? value)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        value = JsonSerializer.Deserialize(ref jsonReader, type, _getOptions?.Invoke());
        return (Int32)jsonReader.BytesConsumed;
    }

    #endregion NonGeneric

    #endregion ISerializer

    #region ITextSerializer

    #region Generic

    public virtual void SerializeToText<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(SerializeToText(in value).AsSpan());

    public String SerializeToText<T>(in T? value)
        => JsonSerializer.Serialize(value, _getOptions?.Invoke());

    public Int32 Deserialize<T>(ReadOnlySpan<Char> span, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(span, _getOptions?.Invoke());
        return span.Length;
    }

    public Int32 Deserialize<T>(ReadOnlyMemory<Char> memory, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());
        return memory.Length;
    }

    public virtual Int32 Deserialize<T>(in ReadOnlySequence<Char> sequence, ref T? value)
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

    public virtual void SerializeToText<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
       => writer.Write(SerializeToText(type, value).AsSpan());

    public String SerializeToText(Type type, Object? value)
        => JsonSerializer.Serialize(value, type, _getOptions?.Invoke());

    public Int32 Deserialize(Type type, ReadOnlySpan<Char> span, ref Object? value)
    {
        value = JsonSerializer.Deserialize(span, type, _getOptions?.Invoke());
        return span.Length;
    }

    public Int32 Deserialize(Type type, ReadOnlyMemory<Char> memory, ref Object? value)
    {
        value = JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());
        return memory.Length;
    }

    public virtual Int32 Deserialize(Type type, in ReadOnlySequence<Char> sequence, ref Object? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(type, sequence.First.Span, ref value);
#else
            return Deserialize(type, sequence.FirstSpan, ref value);
#endif
        }

        Span<Char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion NonGeneric

    #endregion ITextSerializer
}