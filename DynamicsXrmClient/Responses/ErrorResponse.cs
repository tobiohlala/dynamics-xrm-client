using System.Text.Json.Serialization;

namespace DynamicsXrmClient.Responses
{
    internal sealed class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }

    internal sealed class Error
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
