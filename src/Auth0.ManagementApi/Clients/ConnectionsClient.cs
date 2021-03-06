using System.Collections.Generic;
using System.Threading.Tasks;
using Auth0.Core.Http;
using Auth0.ManagementApi.Models;

namespace Auth0.ManagementApi.Clients
{
    /// <summary>
    /// Contains all the methods to call the /connections endpoints.
    /// </summary>
    public class ConnectionsClient : ClientBase, IConnectionsClient
    {
        /// <summary>
        /// Creates a new instance of the ClientBase class.
        /// </summary>
        /// <param name="connection">The <see cref="IApiConnection" /> which is used to communicate with the API.</param>
        public ConnectionsClient(IApiConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Creates a new connection according to the request.
        /// </summary>
        /// <param name="request">The request containing the properties for the new connection.</param>
        /// <returns>A <see cref="Connection" /> containing the newly created Connection.</returns>
        public Task<Connection> CreateAsync(ConnectionCreateRequest request)
        {
            return Connection.PostAsync<Connection>("connections", request, null, null, null, null, null);
        }

        /// <summary>
        /// Deletes a connection and all its users.
        /// </summary>
        /// <param name="id">The id of the connection to delete.</param>
        /// <returns>Task.</returns>
        public Task DeleteAsync(string id)
        {
            return Connection.DeleteAsync<object>("connections/{id}", new Dictionary<string, string>
            {
                {"id", id}
            }, null);
        }

        /// <summary>
        /// Deletes a specified connection user by its email 
        /// </summary>
        /// <remarks>
        /// Currently only database connections are supported and you cannot delete all users from specific connection
        /// </remarks>
        /// <param name="id">The identifier of the connection</param>
        /// <param name="email">The email of the user to delete</param>
        /// <returns></returns>
        public Task DeleteUserAsync(string id, string email)
        {
            return Connection.DeleteAsync<object>("connections/{id}/users", new Dictionary<string, string>
            {
                {"id", id}
            },
                new Dictionary<string, string>
                {
                    {"email", email},
                });
        }

        /// <summary>
        /// Retrieves a connection by its <paramref name="id" />
        /// </summary>
        /// <param name="id">The id of the connection to retrieve.</param>
        /// <param name="fields">A comma separated list of fields to include or exclude (depending on include_fields) from the result, empty to retrieve all fields.</param>
        /// <param name="includeFields">True if the fields specified are to be included in the result, false otherwise (defaults to true).</param>
        /// <returns>The <see cref="Connection" />.</returns>
        public Task<Connection> GetAsync(string id, string fields = null, bool includeFields = true)
        {
            return Connection.GetAsync<Connection>("connections/{id}",
                new Dictionary<string, string>
                {
                    {"id", id}
                },
                new Dictionary<string, string>
                {
                    {"fields", fields},
                    {"include_fields", includeFields.ToString().ToLower()}
                }, null, null);
        }

        /// <summary>
        /// Retrieves every connection matching the specified strategy. All connections are retrieved if no strategy is being specified. Accepts a list of fields to include or exclude in the resulting list of connection objects.
        /// </summary>
        /// <param name="strategy">Provide a type of strategy to only retrieve connections with that strategy.</param>
        /// <param name="fields">A comma separated list of fields to include or exclude (depending on include_fields) from the result, empty to retrieve all fields.</param>
        /// <param name="includeFields">True if the fields specified are to be included in the result, false otherwise (defaults to true).</param>
        /// <param name="name">Provide the name of the connection to retrieve</param>
        /// <returns>A list of <see cref="Connection" /> objects matching the strategy.</returns>
        public Task<IList<Connection>> GetAllAsync(string strategy, string fields = null, bool includeFields = true,
            string name = null)
        {
            return Connection.GetAsync<IList<Connection>>("connections", null,
                new Dictionary<string, string>
                {
                    {"strategy", strategy},
                    {"fields", fields},
                    {"include_fields", includeFields.ToString().ToLower()},
                    {"name", name}
                }, null, null);
        }

        /// <summary>
        /// Updates a connection.
        /// </summary>
        /// <param name="id">The id of the connection to update.</param>
        /// <param name="request">The request containing the properties of the connection you wish to update.</param>
        /// <returns>A <see cref="Connection" /> containing the updated connection.</returns>
        public Task<Connection> UpdateAsync(string id, ConnectionUpdateRequest request)
        {
            return Connection.PatchAsync<Connection>("connections/{id}", request, new Dictionary<string, string>
            {
                {"id", id}
            });
        }
    }
}