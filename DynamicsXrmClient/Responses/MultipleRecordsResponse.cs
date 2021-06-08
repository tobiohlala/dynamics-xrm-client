using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DynamicsXrmClient.Responses
{
    internal sealed class MultipleRecordsResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Results { get; set; }
    }
}
