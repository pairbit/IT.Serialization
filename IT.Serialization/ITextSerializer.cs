using System;
using System.Buffers;
using System.Threading;

namespace IT.Serialization;

public interface ITextSerializer : ISerializer
{
    #region Generic

    //void Serialize<T>(IBufferWriter<Char> writer, T value, CancellationToken cancellationToken = default);

    String SerializeToText<T>(T value, CancellationToken cancellationToken = default);

    #endregion Generic

    #region NonGeneric

    //void Serialize(Type type, IBufferWriter<Char> writer, Object value, CancellationToken cancellationToken = default);

    String SerializeToText(Type type, Object value, CancellationToken cancellationToken = default);

    #endregion NonGeneric
}