using DynamicsXrmClient.Batches;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicsXrmClient
{
    public interface IDynamicsXrmClient
    {
        /// <summary>
        /// Creates a table row in dataverse.
        /// </summary>
        /// <param name="row">
        /// The table row to create.
        /// </param>
        /// <returns> The <see cref="Guid"/> of the table row created</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/create-entity-web-api"/>
        /// </remarks>
        public Task<Guid> CreateAsync<T>(T row);

        /// <summary>
        /// Updates a table row.
        /// </summary>
        /// <param name="row">
        /// The table row to update.
        /// </param>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api"/>
        /// </remarks>
        public Task UpdateAsync<T>(T row);

        /// <summary>
        /// Upserts an table row.
        /// </summary>
        /// <param name="row">
        /// The table row to upsert.
        /// </param>
        /// <returns> The <see cref="Guid"/> of the table row upserted</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api#upsert-an-entity"/>
        /// </remarks>
        public Task<Guid> UpsertAsync<T>(T row);

        /// <summary>
        /// Deletes an table row.
        /// </summary>
        /// <param name="row">
        /// The table row to delete.
        /// </param>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api"/>
        /// </remarks>
        public Task DeleteAsync<T>(T row);

        /// <summary>
        /// Retrieves a single table row.
        /// </summary>
        /// <typeparam name="T">The table row to query</typeparam>
        /// <param name="id">The <see cref="Guid"/> of the table row to retrieve</param>
        /// <param name="options">OData system query options as supported by the Xrm Web API</param>
        /// <returns>A table row of <typeparamref name="T"/>.</returns>
        /// <remarks>
        public async Task<T> RetrieveAsync<T>(Guid id, string options = "")
        {
            return await RetrieveAsync<T>(id.ToString(), options);
        }

        /// <summary>
        /// Retrieves a single table row.
        /// </summary>
        /// <typeparam name="T">The table row to query</typeparam>
        /// <param name="id">The id of the table row to retrieve</param>
        /// <param name="options">OData system query options as supported by the Xrm Web API</param>
        /// <returns>A table row of <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/retrieve-entity-using-web-api"/>
        /// </remarks>
        public Task<T> RetrieveAsync<T>(string id, string options = "");

        /// <summary>
        /// Retrieves a collection of table rows.
        /// </summary>
        /// <typeparam name="T">The table row to query</typeparam>
        /// <param name="options">OData system query options as supported by the Xrm Web API</param>
        /// <returns>A list of table rows of <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// see <a href="https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/retrieve-entity-using-web-api"/>
        /// </remarks>
        public Task<List<T>> RetrieveMultipleAsync<T>(string options);

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
