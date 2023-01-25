using Common.Entities;
using LinqToDB.Data;

namespace Common.Interfaces
{
    public interface IClientService
    {
        /// <summary>
        /// Method for getting a list of clients
        /// </summary>
        /// <returns>A list of client objects</returns>
        Task<List<Client>> GetClients();
        /// <summary>
        /// Method for getting a client from the database
        /// </summary>
        /// <param name="id">ID of the client to get</param>
        /// <returns>A Client object or null</returns>
        Task<Client?> GetClient(Guid? id);
        /// <summary>
        /// Method for deleting a client from the database
        /// </summary>
        /// <param name="id">ID of the client to delete</param>
        /// <returns>A status</returns>
        Task<int> DeleteClient(Guid id);
        /// <summary>
        /// Method for updating a client data in the database
        /// </summary>
        /// <param name="client">A client object to update</param>
        /// <returns>A status</returns>
        Task<int> UpdateClient(Client client);
        /// <summary>
        /// Method for inserting a client to the database
        /// </summary>
        /// <param name="client">A client object to insert</param>
        /// <returns>A status</returns>
        Task<int> InsertClient(Client client);
        /// <summary>
        /// Method for bulk inserting clients to the database
        /// </summary>
        /// <param name="clients">A list of clients</param>
        /// <returns>Info about the rows copied</returns>
        Task<BulkCopyRowsCopied> BulkInsertClients(List<Client> clients);
        /// <summary>
        /// Method for bulk updating clients post codes
        /// </summary>
        /// <param name="updatedClients">Client list with updated post codes</param>
        /// <returns>A status</returns>
        Task<int> BulkUpdateClientsPostCodes(List<Client> updatedClients);
    }
}
