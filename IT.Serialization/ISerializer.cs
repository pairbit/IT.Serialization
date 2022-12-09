using System;
using System.Buffers;
using System.IO;
using System.Threading;

namespace IT.Serialization;

public interface ISerializer : IAsyncSerializer
{
    #region Generic

    void Serialize<T>(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default);

    void Serialize<T>(Stream stream, T value, CancellationToken cancellationToken = default);

    Byte[] Serialize<T>(T value, CancellationToken cancellationToken = default);

    #endregion Generic

    #region NonGeneric

    void Serialize(Type type, IBufferWriter<Byte> writer, Object value, CancellationToken cancellationToken = default);

    void Serialize(Type type, Stream stream, Object value, CancellationToken cancellationToken = default);

    Byte[] Serialize(Type type, Object value, CancellationToken cancellationToken = default);

    #endregion NonGeneric
}