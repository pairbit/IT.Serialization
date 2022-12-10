using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface ISerializer<T> : IAsyncSerializer<T>
{
    void Serialize(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    Byte[] Serialize(in T? value);
}