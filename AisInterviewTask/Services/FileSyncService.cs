using AisInterviewTask.LocalFile;
using AisInterviewTask.Models;

namespace AisInterviewTask.Services
{
    public class FileSyncService(string jsonPath, string filesPath, AisUriProviderService providerService, DownLoaderService downLoaderService)
    {
        private readonly string _filesPath = filesPath;
        private readonly AisUriProviderService _providerService = providerService;        
        private readonly DownLoaderService _downLoaderService = downLoaderService;

        public async Task SyncFilesAsync()
        {
            if (!Directory.Exists(_filesPath))
                Directory.CreateDirectory(_filesPath);

            var listUris = _providerService.FetchUris().ToList();
            var listFilesFromService = listUris.Select(uri => Path.GetFileName(uri.LocalPath)).ToHashSet();
            var listLocalFiles = LocalStorageFile.Load(jsonPath);
            var localFileNames = listLocalFiles.FileNames.ToHashSet();
            var filesToDownload = listUris;
            var filesToDelete = localFileNames;

            if (filesToDelete.Count > 0)
            {
                Console.WriteLine($"Deleting old files...");

                foreach (var file in filesToDelete)
                {
                    var filePath = Path.Combine(_filesPath, file);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"{file} deleted.");
                    }
                }
            }           
           
           Console.WriteLine($"Loading files..."); 
           await _downLoaderService.DownloadFilesAsync(filesToDownload, _filesPath);

            LocalStorageFile.Save(new Files { FileNames = [.. listFilesFromService] }, jsonPath);
        }
    }
}
