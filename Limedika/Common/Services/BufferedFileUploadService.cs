using Common.Entities;
using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Common.Services
{
    public class BufferedFileUploadService : IBufferedFileUploadService
    {
        public async Task<List<Client>?> UploadFile(IFormFile file)
        {
            if(file != null)
            {
                using Stream stream = file.OpenReadStream();
                List<Client>? clients = await JsonSerializer.DeserializeAsync<List<Client>>(stream);

                return clients;
            }
            else
            {
                return null;
            }
        }
    }
}
