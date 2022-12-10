using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncSerializer
{
    ValueTask SerializeAsync<T>(T? value, Stream stream, CancellationToken cancellationToken = default);

    ValueTask SerializeAsync(Type type, Object? value, Stream stream, CancellationToken cancellationToken = default);
}