using Common.Interfaces;
using Newtonsoft.Json.Linq;

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
            var response = await Client.GetAsync(Client.BaseAddress + address);
            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json)["data"];

            if(result != null)
            {
                return result?.First?["post_code"]?.ToString() ?? "NotFound";
            }

            return "NotFound";
        }
    }
}
