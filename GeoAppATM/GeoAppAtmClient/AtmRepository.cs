using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeoAppAtmClient
{
    public class AtmRepository
    {
        private readonly GeoOpenApiClient _openApiClient;

        public AtmRepository()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiServer;
            _openApiClient = new GeoOpenApiClient(baseUrl, httpClient);
        }

        public Task<ICollection<Atm>> GetAtmsAsync()
        {
            return _openApiClient.AtmAsync();
        }

        public Task<AtmBalance> GetAtmBalanceAsync(string atmId)
        {
            return _openApiClient.AtmStatusAsync(atmId);
        }

        public Task UpdateAtmAsync(string atmId, AtmStatus atmStatus)
        {
            return _openApiClient.AtmStatus2Async(atmId, atmStatus);
        }
    }
}
