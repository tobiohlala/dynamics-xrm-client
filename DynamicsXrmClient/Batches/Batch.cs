using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Batches
{
    public class Batch : IEnumerable<IBatchRequest>, IXRMBatchAsyncComposable
    {
        private readonly List<IBatchRequest> _requests;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Batch()
        {
            _requests = new List<IBatchRequest>();
        }

        public IEnumerator<IBatchRequest> GetEnumerator()
        {
            return _requests.GetEnumerator();
        }

        public void Add(IBatchRequest request)
        {
            _requests.Add(request);
        }

        public void Add(IEnumerable<IBatchRequest> requests)
        {
            if (requests.Count() == 0)
            {
                return;
            }

            _requests.AddRange(requests);
        }

        public async Task<HttpContent> ComposeAsync(IDynamicsXrmClient xrmClient)
        {
            var content = new MultipartContent("mixed", $"batch_{Id}");

            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", $"multipart/mixed;boundary=batch_{Id}");

            foreach (var request in this)
            {
                content.Add(await request.ComposeAsync(xrmClient));
            }

            return content;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
