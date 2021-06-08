using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Batches
{
    public class ChangeSet : IBatchRequest, IEnumerable<IChangeSetRequest>
    {
        private List<IChangeSetRequest> _requests;

        public string Id { get; set; } = Guid.NewGuid().ToString();


        public ChangeSet()
        {
            _requests = new List<IChangeSetRequest>();
        }

        public IEnumerator<IChangeSetRequest> GetEnumerator()
        {
            return _requests.GetEnumerator();
        }

        public void Add(IChangeSetRequest request)
        {
            _requests.Add(request);
        }

        public void Add(IEnumerable<IChangeSetRequest> requests)
        {
            if (requests.Count() == 0)
            {
                return;
            }

            _requests.AddRange(requests);
        }

        public async Task<HttpContent> ComposeAsync(IDynamicsXrmClient xrmClient)
        {
            var content = new MultipartContent("mixed", $"changeset_{Id}");

            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", $"multipart/mixed;boundary=changeset_{Id}");

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
