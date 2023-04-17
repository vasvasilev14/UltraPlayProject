using Microsoft.Extensions.Logging;
using UltraPlay.Data.Interfaces;
using UltraPlay.Services.BLL.Interfaces;
using UltraPlay.Services.Utils.XMLReader.Models;

namespace UltraPlay.Services.BLL
{
    public class DatabaseEngine : IDatabaseEngine
    {
        private readonly ILogger<DatabaseEngine> _logger;
        private readonly ISportRepository _databaseRepository;

        public DatabaseEngine(ILogger<DatabaseEngine> logger, ISportRepository databaseRepository)
        {
            _logger = logger;
            _databaseRepository = databaseRepository;
        }

        public async Task UpdateDatabaseAsync(SportDTO[] sports, CancellationToken cToken)
        {
            cToken.ThrowIfCancellationRequested();
            await _databaseRepository.UpdataDatabaseAsync(sports, cToken);
            _logger.LogInformation("Updating database.");
        }
    }
}
