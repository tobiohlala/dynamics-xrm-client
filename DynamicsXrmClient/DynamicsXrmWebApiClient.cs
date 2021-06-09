using System;
using System.Net;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using DynamicsXrmClient.Responses;
using DynamicsXrmClient.Exceptions;
using DynamicsXrmClient.Batches;
using DynamicsXrmClient.Extensions;

namespace DynamicsXrmClient
{
    /// <summary>
    /// <see cref="HttpClient"/> wrapper to communicate with the Dynamics365 Xrm Web Api.
    /// </summary>
    public sealed class DynamicsXrmWebApiClient : IDynamicsXrmClient
    {
        private readonly HttpClient _httpClient;

        private DynamicsXrmWebApiClient(HttpClient httpClient) => _httpClient = httpClient;

        ///<inheritdoc/>
        public DynamicsConnectionParams ConnectionParams { get; set; }

        public JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };

        /// <summary>
        /// Factory method to create and initialize a generic Dynamics365 <see cref="DynamicsXrmWebApiClient">
        /// using an OAuth2 Credential Grant Flow establishing a service-to-service connection.
        /// </summary>
        /// <returns>
        /// A ready-to-use <see cref="DynamicsXrmWebApiClient"> set-up with a valid access token to
        /// query the Dynamics 365 Xrm Web Api.
        /// </returns>
        /// <param name="connection">
        /// A <see cref="DynamicsConnectionParams"/> instance containing the connection information.
        /// </param>
        /// <remarks>
        /// see also <a href="https://docs.microsoft.com/en-us/azure/active-directory/develop/v1-oauth2-client-creds-grant-flow"/>
        /// </remarks>
        public static async Task<DynamicsXrmWebApiClient> ConnectAsync(DynamicsConnectionParams connection)
        {
            using var client = new HttpClient();

            string serviceRootBaseUri = new Uri(connection.ServiceRootUri)
                .GetLeftPart(UriPartial.Authority);

            // Build an access token request with a shared secret.
            var formContent = new FormUrlEncodedContent(new[]
            {
                // Dynamics365 instance service root uri.
                new KeyValuePair<string, string>("resource", serviceRootBaseUri),

                // Azure AD App Registration client id.
                new KeyValuePair<string, string>("client_id", connection.ClientId),

                // Azure AD App Registration client secret.
                new KeyValuePair<string, string>("client_secret", connection.ClientSecret),

                // Grant type in a Client Credentials Grant Flow must be 'client_credentials'.
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // Request an access token.
            HttpResponseMessage response = await client
                .PostAsync($"https://login.microsoftonline.com/{connection.TenantId}/oauth2/token", formContent);

            response.EnsureSuccessStatusCode();

            // Parse access token.
            var content = await response.Content.ReadAsStreamAsync();

            var accessTokenResponse = await JsonSerializer.DeserializeAsync<AccessTokenResponse>(content);

            // Create and initialize http client.
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(connection.ServiceRootUri),
                Timeout = TimeSpan.FromMinutes(5)
            };

            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken);

            return new DynamicsXrmWebApiClient(httpClient)
            {
                ConnectionParams = connection
            };
        }

        ///<inheritdoc/>
        public async Task<Guid> CreateAsync<T>(T entity) where T : IXRMEntity
        {
            // Create http content containing the json representation of the record.
            using HttpContent content = await entity.GetHttpContent(Options);

            // Query the web api
            HttpResponseMessage response = await _httpClient
                .PostAsync($"{entity.GetType().GetLogicalCollectionName()}", content);

            try
            {
                // Throw exception if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();

                var id = response.Headers.Location.AbsoluteUri.Split('(', ')')[1];

                return new Guid(id);
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task UpdateAsync<T>(T entity) where T : IXRMEntity
        {
            // Create http content containing the json representation of the record.
            using HttpContent content = await entity.GetHttpContent(Options);

            // Query the web api.
            HttpResponseMessage response = await _httpClient
                .PatchAsync($"{entity.GetType().GetLogicalCollectionName()}({entity.Id})", content);

            try
            {
                // Throw exception if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task<Guid> UpsertAsync<T>(T entity) where T : IXRMEntity
        {
            // Create http content containing the json representation of the record.
            using HttpContent content = await entity.GetHttpContent(Options);

            // Query the web api.
            HttpResponseMessage response = await _httpClient
                .PatchAsync($"{entity.GetType().GetLogicalCollectionName()}({entity.Id})", content);

            try
            {
                // Throw exception if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();

                var id = response.Headers.Location.AbsoluteUri.Split('(', ')')[1];

                return new Guid(id);
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task DeleteAsync<T>(T entity) where T : IXRMEntity
        {
            // Query the web api.
            HttpResponseMessage response = await _httpClient
                .DeleteAsync($"{entity.GetType().GetLogicalCollectionName()}({entity.Id})");

            try
            {
                // Throw exception if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task<T> RetrieveAsync<T>(string id, string options = "") where T: IXRMEntity
        {
            // Query the web api.
            HttpResponseMessage response = await _httpClient
                .GetAsync($"{typeof(T).GetLogicalCollectionName()}({id}){options}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            // Parse web api response as stream.
            var content = await response.Content.ReadAsStreamAsync();

            try
            {
                // Try parsing a record from json.
                var record = await JsonSerializer.DeserializeAsync<T>(content, Options);

                // Throw if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();

                return record;
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task<List<T>> RetrieveMultipleAsync<T>(string options = "") where T: IXRMEntity
        {
            // Query the web api.
            HttpResponseMessage response = await _httpClient
                .GetAsync($"{typeof(T).GetLogicalCollectionName()}{options}");

            // Parse web api response as stream.
            var content = await response.Content.ReadAsStreamAsync();

            try
            {
                // Try parsing a collection of records from json.
                var records = await JsonSerializer.DeserializeAsync<MultipleRecordsResponse<T>>(content, Options);

                // Throw if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();

                if (records.NextLink != null)
                {
                    // Results span multiple pages, retrieve recursively.

                    var nextOptions = records
                        .NextLink
                        .Replace($"{_httpClient.BaseAddress}{typeof(T).GetLogicalCollectionName()}", string.Empty);

                    var nextRecords = await RetrieveMultipleAsync<T>(nextOptions);

                    return records.Results.Concat(nextRecords).ToList();
                }

                // Cannot be null here as the web api either returns results or
                // an error. The latter is handled above by ensuring an http
                // success status code and any json errors are catched, too.
                return records.Results;
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }

        ///<inheritdoc/>
        public async Task ExecuteBatchAsync(Batch batch)
        {
            // Create http content containing the batch request.
            HttpContent content = await batch.ComposeAsync(this);

            // Query the web api.
            HttpResponseMessage response = await _httpClient.PostAsync("$batch", content);

            try
            {
                // Throw exception if the http request failed or the web api returned an error.
                response.EnsureSuccessStatusCode();

                // TODO parse response.
            }
            catch (Exception exception)
            {
                throw new DynamicsXrmWebApiException(response, exception);
            }
        }
    }
}
