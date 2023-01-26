using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Common.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<List<Client>?> UploadFile(IFormFile file);
        Task<List<Client>?> UploadFile(Stream stream);
    }
}
