
namespace AisInterviewTask.Services
{
    public class DownLoaderService : IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new(3);
        private bool _disposed;

        public async Task DownloadFilesAsync(IEnumerable<Uri> uris, string downloadPath)
        {
            var tasks = new List<Task>();

            foreach (var uri in uris)
            {
                await _semaphore.WaitAsync();

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        Console.WriteLine($"{Path.GetFileName(uri.LocalPath)} downloaded.");
                        using var client = new HttpClient();
                        var data = await client.GetByteArrayAsync(uri);

                        var fileName = Path.GetFileName(uri.LocalPath);
                        var fileFullPath = Path.Combine(downloadPath, fileName);
                        await File.WriteAllBytesAsync(fileFullPath, data);                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error ocurred trying to download {uri}: {ex.Message}");
                    }
                    finally
                    {
                        _semaphore.Release();                       
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _semaphore.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
