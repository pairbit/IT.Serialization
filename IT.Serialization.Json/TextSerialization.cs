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

    public Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeAsync(stream, value, _getOptions?.Invoke(), cancellationToken);

    public ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync<T>(stream, _getOptions?.Invoke(), cancellationToken);

    public Task SerializeAsync(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeAsync(stream, value, type, _getOptions?.Invoke(), cancellationToken);

    public ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.DeserializeAsync(stream, type, _getOptions?.Invoke(), cancellationToken);

    #endregion IAsyncSerializer

    #region ISerializer

    #region Generic

    public void Serialize<T>(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken)
    {
        using var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, value, _getOptions?.Invoke());
    }

    public void Serialize<T>(Stream stream, T value, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, _getOptions?.Invoke());

    public Byte[] Serialize<T>(T value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeToUtf8Bytes(value, _getOptions?.Invoke());

    public T? Deserialize<T>(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());

    public T? Deserialize<T>(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        return JsonSerializer.Deserialize<T>(ref jsonReader, _getOptions?.Invoke());
    }

    public T? Deserialize<T>(Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize<T>(stream, _getOptions?.Invoke());

    #endregion Generic

    #region NonGeneric

    public void Serialize(Type type, IBufferWriter<Byte> writer, Object value, CancellationToken cancellationToken)
    {
        using var jsonWriter = new Utf8JsonWriter(writer);
        JsonSerializer.Serialize(jsonWriter, value, type, _getOptions?.Invoke());
    }

    public void Serialize(Type type, Stream stream, Object value, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(stream, value, type, _getOptions?.Invoke());

    public Byte[] Serialize(Type type, Object value, CancellationToken cancellationToken)
        => JsonSerializer.SerializeToUtf8Bytes(value, type, _getOptions?.Invoke());

    public Object? Deserialize(Type type, ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());

    public Object? Deserialize(Type type, ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken)
    {
        var jsonReader = new Utf8JsonReader(sequence);
        return JsonSerializer.Deserialize(ref jsonReader, type, _getOptions?.Invoke());
    }

    public Object? Deserialize(Type type, Stream stream, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize(stream, type, _getOptions?.Invoke());

    #endregion NonGeneric

    #endregion ISerializer

    #region ITextSerializer

    #region Generic

    //public void Serialize<T>(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public String SerializeToText<T>(T value, CancellationToken cancellationToken)
        => JsonSerializer.Serialize(value, _getOptions?.Invoke());

    public T? Deserialize<T>(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize<T>(memory.Span, _getOptions?.Invoke());

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
        => JsonSerializer.Serialize(value, type, _getOptions?.Invoke());

    public Object? Deserialize(Type type, ReadOnlyMemory<Char> memory, CancellationToken cancellationToken)
        => JsonSerializer.Deserialize(memory.Span, type, _getOptions?.Invoke());

    //public Object? Deserialize(Type type, ReadOnlySequence<Char> sequence, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    #endregion NonGeneric

    #endregion ITextSerializer
}