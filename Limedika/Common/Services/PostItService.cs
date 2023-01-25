using Common.Interfaces;

namespace Common.Services
{
    public class PostItService : IPostCodeService
    {
        public HttpClient Client { get; }
        public PostItService(HttpClient client)
        {
            Client = client;
        }
        public async Task<string> GetPostCode(string address)
        {
            var response = await Client.GetAsync(address);
            var json = await response.Content.ReadAsStringAsync();

            return json;
        }
    }
}
