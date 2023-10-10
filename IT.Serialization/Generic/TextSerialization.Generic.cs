using System;
using System.Buffers;
using System.Text;

namespace IT.Serialization.Generic;

public abstract class TextSerialization<T> : Serialization<T>, ITextSerialization<T>
{
    private readonly Encoding _encoding;

    public TextSerialization(Encoding? encoding = null)
    {
        _encoding = encoding ?? Encoding.UTF8;
    }

    #region ISerializer

    public override byte[] Serialize(in T? value)
        => _encoding.GetBytes(SerializeToText(in value));

    public override int Deserialize(ReadOnlySpan<byte> span, ref T? value)
    {
        var len = _encoding.GetCharCount(span);

        var pool = ArrayPool<char>.Shared;

        var rented = pool.Rent(len);

        var rentedSpan = rented.AsSpan(0, len);

        try
        {
            _encoding.GetChars(span, rentedSpan);

            Deserialize(rentedSpan, ref value);

            return span.Length;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public override int Deserialize(ReadOnlyMemory<byte> memory, ref T? value) => Deserialize(memory.Span, ref value);

    #endregion ISerializer

    #region ITextSerializer

    public virtual void SerializeToText<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(SerializeToText(in value).AsSpan());

    public abstract string SerializeToText(in T? value);

    public abstract int Deserialize(ReadOnlySpan<char> span, ref T? value);

    public virtual int Deserialize(ReadOnlyMemory<char> memory, ref T? value)
        => Deserialize(memory.Span, ref value);

    public virtual int Deserialize(in ReadOnlySequence<char> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion ITextSerializer
}