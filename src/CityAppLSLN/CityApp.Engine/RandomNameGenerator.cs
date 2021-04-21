using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CityApp.Engine
{
    public class RandomNameGenerator
    {
        public static async Task<string> GetRandomNameAsync(string randomNameGeneratorUrl)
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(randomNameGeneratorUrl);
            if (string.IsNullOrEmpty(result)) return Guid.NewGuid().ToString();
                
            var currentName = JObject.Parse(result);
            return currentName["results"]["login"]["username"].ToString();
        }
    }
}