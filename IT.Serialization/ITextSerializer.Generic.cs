using System;
using System.Buffers;

namespace IT.Serialization;

public interface ITextSerializer<T> : ISerializer<T>
{
    void SerializeToText<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>;
#else
         where TBufferWriter : class, IBufferWriter<char>;
#endif

    String SerializeToText(in T? value);
}