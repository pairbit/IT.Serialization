using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncDeserializer<T>
{
    ValueTask<T?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default);
}