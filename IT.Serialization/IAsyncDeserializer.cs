using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncDeserializer
{
    ValueTask<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);

    ValueTask<Object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default);
}