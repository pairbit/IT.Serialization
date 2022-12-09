using System;
using System.Buffers;
using System.Threading;

namespace IT.Serialization;

public interface ITextDeserializer : IDeserializer
{
    #region Generic

    T? Deserialize<T>(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken = default);

    //T? Deserialize<T>(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken = default);

    #endregion Generic

    #region NonGeneric

    Object? Deserialize(Type type, ReadOnlyMemory<Char> memory, CancellationToken cancellationToken = default);

    //Object? Deserialize(Type type, ReadOnlySequence<Char> sequence, CancellationToken cancellationToken = default);

    #endregion NonGeneric
}