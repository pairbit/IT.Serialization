using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IDeserializer
{
    #region Generic

    ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);

    int Deserialize<T>(Stream stream, ref T? value, CancellationToken cancellationToken = default);

    int Deserialize<T>(ReadOnlySpan<byte> span, ref T? value);

    int Deserialize<T>(ReadOnlyMemory<byte> memory, ref T? value);

    int Deserialize<T>(in ReadOnlySequence<byte> sequence, ref T? value);

    #endregion Generic

    #region NonGeneric

    ValueTask<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);

    int Deserialize(Type type, Stream stream, ref object? value, CancellationToken cancellationToken = default);

    int Deserialize(Type type, ReadOnlySpan<byte> span, ref object? value);

    int Deserialize(Type type, ReadOnlyMemory<byte> memory, ref object? value);

    int Deserialize(Type type, in ReadOnlySequence<byte> sequence, ref object? value);

    #endregion NonGeneric
}