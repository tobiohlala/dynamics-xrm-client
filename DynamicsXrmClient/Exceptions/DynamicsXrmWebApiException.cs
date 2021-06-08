using DynamicsXrmClient.Responses;
using System;
using System.Net.Http;
using System.Text.Json;

namespace DynamicsXrmClient.Exceptions
{
    public class DynamicsXrmWebApiException : Exception
    {
        public DynamicsXrmWebApiException(HttpResponseMessage response, Exception innerException) :
            base(ParseError(response), innerException)
        {
        }

        private static string ParseError(HttpResponseMessage response)
        {
            // parse web api response as string
            var content = response.Content.ReadAsStringAsync().Result;

            try
            {
                // try parsing a web api error from json
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);

                if (errorResponse.Error != null)
                {
                    return errorResponse.Error.Message!;
                }
            }
            catch
            {
                // return the original http error message
                if (!response.IsSuccessStatusCode)
                {
                    return response.ReasonPhrase;
                }
            }

            return "Unexpected Error";
        }
    }
}
