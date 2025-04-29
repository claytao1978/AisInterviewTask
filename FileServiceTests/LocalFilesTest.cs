using AisUriProviderApi;

namespace FileServiceTests
{
    public class LocalFilesTest : IDisposable
    {
        private readonly string _testDirectory;

        public LocalFilesTest()
        {             
            _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        [Fact]
        public void UriService_Test()
        {                
            AisUriProvider uriService = new ();
            IEnumerable<Uri> result = uriService.Get();
                         
            Assert.True(result != null);
        }

        [Fact]
        public void LoadLocalFileNames_ReturnsFileNamesOnly()
        {         
            var expectedFileNames = new List<string> { "file1.txt", "file2.txt" };
                         
            foreach (var fileName in expectedFileNames)
            {
                File.WriteAllText(Path.Combine(_testDirectory, fileName), "Test content");
            }
                   
            var result = LoadLocalFileNames(_testDirectory);
           
            Assert.Equal(expectedFileNames.Count, result.Count);
            Assert.True(expectedFileNames.All(name => result.Contains(name)));
        }

        [Fact]
        public void LoadLocalFileNames_ReturnsEmptyList_WhenDirectoryIsEmpty()
        {
            var result = LoadLocalFileNames(_testDirectory);                      
            Assert.Empty(result);
        }

        [Fact]
        public void LoadLocalFileNames_CreatesDirectory_WhenMissing()
        {           
            var missingDir = Path.Combine(_testDirectory, "missingdirectory");           
            var result = LoadLocalFileNames(missingDir);
  
            Assert.True(Directory.Exists(missingDir));
            Assert.Empty(result);
        }

        public static List<string> LoadLocalFileNames(string folderPath)
        {
            var fileNames = new List<string>();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder '{folderPath}' does not exist. Creating now...");
                Directory.CreateDirectory(folderPath);
                return fileNames; 
            }

            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                fileNames.Add(Path.GetFileName(filePath)); 
            }

            return fileNames;
        }

        public void Dispose()
        {
          
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
              
            GC.SuppressFinalize(this);
        }       
    }
}

