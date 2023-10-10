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

    #region ISerializer

    #region Generic

    public ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, _getOptions?.Invoke(), cancellationToken));

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

    public byte[] Serialize<T>(in T? value)
        => JsonSerializer.SerializeToUtf8Bytes(value, _getOptions?.Invoke());

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync<T>(stream, _getOptions?.Invoke(), cancellationToken);

    public int Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize<T>(stream, _getOptions?.Invoke());
        return (int)stream.Length;
    }

    public int Deserialize<T>(ReadOnlySpan<byte> span, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(span, _getOptions?.Invoke());
        return span.Length;
    }

    public int Deserialize<T>(ReadOnlyMemory<byte> memory, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());
        return memory.Length;
    }

    public int Deserialize<T>(in ReadOnlySequence<byte> sequence, ref T? value)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        value = JsonSerializer.Deserialize<T>(ref jsonReader, _getOptions?.Invoke());
        return (int)jsonReader.BytesConsumed;
    }

    #endregion Generic

    #region NonGeneric

    public ValueTask SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => new(JsonSerializer.SerializeAsync(stream, value, type, _getOptions?.Invoke(), cancellationToken));

    public void Serialize(Type type, object? value, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, type, _getOptions?.Invoke());

    public void Serialize<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
    {
        using var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, value, type, _getOptions?.Invoke());
    }

    public byte[] Serialize(Type type, object? value)
        => JsonSerializer.SerializeToUtf8Bytes(value, type, _getOptions?.Invoke());

    public ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync(stream, type, _getOptions?.Invoke(), cancellationToken);

    public int Deserialize(Type type, Stream stream, ref object? value, CancellationToken cancellationToken)
    {
        value = JsonSerializer.Deserialize(stream, type, _getOptions?.Invoke());

        return (int)stream.Length;
    }

    public int Deserialize(Type type, ReadOnlySpan<byte> span, ref object? value)
    {
        value = JsonSerializer.Deserialize(span, type, _getOptions?.Invoke());
        return span.Length;
    }

    public int Deserialize(Type type, ReadOnlyMemory<byte> memory, ref object? value)
    {
        value = JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());
        return memory.Length;
    }

    public int Deserialize(Type type, in ReadOnlySequence<byte> sequence, ref object? value)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        value = JsonSerializer.Deserialize(ref jsonReader, type, _getOptions?.Invoke());
        return (int)jsonReader.BytesConsumed;
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

    public string SerializeToText<T>(in T? value)
        => JsonSerializer.Serialize(value, _getOptions?.Invoke());

    public int Deserialize<T>(ReadOnlySpan<char> span, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(span, _getOptions?.Invoke());
        return span.Length;
    }

    public int Deserialize<T>(ReadOnlyMemory<char> memory, ref T? value)
    {
        value = JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());
        return memory.Length;
    }

    public virtual int Deserialize<T>(in ReadOnlySequence<char> sequence, ref T? value)
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

    public virtual void SerializeToText<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
       => writer.Write(SerializeToText(type, value).AsSpan());

    public string SerializeToText(Type type, object? value)
        => JsonSerializer.Serialize(value, type, _getOptions?.Invoke());

    public int Deserialize(Type type, ReadOnlySpan<char> span, ref object? value)
    {
        value = JsonSerializer.Deserialize(span, type, _getOptions?.Invoke());
        return span.Length;
    }

    public int Deserialize(Type type, ReadOnlyMemory<char> memory, ref object? value)
    {
        value = JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());
        return memory.Length;
    }

    public virtual int Deserialize(Type type, in ReadOnlySequence<char> sequence, ref object? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(type, sequence.First.Span, ref value);
#else
            return Deserialize(type, sequence.FirstSpan, ref value);
#endif
        }

        Span<char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion NonGeneric

    #endregion ITextSerializer
}