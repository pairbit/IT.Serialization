using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface IDeserializer : IAsyncDeserializer
{
    #region Generic

    T? Deserialize<T>(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    T? Deserialize<T>(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default);

    T? Deserialize<T>(Stream stream, CancellationToken cancellationToken = default);

    #endregion Generic

    #region NonGeneric

    Object? Deserialize(Type type, ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    Object? Deserialize(Type type, ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default);

    Object? Deserialize(Type type, Stream stream, CancellationToken cancellationToken = default);

    #endregion NonGeneric
}