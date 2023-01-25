using Common.Data;
using Common.Entities;
using Common.Interfaces;
using LinqToDB;
using LinqToDB.Data;

namespace Common.Services
{
    public class ClientService : IClientService
    {
        private readonly LimedikaDataConnection _connection;
        public ClientService(LimedikaDataConnection connection)
        {
            _connection = connection;
        }

        public Task<List<Client>> GetClients()
        {
            return _connection.Clients.ToListAsync();
        }

        public Task<Client?> GetClient(Guid? id)
        {
            return _connection.Clients.SingleOrDefaultAsync(client => client.Id == id);
        }

        public Task<int> DeleteClient(Guid id)
        {
            return _connection.Clients.Where(client => client.Id == id).DeleteAsync();
        }

        public Task<int> UpdateClient(Client client)
        {
            return _connection.UpdateAsync(client);
        }

        public Task<int> InsertClient(Client client)
        {
            return _connection.InsertAsync(client);
        }

        public Task<BulkCopyRowsCopied> BulkInsertClients(List<Client> clients)
        {
            return _connection.Clients.BulkCopyAsync(new BulkCopyOptions() { TableOptions = TableOptions.CreateIfNotExists }, clients);
        }

        public Task<int> BulkUpdateClientsPostCodes(List<Client> updatedClients)
        {
            return _connection.BulkUpdateAsync(updatedClients, _connection.Clients, x => x.PostCode);
        }
    }
}
