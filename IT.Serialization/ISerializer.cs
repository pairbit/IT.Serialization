using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface ISerializer : IAsyncSerializer
{
    #region Generic

    void Serialize<T>(in T? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    Byte[] Serialize<T>(in T? value);

    #endregion Generic

    #region NonGeneric

    void Serialize(Type type, Object? value, Stream stream, CancellationToken cancellationToken = default);

    void Serialize<TBufferWriter>(Type type, Object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>;
#else
         where TBufferWriter : class, IBufferWriter<byte>;
#endif

    Byte[] Serialize(Type type, Object? value);

    #endregion NonGeneric
}