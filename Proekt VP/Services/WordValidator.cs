using System.Net.Http;
using System.Threading.Tasks;

namespace Proekt_VP.Services
{
    public static class WordValidator
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<bool> IsValidWordAsync(string word)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"https://api.dictionaryapi.dev/api/v2/entries/en/{word.ToLower()}"
                );
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return true;
            }
        }
    }
}
