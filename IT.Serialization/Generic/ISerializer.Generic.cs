using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.Generic;

public interface ISerializer<T>
{
    ValueTask SerializeAsync(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    byte[] Serialize(in T? value);
}