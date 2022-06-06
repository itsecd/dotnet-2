using GeoAppAtmClient.Properties;
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
            return _openApiClient.AtmAllAsync();
        }

        public Task<Atm> GetAtmAsync(string atmId)
        {
            return _openApiClient.AtmAsync(atmId);
        }

        public Task<Atm> ChangeAtmBalanceAsync(string atmId, int atmBalance)
        {
            return _openApiClient.Atm2Async(atmId, atmBalance);
        }
    }
}
