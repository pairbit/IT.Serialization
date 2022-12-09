using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncSerializer
{
    Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken = default);

    Task SerializeAsync(Type type, Stream stream, Object value, CancellationToken cancellationToken = default);
}