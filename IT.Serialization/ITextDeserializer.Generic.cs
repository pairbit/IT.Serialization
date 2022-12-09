using System;
using System.Buffers;
using System.Threading;

namespace IT.Serialization;

public interface ITextDeserializer<T> : IDeserializer<T>
{
    T? Deserialize(ReadOnlyMemory<Char> memory, CancellationToken cancellationToken = default);

    //T? Deserialize(in ReadOnlySequence<Char> sequence, CancellationToken cancellationToken = default);
}