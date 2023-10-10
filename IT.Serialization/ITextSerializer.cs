using System;
using System.Buffers;

namespace IT.Serialization;

public interface ITextSerializer : ISerializer
{
    #region Generic

    void SerializeToText<T, TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>;
#else
         where TBufferWriter : class, IBufferWriter<char>;
#endif

    string SerializeToText<T>(in T? value);

    #endregion Generic

    #region NonGeneric

    void SerializeToText<TBufferWriter>(Type type, object? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>;
#else
         where TBufferWriter : class, IBufferWriter<char>;
#endif

    string SerializeToText(Type type, object? value);

    #endregion NonGeneric
}