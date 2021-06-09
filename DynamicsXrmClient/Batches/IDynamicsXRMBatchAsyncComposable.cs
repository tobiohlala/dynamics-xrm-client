using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Batches
{
    public interface IDynamicsXRMBatchAsyncComposable
    {
        Task<HttpContent> ComposeAsync(DynamicsXrmConnectionParams connectionParams, JsonSerializerOptions options);
    }
}
