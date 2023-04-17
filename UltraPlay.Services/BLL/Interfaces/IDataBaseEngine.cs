using UltraPlay.Services.Utils.XMLReader.Models;
namespace UltraPlay.Services.BLL.Interfaces
{
    public interface IDatabaseEngine
    {
        public Task UpdateDatabaseAsync(SportDTO[] sports, CancellationToken cToken);
    }
}
