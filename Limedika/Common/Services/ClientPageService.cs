using Common.Entities;
using Common.Entities.Comparers;
using Common.Interfaces;
using LinqToDB.Data;

namespace Common.Services
{
    public class ClientPageService : IClientPageService
    {
        private readonly IClientService _clientService;
        private readonly ILogService _logService;
        private readonly IPostCodeService _postCodeService;
        public ClientPageService(IClientService clientService, ILogService logService, IPostCodeService postCodeService)
        {
            _clientService = clientService;
            _logService = logService;
            _postCodeService = postCodeService;
        }

        public async Task<int> UpdatePostCodes()
        {
            var clients = await _clientService.GetClients();
            foreach (var client in clients)
            {
                var oldCode = client.PostCode;
                var newCode = await _postCodeService.GetPostCode(client.Address);

                client.PostCode = newCode;

                await _logService.InsertLog(new Log() 
                { 
                    Id = Guid.NewGuid(), 
                    TimeStamp = DateTime.Now, 
                    UserAction = $"PostCode updated from {oldCode} to {newCode} for client {client.Id}" 
                });
            }

            return await _clientService.BulkUpdateClientsPostCodes(clients);
        }

        public async Task<BulkCopyRowsCopied> ImportClients(List<Client> newClients)
        {
            var existingClients = await _clientService.GetClients();

            var filteredClients = newClients.Except(existingClients, new ClientComparer()).ToList();

            foreach (var client in newClients)
            {
                await _logService.InsertLog(new Log()
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTime.Now,
                    UserAction = $"Client added {client.Id}"
                });
            }

            return await _clientService.BulkInsertClients(filteredClients);
        }
    }
}
