using Microsoft.Extensions.Logging;
using UltraPlay.Services.Clients.Interfaces;
using UltraPlay.Services.Constants;

namespace UltraPlay.Services.Clients
{
    public class SportsClient : ISportsClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public SportsClient(ILogger<SportsClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        // <inheritdoc />
        public async Task<string> GetXmlSportsDataAsync(CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();

            var uri = new Uri(Constans.XMLSportsDataUrl);

            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            if (response == null)
            {
                throw new Exception("Error while trying to download sports data.");
            }

            if (response.Content == null)
            {
                throw new Exception("Response content is empty.");
            }

            _logger.LogInformation("Sports data retrieved successfully");

            return await response.Content.ReadAsStringAsync(cToken);
        }
    }
}
