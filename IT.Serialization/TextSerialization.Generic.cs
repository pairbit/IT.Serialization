using System;
using System.Buffers;
using System.Text;

namespace IT.Serialization;

public abstract class TextSerialization<T> : Serialization<T>, ITextSerialization<T>
{
    private readonly Encoding _encoding;

    public TextSerialization(Encoding? encoding = null)
    {
        _encoding = encoding ?? Encoding.UTF8;
    }

    #region ISerializer

    public override Byte[] Serialize(in T? value)
        => _encoding.GetBytes(SerializeToText(in value));

    public override Int32 Deserialize(ReadOnlySpan<Byte> span, ref T? value)
    {
        var len = _encoding.GetCharCount(span);

        var pool = ArrayPool<Char>.Shared;

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

    public override Int32 Deserialize(ReadOnlyMemory<Byte> memory, ref T? value) => Deserialize(memory.Span, ref value);

    #endregion ISerializer

    #region ITextSerializer

    public virtual void SerializeToText<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<char>
#else
         where TBufferWriter : class, IBufferWriter<char>
#endif
        => writer.Write(SerializeToText(in value).AsSpan());

    public abstract String SerializeToText(in T? value);

    public abstract Int32 Deserialize(ReadOnlySpan<Char> span, ref T? value);

    public virtual Int32 Deserialize(ReadOnlyMemory<Char> memory, ref T? value)
        => Deserialize(memory.Span, ref value);

    public virtual Int32 Deserialize(in ReadOnlySequence<Char> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<Char> span = stackalloc char[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

    #endregion ITextSerializer
}