using System;
using System.Buffers;
using System.Threading;

namespace IT.Serialization;

public interface ITextSerializer<T> : ISerializer<T>
{
    //void Serialize(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken = default);

    String SerializeToText(T value, CancellationToken cancellationToken = default);
}