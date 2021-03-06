using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DynamicsXrmClient.Responses
{
    internal sealed class MultipleRowsResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Results { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        public string NextLink { get; set; }
    }
}
