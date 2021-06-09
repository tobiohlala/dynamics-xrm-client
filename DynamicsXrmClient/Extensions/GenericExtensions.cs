using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynamicsXrmClient.Extensions
{
    internal static class GenericExtensions
    {
        internal static async Task<HttpContent> GetHttpContent<T>(this T row, JsonSerializerOptions options)
        {
            var body = new MemoryStream();

            await JsonSerializer.SerializeAsync<T>(body, row, options);

            body.Position = 0;

            HttpContent content = new StreamContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }
    }
}
