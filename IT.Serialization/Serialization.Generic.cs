using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public abstract class Serialization<T> : ISerialization<T>
{
    #region IAsyncSerializer

    public virtual Task SerializeAsync(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value, cancellationToken);
        return stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
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

            return Deserialize(rented.AsMemory(0, len), cancellationToken);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    #endregion IAsyncSerializer

    #region ISerializer

    public virtual void Serialize(IBufferWriter<Byte> writer, T value, CancellationToken cancellationToken = default)
        => writer.Write(Serialize(value, cancellationToken));

    public virtual void Serialize(Stream stream, T value, CancellationToken cancellationToken = default)
    {
        var bytes = Serialize(value, cancellationToken);
        stream.Write(bytes, 0, bytes.Length);
    }

    public abstract Byte[] Serialize(T value, CancellationToken cancellationToken = default);

    public abstract T? Deserialize(ReadOnlyMemory<Byte> memory, CancellationToken cancellationToken = default);

    public virtual T? Deserialize(in ReadOnlySequence<Byte> sequence, CancellationToken cancellationToken = default)
    {
        if (!sequence.IsSingleSegment) throw new NotImplementedException();
        return Deserialize(sequence.First, cancellationToken);
    }

    public virtual T? Deserialize(Stream stream, CancellationToken cancellationToken = default)
    {
        var len = checked((Int32)stream.Length);

        var pool = ArrayPool<Byte>.Shared;

        var rented = pool.Rent(len);
        try
        {
            var readed = stream.Read(rented, 0, len);

            if ((uint)readed > (uint)len) throw new IOException("IO_StreamTooLong");

            return Deserialize(rented.AsMemory(0, len), cancellationToken);
        }
        finally
        {
            pool.Return(rented);
        }
    }

    #endregion ISerializer
}