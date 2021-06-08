using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Batches
{
    public interface IXRMBatchAsyncComposable
    {
        Task<HttpContent> ComposeAsync(IDynamicsXrmClient xrmClient);
    }
}
