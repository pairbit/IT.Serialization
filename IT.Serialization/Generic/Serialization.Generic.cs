using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization.Generic;

public abstract class Serialization<T> : ISerialization<T>
{
    public virtual ValueTask SerializeAsync(in T? value, Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(in value);

#if NETSTANDARD2_0
        return new(stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken));
#else
        return stream.WriteAsync(bytes, cancellationToken);
#endif
    }

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

    public abstract byte[] Serialize(in T? value);

    public virtual async ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var len = checked((int)stream.Length);
        var pool = ArrayPool<byte>.Shared;
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

    public virtual int Deserialize(Stream stream, ref T? value, CancellationToken cancellationToken = default)
    {
        var len = checked((int)stream.Length);

        var pool = ArrayPool<byte>.Shared;

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

    public abstract int Deserialize(ReadOnlySpan<byte> span, ref T? value);

    public virtual int Deserialize(ReadOnlyMemory<byte> memory, ref T? value) => Deserialize(memory.Span, ref value);

    public virtual int Deserialize(in ReadOnlySequence<byte> sequence, ref T? value)
    {
        if (sequence.IsSingleSegment)
        {
#if NETSTANDARD2_0
            return Deserialize(sequence.First.Span, ref value);
#else
            return Deserialize(sequence.FirstSpan, ref value);
#endif
        }

        Span<byte> span = stackalloc byte[(int)sequence.Length];

        sequence.CopyTo(span);

        return Deserialize(span, ref value);
    }
}