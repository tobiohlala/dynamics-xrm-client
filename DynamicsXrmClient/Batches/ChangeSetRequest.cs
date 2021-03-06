using DynamicsXrmClient.Extensions;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Batches
{
    public interface IChangeSetRequest : IDynamicsXRMBatchAsyncComposable
    {
    }

    public class ChangeSetRequest<T> : IChangeSetRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ChangeSetRequestAction Action { get; set; }

        public T Row { get; set; }

        public ChangeSetRequest(T row, ChangeSetRequestAction action)
        {
            Row = row;
            Action = action;
        }

        public async Task<HttpContent> ComposeAsync(DynamicsXrmConnectionParams connectionParams, JsonSerializerOptions options)
        {
            var (httpMethod, relativeUri) = Action.Resolve(Row);

            var request = new HttpRequestMessage(httpMethod, connectionParams.ServiceRootUri + relativeUri)
            {
                Content = await Row.GetHttpContent(options)
            };

            request.Content.Headers.Remove("Content-Type");
            request.Content.Headers.Add("Content-Type", "application/json;type=entry");

            var content = new HttpMessageContent(request);

            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/http");
            content.Headers.Add("Content-Transfer-Encoding", "binary");
            content.Headers.Add("Content-ID", Id);

            return content;
        }
    }
}
