using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface ISerializer
{
    #region Generic

    ValueTask SerializeAsync<T>(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    byte[] Serialize<T>(in T? value);

    #endregion Generic

    #region NonGeneric

    ValueTask SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize(Type type, object? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    byte[] Serialize(Type type, object? value);

    #endregion NonGeneric
}