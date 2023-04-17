using UltraPlay.Services.Utils.XMLReader.Models;

namespace UltraPlay.Data.Interfaces
{
    public interface ISportRepository
    {
        public Task UpdataDatabaseAsync(SportDTO[] sports, CancellationToken cToken);
    }
}
