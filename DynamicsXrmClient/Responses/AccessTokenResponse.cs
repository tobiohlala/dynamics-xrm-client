using System.Text.Json.Serialization;

namespace DynamicsXrmClient.Responses
{
    internal sealed class AccessTokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
