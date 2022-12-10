using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Serialization;

public interface IAsyncSerializer<T>
{
    ValueTask SerializeAsync(T? value, Stream stream, CancellationToken cancellationToken = default);
}