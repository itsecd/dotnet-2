using TaskClientWPF.Properties;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskClientWPF
{
    public class TaskRepositoryClient
    {
        private readonly TaskClient _taskClient;

        public TaskRepositoryClient()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiClient;
            _taskClient = new TaskClient(baseUrl,httpClient);
        }

    }
}
