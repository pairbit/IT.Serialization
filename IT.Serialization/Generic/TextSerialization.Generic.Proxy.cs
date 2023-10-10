using System;
using System.Buffers;

namespace IT.Serialization.Generic;

public class TextSerializationProxy<T> : SerializationProxy<T>, ITextSerialization<T>
{
    private readonly ITextSerialization _textSerialization;

    public TextSerializationProxy(ITextSerialization textSerialization) : base(textSerialization)
    {
        _textSerialization = textSerialization;
    }

    #region ITextSerialization

    public virtual void SerializeToText<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => _textSerialization.SerializeToText(in value, in writer);

    public string SerializeToText(in T? value)
        => _textSerialization.SerializeToText(in value);

    public int Deserialize(ReadOnlySpan<char> span, ref T? value)
        => _textSerialization.Deserialize(span, ref value);

    public int Deserialize(ReadOnlyMemory<char> memory, ref T? value)
        => _textSerialization.Deserialize(memory, ref value);

    public int Deserialize(in ReadOnlySequence<char> sequence, ref T? value)
        => _textSerialization.Deserialize(in sequence, ref value);

    #endregion ITextSerialization
}