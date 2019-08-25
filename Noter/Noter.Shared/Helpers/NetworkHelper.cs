using System;
using System.Net.Http;
using System.Threading.Tasks;
using SQLite;
// import com.google.api.client.auth.oauth2.Credential;
// import com.google.api.services.drive.Drive;

namespace Noter.Shared.DataAccessLayer
{
    public static class NetworkHelper
    {
        public static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(10);

        public static async Task<string> GetResponseAsync(string targetURL) => await GetResponseAsync(targetURL, TIMEOUT);

        public static async Task<string> GetResponseAsync(string targetURL, TimeSpan timeout)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = timeout;
                return await client.GetStringAsync(targetURL);
            }
        }

        public static async Task<HttpResponseMessage> PostResponseAsync(string targetURL, string content) => await PostResponseAsync(targetURL, content, TIMEOUT);

        public static async Task<HttpResponseMessage> PostResponseAsync(string targetURL, string content, TimeSpan timeout)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = timeout;
                return await client.PostAsync(targetURL, new StringContent(content));
            }
        }
    }
}
