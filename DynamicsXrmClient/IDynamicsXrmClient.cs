using DynamicsXrmClient.Batches;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynamicsXrmClient
{
    public interface IDynamicsXrmClient
    {
        /// <summary>
        /// Connection settings for the Dynamics365 instance this client connects to.
        /// </summary>
        public DynamicsConnectionParams ConnectionParams { get; }

        /// <summary>
        /// Serialization options used when exchanging records with the web api.
        /// </summary>
        public JsonSerializerOptions Options { get; }

        /// <summary>
        /// Creates an entity record.
        /// </summary>
        /// <param name="entity">
        /// The entity record to create.
        /// </param>
        /// <returns> The <see cref="Guid"/> of the entity record created</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/create-entity-web-api"/>
        /// </remarks>
        public Task<Guid> CreateAsync<T>(T entity) where T: IXRMEntity;

        /// <summary>
        /// Updates an entity record.
        /// </summary>
        /// <param name="entity">
        /// An instance of the entity record to update.
        /// </param>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api"/>
        /// </remarks>
        public Task UpdateAsync<T>(T entity) where T : IXRMEntity;

        /// <summary>
        /// Upserts an entity record.
        /// </summary>
        /// <param name="entity">
        /// An instance of the entity record to upsert.
        /// </param>
        /// <returns> The <see cref="Guid"/> of the entity record upserted</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api#upsert-an-entity"/>
        /// </remarks>
        public Task<Guid> UpsertAsync<T>(T entity) where T : IXRMEntity;

        /// <summary>
        /// Deletes an entity record.
        /// </summary>
        /// <param name="entity">
        /// An instance of the entity record to delete.
        /// </param>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api"/>
        /// </remarks>
        public Task DeleteAsync<T>(T entity) where T : IXRMEntity;

        /// <summary>
        /// Retrieves a single entity record.
        /// </summary>
        /// <typeparam name="T">The entity to query</typeparam>
        /// <param name="id">The <see cref="Guid"/> of the entity record to retrieve</param>
        /// <param name="options">OData system query options as supported by the Xrm Web Api</param>
        /// <returns>Entity record of <typeparamref name="T"/>.</returns>
        /// <remarks>
        public async Task<T> RetrieveAsync<T>(Guid id, string options = "") where T: IXRMEntity
        {
            return await RetrieveAsync<T>(id.ToString(), options);
        }

        /// <summary>
        /// Retrieves a single entity record.
        /// </summary>
        /// <typeparam name="T">The entity to query</typeparam>
        /// <param name="id">The id of the entity record to retrieve</param>
        /// <param name="options">OData system query options as supported by the Xrm Web Api</param>
        /// <returns>Entity record of <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/retrieve-entity-using-web-api"/>
        /// </remarks>
        public Task<T> RetrieveAsync<T>(string id, string options = "") where T: IXRMEntity;

        /// <summary>
        /// Retrieves a collection of entity records.
        /// </summary>
        /// <typeparam name="T">The entity to query</typeparam>
        /// <param name="options">OData system query options as supported by the Xrm Web Api</param>
        /// <returns>Collection of entity records of <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/retrieve-entity-using-web-api"/>
        /// </remarks>
        public Task<List<T>> RetrieveMultipleAsync<T>(string options) where T: IXRMEntity;

        /// <summary>
        /// Executes multiple operations in a single HTTP request using a batch operation.
        /// </summary>
        /// <param name="batch">The batch to execute.</param>
        /// <returns></returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/execute-batch-operations-using-web-api"/>
        /// </remarks>
        public Task ExecuteBatchAsync(Batch batch);
    }
}
