using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UltraPlay.Services.BLL.Interfaces;
using UltraPlay.Services.Clients.Interfaces;
using UltraPlay.Services.Constants;
using UltraPlay.Services.Utils.XMLReader.Interfaces;

namespace UltraPlay.Services.Workers
{
    public sealed class SyncWorker : BackgroundService
    {
        private readonly ILogger<SyncWorker> _logger;
        private readonly IServiceScopeFactory _factory;

        public SyncWorker(ILogger<SyncWorker> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            var counter = 1;
            while (!cToken.IsCancellationRequested)
            {
                await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                IXMLReader xmlReader = asyncScope.ServiceProvider.GetRequiredService<IXMLReader>();
                ISportsClient sportsClient = asyncScope.ServiceProvider.GetRequiredService<ISportsClient>();
                IDatabaseEngine databaseEngine = asyncScope.ServiceProvider.GetRequiredService<IDatabaseEngine>();

                var xmlData = await sportsClient.GetXmlSportsDataAsync(cToken);
                var sportDto = xmlReader.ParseSportsDataXML(xmlData);

                await databaseEngine.UpdateDatabaseAsync(sportDto, cToken);

                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}, for {counter} time");
                counter++;

                await Task.Delay(Constans.DownloadIntervalMiliseconds, cToken);
            }
        }
    }
}
