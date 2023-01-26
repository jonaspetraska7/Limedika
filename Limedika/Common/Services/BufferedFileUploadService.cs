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
                return await UploadFile(stream);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Client>?> UploadFile(Stream stream)
        {
            try
            {
                List<Client>? clients = await JsonSerializer.DeserializeAsync<List<Client>>(stream);
                return clients;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
