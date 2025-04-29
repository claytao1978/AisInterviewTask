using AisInterviewTask.Services;
using System.Configuration;
internal class Program
{
    private static async Task Main(string[] args)
    {
        string jsonPath = ConfigurationManager.AppSettings["FilePath"] ?? "localFiles.json";
        string filesPath = Path.Combine(Environment.CurrentDirectory, "LocalFiles");
        var providerService = new AisUriProviderService();        
        var downLoaderService = new DownLoaderService();
        var syncService = new FileSyncService(jsonPath, filesPath, providerService, downLoaderService);
        
        await syncService.SyncFilesAsync();        
                
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync())
        {
            await syncService.SyncFilesAsync();
        }        
    }
}
