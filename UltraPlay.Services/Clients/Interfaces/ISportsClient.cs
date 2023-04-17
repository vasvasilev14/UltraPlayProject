namespace UltraPlay.Services.Clients.Interfaces
{
    public interface ISportsClient
    {
        /// <summary>
        /// Gets Sports data from server as xml.
        /// </summary>
        /// <param name="cToken"></param>
        /// <returns>A string representing the xml data on the server.</returns>
        Task<string> GetXmlSportsDataAsync(CancellationToken cToken);
    }
}
