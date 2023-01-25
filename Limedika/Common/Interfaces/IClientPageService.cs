using Common.Entities;
using LinqToDB.Data;

namespace Common.Interfaces
{
    public interface IClientPageService
    {
        /// <summary>
        /// Method for updating post codes for all clients
        /// </summary>
        /// <returns>A status</returns>
        Task<int> UpdatePostCodes();
        /// <summary>
        /// Method for importing clients
        /// </summary>
        /// <param name="clients"></param>
        /// <returns>Information about rows copied</returns>
        Task<BulkCopyRowsCopied> ImportClients(List<Client> clients);
    }
}