using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public abstract class Serialization<T> : ISerialization<T>
{
    #region IAsyncSerializer

    public virtual ValueTask SerializeAsync(T? value, Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value);

#if NETSTANDARD2_0
        return new(stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken));
#else
        return stream.WriteAsync(bytes, cancellationToken);
#endif
    }

    public virtual async ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var len = checked((Int32)stream.Length);
        var pool = ArrayPool<Byte>.Shared;
        var rented = pool.Rent(len);
        try
        {
            var readed = await stream.ReadAsync(rented, 0, len, cancellationToken).ConfigureAwait(false);

            if ((uint)readed > (uint)len) throw new IOException("IO_StreamTooLong");

            T? value = default;

            Deserialize(rented.AsSpan(0, len), ref value);

            return value;
        }
        finally
        {
            pool.Return(rented);
        }
    }

    #endregion IAsyncSerializer

    #region ISerializer

    public virtual void Serialize(in T? value, Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    public virtual void Serialize<TBufferWriter>(in T? value, in TBufferWriter writer)
#if NET7_0_OR_GREATER
         where TBufferWriter : IBufferWriter<byte>
#else
         where TBufferWriter : class, IBufferWriter<byte>
#endif
        => writer.Write(Serialize(in value));

    public abstract Byte[] Serialize(in T? value);

    public virtual Int32 Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default)
    {
        var len = checked((Int32)stream.Length);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);
        try
        {
            var readed = stream.Read(rented, 0, len);

            if ((uint)readed > (uint)len) throw new IOException("IO_StreamTooLong");

            return Deserialize(rented.AsSpan(0, len), ref value);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    public abstract Int32 Deserialize(ReadOnlySpan<Byte> span, ref T? value);

    public virtual Int32 Deserialize(ReadOnlyMemory<Byte> memory, ref T? value) => Deserialize(memory.Span, ref value);

    public virtual Int32 Deserialize(in ReadOnlySequence<Byte> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<Byte> span = stackalloc Byte[(Int32)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }

#endregion ISerializer
}